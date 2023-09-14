using System.ComponentModel.DataAnnotations;

namespace Api.Focus.Models
{
    public class BaseModel
    {
        public BaseModel() { }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
    }
}