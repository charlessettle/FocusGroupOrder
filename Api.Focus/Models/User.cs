using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Focus.Models
{
    public class User : BaseModel
    {
        public User() { }

        [Key]
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }

        public List<UserOrder> UserOrders { get; set; }
    }
}

