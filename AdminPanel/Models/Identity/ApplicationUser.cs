using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public string JobDescription { get; set; }

        [PersonalData]
        public DateTime? BirthDate { get; set; }
    }

    public class Client
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class ClientBook
    {
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public Client Client { get; set; }
        public Book Book { get; set; }
        public DateTime CreationDate { get; set; }

    }

    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string ISBN { get; set; }
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        [ForeignKey(nameof(Publisher))]
        public int PublisherId { get; set; }
        public int PublishYear { get; set; }
        public int CopyNumber { get; set; }
        [ForeignKey(nameof(BookCategory))]
        public int BookCategoryId { get; set; }
        public double Price { get; set; }
        public BookCategory BookCategory { get; set; }
        public Author Author { get; set; }
        public Publisher Publisher { get; set; }
        public DateTime CreationDate { get; set; }

    }

    public class BookCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public double Price { get; set; }
        [ForeignKey(nameof(Cart))]
        public int CartId { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Cart Cart { get; set; }
        public Order Order { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public DateTime CreationDate { get; set; }
    }
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
