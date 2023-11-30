using AutoMapper;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Model.DTOs.Book;
using Model.DTOs.BorrowedBook;
using Model.Entities;
using Serilog;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BorrowedBookService : IBorrowedBookService
    {
        private readonly LibaryMDbContext _dbContext;
        private readonly IMapper _mapper;


        public BorrowedBookService(LibaryMDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<int> CreateOrEdit(CreateOrEditBorrowedBookDto input)
        {
            if (input.Id == 0)
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }
        public async Task<bool> UpdateReturn(int bookId, int userId)
        {
            try
            {
                var borrowedBook = await _dbContext.BorrowedBook
                         .FirstOrDefaultAsync(a => a.UserId == userId || a.BookId == bookId);

                if (borrowedBook is not null)
                {
                    borrowedBook.Returned = true;
                    borrowedBook.ReturnDate = DateTime.Now;

                    var map = _mapper.Map<CreateOrEditBorrowedBookDto>(borrowedBook);
                    var data = await this.Update(map);

                    if (data > 0)
                    {
                        Log.Information($"Book with ID {bookId} returned successfully.");
                        return true;
                    }
                    else
                    {
                        Log.Warning($"Book with ID {bookId} return status not updated.");
                        return false;
                    }
                }
                else
                {
                    Log.Warning($"Borrowed book with Book ID {bookId} and User ID {userId} not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while updating return status for Book ID: {bookId} and User ID: {userId}");
                throw;
            }
        }

        public async Task<bool> CheckBookBorrewed(int bookId, int userId)
        {
            try
            {
                var borrowedBook = await _dbContext.BorrowedBook
                      .FirstOrDefaultAsync(a => a.UserId == userId || a.BookId == bookId);

                if (borrowedBook is not null)
                {
                    return true;
                }

                Log.Warning($"BorrwdBook with bookID:{bookId} and userId:{userId} not found.");
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while retrieving bookID:{bookId} and userId:{userId}");
                throw;
            }

        }
        private async Task<int> Create(CreateOrEditBorrowedBookDto input)
        {
            try
            {
                var data = await _dbContext.BorrowedBook.FirstOrDefaultAsync(a => a.UserId == input.UserId && a.BookId == input.BookId);

                if (data != null)
                {
                    var updatedData = await Update(input);
                    return updatedData;
                }

                var map = _mapper.Map<BorrowedBook>(input);
                _dbContext.BorrowedBook.Add(map);

                await _dbContext.SaveChangesAsync();

                Log.Information($"BorrowedBook created successfully. Book ID: {map.Id} -- UserID : {map.UserId}");
                return map.Id;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating/updating the borrowedbook.");
                throw;
            }
        }
        private async Task<int> Update(CreateOrEditBorrowedBookDto input)
        {
            try
            {
                var data = await _dbContext.BorrowedBook.FirstOrDefaultAsync(a => a.Id == input.Id);

                if (data != null)
                {
                    var mapping = _mapper.Map(input, data);

                    _dbContext.BorrowedBook.Update(mapping);
                    await _dbContext.SaveChangesAsync();

                    Log.Information($"BorrowedBook updated successfully. Book ID: {mapping.Id}");
                    return mapping.Id;
                }
                else
                {
                    Log.Error($"BorrowedBook with UserID: {input.UserId} -- BookID: {input.BookId} not found.");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred while updating the BorrowedBook.");
                throw ex;
            }
        }

    }
}
