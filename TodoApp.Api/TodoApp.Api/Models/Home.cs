using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoApp.Api.Models
{
    public class Home
    {
        public int HomeId { get; set; }
        public ICollection<HomeWork> Homeworks { get; set; }
        public Owner Owner { get; set; }
        public Address Address { get; set; }
        public ICollection<Assistant> Assistants { get; set; }
    }
}
