using System.Text.Json.Serialization;

namespace TaskManagerLite.Entities
{
    public class RestoreTaskRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
