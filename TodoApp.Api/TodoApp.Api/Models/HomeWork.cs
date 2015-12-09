using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Api.Models
{
    public class HomeWork
    {
        public int HomeworkId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime  CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Home Home { get; set; }
        public Assistant AssignTo { get; set; }
        public bool IsDone { get; set; }

        public ICollection<HomeWorkProduct> HomeWorks { get; set; }
    }
}
