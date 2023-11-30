using Model.Entities.Core;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string ImageURL { get; set; } 
        public Status Status { get; set; }
    }
}
