using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Api.Models
{
    public class Products
    {
        public int ProductsId { get; set; }
        public string Name { get; set; }
        public ICollection<HomeWorkProduct> Products { get; set; }
    }
}
