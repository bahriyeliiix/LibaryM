
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace DAL.Context;

public class LibaryMDbContext : DbContext
{
    public LibaryMDbContext(DbContextOptions<LibaryMDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.EnableSensitiveDataLogging();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<BorrowedBook> BorrowedBook { get; set; }
    public DbSet<Book> Books { get; set; }

}
