using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using TaskManagerLite.Data;
using TaskManagerLite.Entities.Tasks;

namespace UnitTests
{
    public static class Helper
    {
        private static IServiceProvider _serviceProvider { get; set; }

        static Helper()
        {
            var startup = new Startup();
            _serviceProvider = startup.ServiceProvider;
        }

        public static async Task<GetShortTask?> GetTaskByNameAsync(string name, bool showOverdue=false)
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                var result = await taskRepo.GetTasksAsync(new TaskFilters
                {
                    Name = name,
                    ShowOverdue = showOverdue
                });

                if (result.Tasks == null)
                    return null;

                var task = result.Tasks![0];
                return task;
            }
        }

        public static async Task DeleteAllTasksAsync()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaskContext>();

                var tasksToDelete = await context.Tasks
                    .ToListAsync();

                context.RemoveRange(tasksToDelete);
                await context.SaveChangesAsync();
            }
        }

        public static async Task MakeTaskOldAsync(Guid id)
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaskContext>();

                var task = await context.Tasks.Where(t => t.Id == id)
                    .FirstOrDefaultAsync();

                task!.DeletedDate = DateTime.UtcNow.AddDays(-31);

                await context.SaveChangesAsync();
            }
        }
    }
}
