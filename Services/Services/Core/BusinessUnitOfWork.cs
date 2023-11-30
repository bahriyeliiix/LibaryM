using AutoMapper;
using DAL.Context;
using Model.DTOs.Book;
using Model.DTOs.BorrowedBook;
using Model.Entities;
using Model.Enums;
using Serilog;
using Services.Interfaces;
using Services.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Core
{
    public class BusinessUnitOfWork : IBusinessUnitOfWork
    {
        private readonly IMapper _map;
        public IBookService BookService { get; }
        public IUserService UserService { get; }
        public IBorrowedBookService BorrewedBookService { get; }

        public BusinessUnitOfWork(IMapper map, IBookService bookService, IBorrowedBookService borrewedBookService, IUserService userService)
        {
            _map = map;
            BookService = bookService;
            BorrewedBookService = borrewedBookService;
            UserService = userService;
        }

        public async Task<bool> ToLend(CreateOrEditBorrowedBookDto input)
        {
            try
            {
                var isBookExist = await this.BookService.CheckBook(input.BookId);

                if (isBookExist)
                {
                    var isBookAvailable = await this.BookService.CheckBookInLibrary(input.BookId);

                    if (isBookAvailable)
                    {

                        var userId = await this.UserService.CheckUser(input.BorrowerName);

                        input.UserId = userId;

                        await this.BorrewedBookService.CreateOrEdit(input);

                        var bookStatus = await this.BookService.UpdateStatus(input.BookId, Status.CheckedOut);

                        if (bookStatus)
                        {
                            Log.Information($"Book with ID {input.BookId} status updated successfully.");
                            return true;
                        }
                        else
                        {
                            Log.Warning($"Book with ID {input.BookId} status not updated.");
                            return false;
                        }
                    }
                    else
                    {
                        Log.Warning($"Book with ID {input.BookId} is not available in the library.");
                        return false;
                    }
                }
                else
                {
                    Log.Warning($"Book with ID {input.BookId} not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while updating the book status for Book ID: {input.BookId}");
                throw;
            }
        }

        public async Task<bool> ToReturn(ToReturnDto input)
        {
            try
            {
                var isBookBorrowed = await this.BorrewedBookService.CheckBookBorrewed(input.BookId, input.UserId);

                if (isBookBorrowed)
                {
                    await this.BorrewedBookService.UpdateReturn(input.BookId,input.UserId);

                    var bookStatus = await this.BookService.UpdateStatus(input.BookId, Status.InLibrary);

                    if (bookStatus)
                    {
                        Log.Information($"Book with ID {input.BookId} returned successfully.");
                        return true;
                    }
                    else
                    {
                        Log.Warning($"Book with ID {input.BookId} status not updated after return.");
                        return false;
                    }
                }
                else
                {
                    Log.Warning($"Book with ID {input.BookId} is not currently borrowed by user {input.UserId}.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while processing the return for Book ID: {input.BookId}");
                throw;
            }
        }

    }
}
