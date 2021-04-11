using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiLab.DAL.Entities;

namespace WebApiLab.BLL.DTO
{
    public record Category(int Id, string Name);

    public record Order(int Id, DateTime OrderDate);    

    public record Product
    {
        public int Id { get; init; }

        [Required(ErrorMessage = "Product name is required.", AllowEmptyStrings = false)]
        public string Name { get; init; }

        [Range(1, int.MaxValue, ErrorMessage = "Unit price must be higher than 0.")]
        public int UnitPrice { get; init; }

        public ShipmentRegion ShipmentRegion { get; init; }
        public int CategoryId { get; init; }
        public Category Category { get; init; }
        public List<Order> Orders { get; init; }
    }
}