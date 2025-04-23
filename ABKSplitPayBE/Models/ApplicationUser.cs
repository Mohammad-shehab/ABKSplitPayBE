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
        public string ProfilePictureUrl { get; set; } = "https://rslqld.org/-/media/rslqld/stock-images/find-help/advocacy/dva-claims-icons/rsl-contact-methods_in-person-01.png?modified=20201013230428";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Cart Cart { get; set; }
        public ICollection<WishList> WishListItems { get; set; }
    }
}