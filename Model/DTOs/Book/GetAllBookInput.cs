using Model.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.Book
{
    public class GetAllBookInput : PaginationParams
    {
        public string? FilterText { get; set; }
    }
}
