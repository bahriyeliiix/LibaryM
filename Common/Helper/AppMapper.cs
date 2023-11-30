using AutoMapper;
using Model.DTOs.Book;
using Model.DTOs.BorrowedBook;
using Model.Entities;

public class AppMapper : Profile
{
    public AppMapper()
    {
        CreateMap<Book, BookDto>();
        CreateMap<BookDto, Book>();

        CreateMap<CreateOrEditBookDto, BookDto>();
        CreateMap<BookDto, CreateOrEditBookDto>();

        CreateMap<CreateOrEditBookDto, Book>();
        CreateMap<Book, CreateOrEditBookDto>();

        CreateMap<CreateOrEditBorrowedBookDto, BorrowedBook>();
        CreateMap<BorrowedBook, CreateOrEditBorrowedBookDto>();




    }
}
