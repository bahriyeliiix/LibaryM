using Model.DTOs.Core;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.Book
{
    public class BookDto : BaseDto
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string ImageURL { get; set; }
        public Status Status { get; set; }
    }
}
