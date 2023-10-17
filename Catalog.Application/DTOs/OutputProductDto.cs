using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.DTOs
{
    public class OutputProductDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}