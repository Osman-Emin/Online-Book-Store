using System;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Models;
using AdminPanel.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AdminPanel.Data.Seeders
{
    public static class SiteDataSeeder
    {
        public static async Task SeedDataAsync(IServiceProvider services)
        {
            _context = services.GetRequiredService<ApplicationDbContext>();

            if (!_context.Authors.Any())
            {
                SeedAuthors();
                _context.SaveChanges();
            }

            if (!_context.BookCategories.Any())
            {
                SeedBookCategories();
                _context.SaveChanges();
            }

            if (!_context.Publishers.Any())
            {
                SeedPublishers();
                _context.SaveChanges();
            }

            if (!_context.Books.Any())
            {
                SeedBooks();
                _context.SaveChanges();
            }

            if (!_context.Clients.Any())
            {
                SeedClients();
                _context.SaveChanges();
            }

            if (!_context.Carts.Any())
            {
                SeedCarts();
                _context.SaveChanges();
            }
            if (!_context.CartItems.Any())
            {
                SeedCartItems();
                _context.SaveChanges();
            }
            if (!_context.Orders.Any())
            {
                SeedOrders();
                _context.SaveChanges();
            }

        }

        private static void SeedClients()
        {
            foreach (var user in _context.Users)
            {
                user.FullName = Faker.Name.FullName();
                user.JobDescription = Faker.Lorem.Sentence(1);
                user.BirthDate = Faker.Identification.DateOfBirth();
                user.Address = Faker.Address.StreetAddress(true);
                _context.Clients.Add(
                    new Client() { ApplicationUserId = user.Id ,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))}
                );
            }
        }
        private static ApplicationDbContext _context;

        private static void SeedAuthors()
        {
            for (int i = 0; i < 20; i++)
            {
                 _context.Authors.Add(
                 
                     new Author(){Name = Faker.Name.FullName(),CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))}
                 );
            }
        }

        private static void SeedBookCategories()
        {
            _context.BookCategories.AddRange(new[]
            {
                new BookCategory() { Name = "Fantasy" ,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))}, new BookCategory() { Name = "Sci-Fi" ,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))},
                new BookCategory() { Name = "Mystery" ,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))}, new BookCategory() { Name = "Thriller" ,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))},
                new BookCategory() { Name = "Romance",CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90)) }, new BookCategory() { Name = "Westerns" ,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))},
                new BookCategory() { Name = "Dystopian" ,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))}, new BookCategory() { Name = "Contemporary" ,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))},
            });

        }

        private static void SeedPublishers()
        {
            for (int i = 0; i < 20; i++)
            {
                _context.Publishers.Add(
                
                    new Publisher(){Name = Faker.Company.Name(),CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))}
                );
            }
        }
        private static void SeedBooks()
        {
            for (int i = 0; i < 200; i++)
            {
                _context.Books.Add(
                
                    new Book()
                    {
                        Title = Faker.Lorem.Sentence(2),ISBN = Faker.Identification.SocialSecurityNumber(),BookCategoryId = Faker.RandomNumber.Next(1,_context.BookCategories.Count())
                        ,AuthorId = Faker.RandomNumber.Next(1,_context.Authors.Count()),PublisherId = Faker.RandomNumber.Next(1,_context.Publishers.Count()),CopyNumber = Faker.RandomNumber.Next(1,4),
                        CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90)),Price = Faker.RandomNumber.Next(30,200),PublishYear = Faker.RandomNumber.Next(1900,2021)
                    }
                );
            }
        }
        
        private static void SeedCarts()
        {
            foreach (var client in _context.Clients.Take(50))
            {
                _context.Carts.Add(new Cart()
                {
                    ClientId = client.Id,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))
                });
            }
        }
        private static void SeedCartItems()
        {
            foreach (var cart in _context.Carts.Take(20))
            {
                for(int i=0;i<Faker.RandomNumber.Next(1,10);i++)
                {

                    var book = _context.Books.FirstOrDefault(b =>
                        b.Id == Faker.RandomNumber.Next(1, _context.Books.Count()));
                    if(book != null)
                        _context.CartItems.Add(new CartItem()
                        {
                            CartId = cart.Id,
                            BookId = book.Id,
                            Price = book.Price
                            ,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))
                        });
                }
            }
        }
        private static void SeedOrders()
        {
            foreach (var cart in _context.Carts.Where(o=>o.CartItems.Count>0).Take(5))
            {
                var order = _context.Orders.Add(new Order()
                {
                    ClientId = cart.ClientId,PaymentReferenceNo = Faker.Identification.SocialSecurityNumber(),CartItems = cart.CartItems
                    ,CreationDate = DateTime.Now.AddDays(Faker.RandomNumber.Next(0,90))
                });
            }
        }
        
        
    }
}
