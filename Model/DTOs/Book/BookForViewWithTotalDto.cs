using Model.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.Book
{
    public class BookForViewWithTotalDto
    {
        public PagedList<GetBookForViewDto> Book { get; set; }
        public int Total { get; set; }
    }
}
