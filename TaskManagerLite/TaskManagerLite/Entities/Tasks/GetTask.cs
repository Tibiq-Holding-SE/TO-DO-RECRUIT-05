using System.Text.Json.Serialization;

namespace TaskManagerLite.Entities.Tasks
{
#nullable disable
    public class GetTask
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("dueDate")]
        public DateTime? DueDate { get; set; }
        [JsonPropertyName("status")]
        public TaskStatus Status { get; set; }
    }
}
