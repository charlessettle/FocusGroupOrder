using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Focus.Models
{
    public class UserOrder : BaseModel
    {
        public UserOrder() { }

        [Key]
        public int UserOrderId { get; set; }

        [Required]
        public int UserID { get; set; }

        public User User { get; set; }

        [Required]
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public bool? IsComplete { get; set; }

        public List<LineItemPerUser> LineItemsPerUser { get; set; }
    }
}

