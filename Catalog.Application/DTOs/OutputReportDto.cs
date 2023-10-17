using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.Application.DTOs
{
    public class OutputReportDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Theme { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}