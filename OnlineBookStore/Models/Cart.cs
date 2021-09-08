using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public Client Client { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
