using System.Text.Json.Serialization;

namespace TaskManagerLite.Entities
{
    // Class, used for editing tasks

    #nullable enable
    public class EditTaskRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("dueDate")]
        public DateTime? DueDate { get; set; }
    }
}
