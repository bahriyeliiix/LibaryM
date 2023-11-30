using Model.DTOs.Book;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookService
    {
        Task<BookForViewWithTotalDto> GetAll(GetAllBookInput input);
        Task<int> CreateOrEdit(CreateOrEditBookDto input);
        Task<BookDto> GetById(int id);
        Task<bool> SoftDelete(int Id);
        Task<bool> CheckBook(int Id);
        Task<bool> UpdateStatus(int Id,Status newSatus);
        Task<bool> CheckBookInLibrary(int Id);
    }
}
