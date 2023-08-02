using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.DTOs
{
    public class UpdateProductDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}