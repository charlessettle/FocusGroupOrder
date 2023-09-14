using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Api.Focus.Models
{
    public class Order : BaseModel
    {
        public Order() { }

        [Key]
        public int OrderId { get; set; }

        public int CreatorId { get; set; }
    }
}