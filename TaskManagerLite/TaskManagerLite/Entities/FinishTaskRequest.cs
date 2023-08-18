using System.Text.Json.Serialization;

namespace TaskManagerLite.Entities
{
    public class FinishTaskRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
