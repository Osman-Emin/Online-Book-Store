using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdminPanel.Models.Identity;

namespace AdminPanel.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
