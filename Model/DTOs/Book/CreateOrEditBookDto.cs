using Microsoft.AspNetCore.Http;
using Model.DTOs.Core;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.Book
{
    public class CreateOrEditBookDto : BaseDto
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string? ImageURL { get; set; }
        public IFormFile Image { get; set; }
        public int Status { get; set; }
    }
}
