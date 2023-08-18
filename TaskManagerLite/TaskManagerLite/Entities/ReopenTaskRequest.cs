using System.Text.Json.Serialization;

namespace TaskManagerLite.Entities
{
    public class ReopenTaskRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
