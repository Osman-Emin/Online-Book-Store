using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AdminPanel.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public string PaymentReferenceNo { get; set; }
        public bool IsProcessed { get; set; } = false;
        public virtual ICollection<CartItem> CartItems { get; set; }
        public Client Client { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [NotMapped]
        public double Total
        {
            get
            {
                return CartItems.Any() ? CartItems.Sum(c => c.Price):0;
            }
        }
    }
}
