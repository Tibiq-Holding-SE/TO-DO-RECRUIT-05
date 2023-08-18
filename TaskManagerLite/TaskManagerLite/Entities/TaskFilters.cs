using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagerLite.Entities
{
    public class TaskFilters
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("showDeleted")]
        public bool ShowDeleted { get; set; }
        [JsonPropertyName("showExpired")]
        public bool ShowOverdue { get; set; }
        [JsonPropertyName("showFinished")]
        public bool ShowFinished { get; set; }

        public TaskFilters()
        {
            ShowDeleted = false;
            ShowOverdue = false;
            ShowFinished = false;
        }
    }
}
