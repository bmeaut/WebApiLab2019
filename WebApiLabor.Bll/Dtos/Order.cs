using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLabor.Api.Dtos
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
