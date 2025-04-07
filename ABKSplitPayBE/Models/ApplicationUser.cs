using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(255)]
        public string ProfilePictureUrl { get; set; } = "https://example.com/images/default-profile.jpg"; // Default value

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Relationships
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Cart Cart { get; set; }
        public ICollection<WishList> WishListItems { get; set; }
    }
}