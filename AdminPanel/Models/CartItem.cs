using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public double Price { get; set; }
        [ForeignKey(nameof(Cart))]
        public int? CartId { get; set; }
        [ForeignKey(nameof(Order))]
        public int? OrderId { get; set; } 
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public Cart Cart { get; set; }
        public Order Order { get; set; }
        public Book Book { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
