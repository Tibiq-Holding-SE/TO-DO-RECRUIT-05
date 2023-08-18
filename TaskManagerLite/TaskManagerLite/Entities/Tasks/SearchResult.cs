using System.Text.Json.Serialization;

namespace TaskManagerLite.Entities.Tasks
{
    public class SearchResult
    {
        [JsonPropertyName("tasks")]
        public List<GetShortTask>? Tasks { get; set; }

        public SearchResult(List<GetShortTask>? tasks)
        {
            Tasks = tasks;
        }
    }
}
