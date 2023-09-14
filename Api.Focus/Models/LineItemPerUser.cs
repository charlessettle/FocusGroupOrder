using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Focus.Models
{
	public class LineItemPerUser : BaseModel
    {
        public LineItemPerUser() { }

        [Key]
        public int LineItemPerUserId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public int UserOrderId { get; set; }

        public UserOrder UserOrder { get; set; }
    }
}