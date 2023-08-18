namespace TaskManagerLite.Interfaces
{
    public interface IBackgroundRepository
    {
        Task MarkOverdueTasksAsync();
        Task MarkCloseToOverdueTasksAsync();
        Task DeleteOldTasksAsync();
    }
}
