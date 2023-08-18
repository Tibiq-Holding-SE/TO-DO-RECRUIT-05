using TaskManagerLite.Entities;
using TaskManagerLite.Entities.Tasks;

namespace TaskManagerLite.Interfaces
{
    public interface ITaskRepository
    {
        Task<SearchResult> GetTasksAsync(TaskFilters? filters);
        Task<GetTask?> GetSingleTaskAsync(Guid taskId);
        Task<ManageTaskResult> CreateTaskAsync(CreateTaskRequest request);
        Task<ManageTaskResult> EditTaskAsync(EditTaskRequest request);
        Task ChangeNameAsync(ChangeNameRequest request);
        Task<ManageTaskResult> DeleteTaskAsync (DeleteTaskRequest request);
        Task<ManageTaskResult> FinishTaskAsync (FinishTaskRequest request);
        Task<ManageTaskResult> ReopenTaskAsync (ReopenTaskRequest request);
        Task<ManageTaskResult> RestoreTaskAsync (RestoreTaskRequest request);
    }
}
