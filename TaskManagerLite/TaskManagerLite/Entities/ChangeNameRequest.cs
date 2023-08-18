using System.Text.Json.Serialization;

namespace TaskManagerLite.Entities
{
    #nullable disable
    public class ChangeNameRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
