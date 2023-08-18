using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json.Serialization;
using TaskManagerLite.Entities.Tasks;

namespace TaskManagerLite.Entities
{
    public class CompositeViewModel
    {
        [JsonPropertyName("filters")]
        public TaskFilters? Filters { get; set; }
        [JsonPropertyName("result")]
        public SearchResult? Result { get; set; }

        public CompositeViewModel(TaskFilters filters)
        {
            Filters = filters;
        }

        public CompositeViewModel(SearchResult result)
        {
            Result = result;
        }

        public CompositeViewModel(TaskFilters filters, SearchResult result)
        {
            Filters = filters;
            Result = result;
        }
    }
}
