using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLab.API.ProblemDetails
{
    public record Conflict(object CurrentValue, object SentValue);
    
    public class ConcurrencyProblemDetails : StatusCodeProblemDetails
    {
        public Dictionary<string, Conflict> Conflicts { get; }

        public ConcurrencyProblemDetails(DbUpdateConcurrencyException ex) :
            base(StatusCodes.Status409Conflict)
        {
            Conflicts = new Dictionary<string, Conflict>();
            var entry = ex.Entries[0];
            var props = entry.Properties
                .Where(p => !p.Metadata.IsConcurrencyToken).ToArray();
            var currentValues = props.ToDictionary(
                p => p.Metadata.Name, p => p.CurrentValue);

            entry.Reload();

            foreach (var property in props)
            {
                if (!currentValues[property.Metadata.Name].
                    Equals(property.CurrentValue))
                {
                    Conflicts[property.Metadata.Name] = new Conflict
                    (
                        property.CurrentValue,
                        currentValues[property.Metadata.Name]
                    );
                }
            }
        }
    }
}
