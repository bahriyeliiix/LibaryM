using AutoMapper;
using DAL.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Model.DTOs.Book;
using Model.DTOs.Core;
using Model.Entities;
using Model.Enums;
using Serilog;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BookService : IBookService
    {
        private readonly LibaryMDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _hostEnvironment;


        public BookService(LibaryMDbContext dbContext, IMapper mapper, IHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<int> CreateOrEdit(CreateOrEditBookDto input)
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
        public async Task<BookForViewWithTotalDto> GetAll(GetAllBookInput input)
        {
            var query = await BuildBookQuery(input);
            var filteredData = await ApplyFilterAndCreateResultAsync(query, input);

            return filteredData;
        }
        public async Task<BookDto> GetById(int id)
        {
            try
            {
                var book = await _dbContext.Books
                      .FirstOrDefaultAsync(a => a.Id == id);

                if (book is not null)
                {
                    var mapping = _mapper.Map<BookDto>(book);
                    return mapping;
                }

                Log.Warning($"Book with ID {id} not found.");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while retrieving book with ID {id}.");
                throw;
            }
        }
        public async Task<bool> SoftDelete(int Id)
        {
            try
            {
                var book = await _dbContext.Books.FirstOrDefaultAsync(a => a.Id == Id);

                if (book is not null)
                {
                    book.IsSoftDel = true;
                    await _dbContext.SaveChangesAsync();

                    Log.Information($"Soft delete successful for Book ID: {Id}");
                    return true;
                }

                Log.Error($"Book with ID {Id} not found for soft delete.");
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while performing soft delete.");
                throw;
            }
        }
        public async Task<bool> CheckBook(int Id)
        {
            try
            {
                var book = await _dbContext.Books
                      .FirstOrDefaultAsync(a => a.Id == Id);

                if (book is not null)
                {
                    return true;
                }

                Log.Warning($"Book with ID {Id} not found.");
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while retrieving book with ID {Id}.");
                throw;
            }
        }
        public async Task<bool> CheckBookInLibrary(int Id)
        {
            try
            {
                var book = await _dbContext.Books
                    .FirstOrDefaultAsync(a => a.Id == Id);

                if (book is not null || book.Status == Status.InLibrary)
                {
                    return true;
                }
                Log.Warning($"Book with ID {Id} not found.");
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while retrieving book with ID {Id}.");
                throw;
            }
        }
        public async Task<bool> UpdateStatus(int bookId, Status newStatus)
        {
            try
            {
                var book = await this.GetById(bookId);

                if (book is not null)
                {
                    book.Status = newStatus;

                    var bookDto = _mapper.Map<CreateOrEditBookDto>(book);
                    await this.CreateOrEdit(bookDto);

                    Log.Information($"Book status updated successfully. Book ID: {bookId}, New Status: {newStatus}");
                    return true;
                }
                Log.Warning($"Book with ID {bookId} not found.");
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while retrieving book with ID {bookId}.");
                throw;
            }
        }


        private async Task<string> UploadImage(IFormFile formFile)
        {
            string uniqeName = string.Empty;

            string uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot/images");
            uniqeName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqeName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return uniqeName;
        }
        private async Task<IQueryable<Book>> BuildBookQuery(GetAllBookInput input)
        {
            return _dbContext.Books.AsQueryable()
                .WhereIf(!string.IsNullOrEmpty(input.FilterText), c => c.Name.Contains(input.FilterText))
                .Where(a => a.IsSoftDel == false)
                .OrderBy(a => a.Name).AsNoTracking();
        }
        private async Task<BookForViewWithTotalDto> ApplyFilterAndCreateResultAsync(IQueryable<Book> query, GetAllBookInput input)
        {
            var filter = from o in query

                         join o1 in _dbContext.BorrowedBook.AsQueryable() on o.Id equals o1.BookId into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _dbContext.Users.AsQueryable() on s1.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetBookForViewDto()
                         {
                             Book = _mapper.Map<BookDto>(o),
                             StatusName = o.Status.ToString(),
                             BorrowedDate = o.Status == Model.Enums.Status.CheckedOut ? s1.BorrowedDate : null,
                             ReturnDate = o.Status == Model.Enums.Status.CheckedOut ? s1.ReturnDate : null,
                             Borrower = o.Status == Model.Enums.Status.CheckedOut ? s2.Name : string.Empty,
                             IsAvailable = o.Status == Model.Enums.Status.CheckedOut ? false : true
                         };

            var filteredData = await PagedList<GetBookForViewDto>.CreateAsync(filter, input.PageNumber, input.PageSize);

            var result = new BookForViewWithTotalDto()
            {
                Book = filteredData,
                Total = query.Count()
            };

            return result;
        }
        private async Task<int> Update(CreateOrEditBookDto input)
        {
            try
            {
                var data = await _dbContext.Books.FirstOrDefaultAsync(a => a.Id == input.Id);

                if (data is not null)
                {
                    var mapping = _mapper.Map(input, data);

                    _dbContext.Books.Update(mapping);
                    await _dbContext.SaveChangesAsync();

                    Log.Information($"Book updated successfully. Book ID: {mapping.Id}");
                    return mapping.Id;
                }
                else
                {
                    Log.Error($"Book with ID {input.Id} not found.");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred while updating the book.");
                throw ex;
            }
        }
        private async Task<int> Create(CreateOrEditBookDto input)
        {
            try
            {

                var data = await _dbContext.Books.FirstOrDefaultAsync(a => a.Id == input.Id);

                if (data is not null)
                {
                    var updatedData = await Update(input);
                    return updatedData;
                }

                if (input.Image is not null)
                {
                    var path = await UploadImage(input.Image);
                    input.ImageURL = path;
                }
                var map = _mapper.Map<Book>(input);
                _dbContext.Books.Add(map);

                await _dbContext.SaveChangesAsync();

                Log.Information($"Book created successfully. Book ID: {map.Id}");
                return map.Id;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating/updating the book.");
                throw;
            }
        }
    }
}
