using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Focus.Models
{
    public class Product : BaseModel
    {
        public Product() { }

        [Key]
        public int ProductId { get; set; }

        [Required, Column(TypeName = "money")] //specifies the money data type in SQL
        public decimal Price { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}