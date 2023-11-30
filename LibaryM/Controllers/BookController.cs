using LibaryM.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Core;
using Model.Entities;
using System.Diagnostics;
using Model.DTOs.Book;
using Model.DTOs.BorrowedBook;

namespace LibaryM.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBusinessUnitOfWork _uOW;

        public BookController(ILogger<BookController> logger, IBusinessUnitOfWork uOW)
        {
            _logger = logger;
            _uOW = uOW;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await _uOW.BookService.GetAll(new Model.DTOs.Book.GetAllBookInput { FilterText = "", PageSize = 10, PageNumber = 1 });
            return View(books);
        }
        [HttpGet]
        public async Task<IActionResult> New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrEditBookDto book)
        {
            if (ModelState.IsValid)
            {
                var result = await _uOW.BookService.CreateOrEdit(book);
                return RedirectToAction("Index");
            }

            return View("New", book);
        }

        [HttpGet]
        public IActionResult Borrow(int bookId, string bookName, string bookAuthor)
        {
            var borrowViewModel = new BorrowViewModel
            {
                BookId = bookId,
                BookName = bookName,
                BookAuthor = bookAuthor,
                BorrowDate = DateTime.Now
            };

            return View(borrowViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Borrow(CreateOrEditBorrowedBookDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _uOW.ToLend(model);
                return RedirectToAction("Index");
            }

            return View("Borrow", model);
        }
    }
}

