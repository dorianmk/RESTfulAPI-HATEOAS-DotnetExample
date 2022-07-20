using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data.Seed
{
    public class BookSeeder
    {
        internal void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasData(new Book
                {
                    Id = 1,
                    Title = "Mały Książe",
                    ReleaseDate = new DateTime(1943, 4, 6),
                    Price = 10.5m
                });

            modelBuilder.Entity<Book>()
               .HasData(new Book
               {
                   Id = 2,
                   Title = "Balladyna",
                   ReleaseDate = new DateTime(1839, 2, 2),
                   Price = 20.5m
               });

            modelBuilder.Entity<Author>()
                .HasData(new Author
                {
                    Id = 1,
                    FirstName = "Antoine",
                    LastName = "de Saint-Exupéry"
                });

            modelBuilder.Entity<Author>()
                .HasData(new Author
                {
                    Id = 2,
                    FirstName = "Juliusz",
                    LastName = "Słowacki"
                });

            modelBuilder.Entity<BookAuthor>()
                .HasData(new BookAuthor
                {
                    AuthorId = 1,
                    BookId = 1
                });

            modelBuilder.Entity<BookAuthor>()
                .HasData(new BookAuthor
                {
                    AuthorId = 2,
                    BookId = 2
                });
        }
    }
}
