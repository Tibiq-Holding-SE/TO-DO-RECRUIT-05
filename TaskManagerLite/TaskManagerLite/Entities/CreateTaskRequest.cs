using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace TaskManagerLite.Entities
{
    // Class, used for creating tasks

    #nullable enable
    public class CreateTaskRequest
    {
        [NotNull]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [MaybeNull]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [MaybeNull]
        [JsonPropertyName("dueDate")]
        public DateTime? DueDate { get; set; }
    }
}
