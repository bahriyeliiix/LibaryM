using Model.DTOs.BorrowedBook;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Core
{
    public interface IBusinessUnitOfWork
    {
        IBookService BookService { get; }
        IUserService UserService { get; }
        IBorrowedBookService BorrewedBookService { get; }

        public Task<bool> ToLend(CreateOrEditBorrowedBookDto input);
        public Task<bool> ToReturn(ToReturnDto input);
    }
}
