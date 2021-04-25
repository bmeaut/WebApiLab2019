using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLab.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Column("ProductName")]
        public string Name { get; set; }
        public int UnitPrice { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<OrderItem> ProductOrders { get; }
                                        = new List<OrderItem>();

        public ICollection<Order> Orders { get; } = new List<Order>();

        public ShipmentRegion ShipmentRegion { get; set; }

        public byte[] RowVersion { get; set; }
    }

}
