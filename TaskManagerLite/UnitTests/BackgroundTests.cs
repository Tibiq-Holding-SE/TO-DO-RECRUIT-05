using Microsoft.Extensions.DependencyInjection;
using TaskStatus = TaskManagerLite.Entities.TaskStatus;

namespace UnitTests
{
    public class BackgroundTests
    {
        private IServiceProvider _serviceProvider { get; set; }

        public BackgroundTests()
        {
            var startup = new Startup();
            _serviceProvider = startup.ServiceProvider;
        }

        [Fact]
        public async Task MarkCloseToOverdue_Marked()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var backgroundRepo = scope.ServiceProvider.GetRequiredService<IBackgroundRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = (DateTime.UtcNow).AddHours(1)
                });

                await backgroundRepo.MarkCloseToOverdueTasksAsync();
                var task = await Helper.GetTaskByNameAsync(taskName);
                var taskInfo = await taskRepo.GetSingleTaskAsync(task!.Id);

                Assert.Equal(TaskStatus.CloseToOverdue, taskInfo!.Status);
            }
        }

        [Fact]
        public async Task MarkOverdue_Marked()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var backgroundRepo = scope.ServiceProvider.GetRequiredService<IBackgroundRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = (DateTime.UtcNow).AddHours(-1)
                });

                await backgroundRepo.MarkOverdueTasksAsync();
                var task = await Helper.GetTaskByNameAsync(taskName, true);
                var taskInfo = await taskRepo.GetSingleTaskAsync(task!.Id);

                Assert.Equal(TaskStatus.Overdue, taskInfo!.Status);
            }
        }

        [Fact]
        public async Task DeleteOldTasks_Deleted()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var backgroundRepo = scope.ServiceProvider.GetRequiredService<IBackgroundRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = (DateTime.UtcNow).AddHours(1)
                });

                var task = await Helper.GetTaskByNameAsync(taskName);
                var taskInfo = await taskRepo.GetSingleTaskAsync(task!.Id);

                await Helper.MakeTaskOldAsync(taskInfo!.Id);
                await backgroundRepo.DeleteOldTasksAsync();

                taskInfo = await taskRepo.GetSingleTaskAsync(task!.Id);

                Assert.Null(taskInfo);
            }
        }

    }
}
