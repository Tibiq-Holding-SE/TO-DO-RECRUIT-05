using System.Text.Json.Serialization;

namespace TaskManagerLite.Entities
{
    public class DeleteTaskRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
