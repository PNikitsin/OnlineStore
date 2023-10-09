namespace Catalog.Domain.Entities.Mongo
{
    public class Report : Document
    {
        public string Username { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}