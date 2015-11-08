using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoApp.Api.Models
{
    public class Address
    {
        public string Sector { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Municipio { get; set; }
        public string Street { get; set; }
    }
}
