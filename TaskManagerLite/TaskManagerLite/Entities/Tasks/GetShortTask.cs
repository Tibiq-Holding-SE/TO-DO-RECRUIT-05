using System.Text.Json.Serialization;

//Used in list menu
#nullable disable
namespace TaskManagerLite.Entities.Tasks
{
    public class GetShortTask
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("status")]
        public TaskStatus Status { get; set; }
    }
}
