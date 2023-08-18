using Microsoft.Extensions.DependencyInjection;
using TaskStatus = TaskManagerLite.Entities.TaskStatus;

namespace UnitTests
{
    public class TaskTests
    {
        private IServiceProvider _serviceProvider { get; set; }

        public TaskTests()
        {
            var startup = new Startup();
            _serviceProvider = startup.ServiceProvider;
        }

        [Fact]
        public async Task CreateTask_OnlyNameIsProvided_TaskIsCreated()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                var result = await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name",
                    Description = "",
                    DueDate = null
                });

                Assert.Equal(ManageTaskResult.Success, result);
            }
        }

        [Fact]
        public async Task CreateTask_EachParameterIsProvided_TaskIsCreated()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                var now = DateTime.UtcNow;

                var result = await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name",
                    Description = "Description",
                    DueDate = now
                });

                Assert.Equal(ManageTaskResult.Success, result);
            }
        }

        [Fact]
        public async Task CreateTask_NameIsNotProvided_TaskIsNotCreated()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                var result = await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "",
                    Description = "",
                    DueDate = null
                });

                Assert.Equal(ManageTaskResult.NameIsNull, result);
            }
        }

        [Fact]
        public async Task CreateTask_SameDueDates_TaskIsNotCreated()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                var now = DateTime.UtcNow;
                var model = new CreateTaskRequest
                {
                    Name = "Name",
                    Description = "",
                    DueDate = now
                };

                await taskRepo.CreateTaskAsync(model);
                await taskRepo.CreateTaskAsync(model);
                var result = await taskRepo.CreateTaskAsync(model);

                Assert.Equal(ManageTaskResult.SameDueDate, result);
            }
        }

        [Fact]
        public async Task GetTasks_ByName_TaskIsFound()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var result = await taskRepo.GetTasksAsync(new TaskFilters
                {
                    Name = taskName
                });

                Assert.NotNull(result.Tasks);
                Assert.True(result.Tasks!.Count >= 1);
            }
        }

        [Fact]
        public async Task GetTasks_ByIsDeletedFlag_TasksAreFound()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                await Helper.DeleteAllTasksAsync();

                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name1",
                    Description = "",
                    DueDate = null
                });

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name2",
                    Description = "",
                    DueDate = null
                });

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name3",
                    Description = "",
                    DueDate = null
                });

                var taskToBeDeleted1 = await Helper.GetTaskByNameAsync("Name1");
                var taskToBeDeleted2 = await Helper.GetTaskByNameAsync("Name2");
                var taskToBeFinished = await Helper.GetTaskByNameAsync("Name3");

                await taskRepo.DeleteTaskAsync(new DeleteTaskRequest { Id = taskToBeDeleted1!.Id });
                await taskRepo.DeleteTaskAsync(new DeleteTaskRequest { Id = taskToBeDeleted2!.Id });
                await taskRepo.FinishTaskAsync(new FinishTaskRequest { Id = taskToBeFinished!.Id });

                var result = await taskRepo.GetTasksAsync(new TaskFilters
                {
                    ShowDeleted = true
                });

                Assert.NotNull(result.Tasks);
                Assert.True(result.Tasks!.Count == 2);
            }
        }

        [Fact]
        public async Task GetTasks_ByIsFinishedFlag_TasksAreFound()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                await Helper.DeleteAllTasksAsync();

                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name1",
                    Description = "",
                    DueDate = null
                });

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name2",
                    Description = "",
                    DueDate = null
                });

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name3",
                    Description = "",
                    DueDate = null
                });

                var taskToBeDeleted1 = await Helper.GetTaskByNameAsync("Name1");
                var taskToBeDeleted2 = await Helper.GetTaskByNameAsync("Name2");
                var taskToBeFinished = await Helper.GetTaskByNameAsync("Name3");

                await taskRepo.DeleteTaskAsync(new DeleteTaskRequest { Id = taskToBeDeleted1!.Id });
                await taskRepo.DeleteTaskAsync(new DeleteTaskRequest { Id = taskToBeDeleted2!.Id });
                await taskRepo.FinishTaskAsync(new FinishTaskRequest { Id = taskToBeFinished!.Id });

                var result = await taskRepo.GetTasksAsync(new TaskFilters
                {
                    ShowFinished = true
                });

                Assert.NotNull(result.Tasks);
                Assert.True(result.Tasks!.Count == 1);
            }
        }

        [Fact]
        public async Task GetTasks_ByIsOverdueFlag_TasksAreFound()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                await Helper.DeleteAllTasksAsync();

                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name1",
                    Description = "",
                    DueDate = null
                });

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name2",
                    Description = "",
                    DueDate = null
                });

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name3",
                    Description = "",
                    DueDate = null
                });

                var taskToBeDeleted1 = await Helper.GetTaskByNameAsync("Name1");
                var taskToBeDeleted2 = await Helper.GetTaskByNameAsync("Name2");
                var taskToBeFinished = await Helper.GetTaskByNameAsync("Name3");

                await taskRepo.DeleteTaskAsync(new DeleteTaskRequest { Id = taskToBeDeleted1!.Id });
                await taskRepo.DeleteTaskAsync(new DeleteTaskRequest { Id = taskToBeDeleted2!.Id });
                await taskRepo.FinishTaskAsync(new FinishTaskRequest { Id = taskToBeFinished!.Id });

                var result = await taskRepo.GetTasksAsync(new TaskFilters
                {
                    ShowOverdue = true
                });

                Assert.NotNull(result.Tasks);
                Assert.True(result.Tasks!.Count == 0);
            }
        }

        [Fact]
        public async Task GetTasks_ByAllFilters_TasksAreFound()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                await Helper.DeleteAllTasksAsync();

                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name1",
                    Description = "",
                    DueDate = null
                });

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name2",
                    Description = "",
                    DueDate = null
                });

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name3",
                    Description = "",
                    DueDate = null
                });

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name4",
                    Description = "",
                    DueDate = null
                });

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = "Name5",
                    Description = "",
                    DueDate = null
                });

                var taskToBeDeleted1 = await Helper.GetTaskByNameAsync("Name1");
                var taskToBeDeleted2 = await Helper.GetTaskByNameAsync("Name2");
                var taskToBeFinished = await Helper.GetTaskByNameAsync("Name3");

                await taskRepo.DeleteTaskAsync(new DeleteTaskRequest { Id = taskToBeDeleted1!.Id });
                await taskRepo.DeleteTaskAsync(new DeleteTaskRequest { Id = taskToBeDeleted2!.Id });
                await taskRepo.FinishTaskAsync(new FinishTaskRequest { Id = taskToBeFinished!.Id });

                var result = await taskRepo.GetTasksAsync(new TaskFilters
                {
                    ShowOverdue = true,
                    ShowFinished = true,
                    ShowDeleted = true
                });

                Assert.NotNull(result.Tasks);
                Assert.True(result.Tasks!.Count == 5);
            }
        }

        [Fact]
        public async Task EditTask_NameIsProvided_TaskIsEditted()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var task = await Helper.GetTaskByNameAsync(taskName);

                var result = await taskRepo.EditTaskAsync(new EditTaskRequest
                {
                    Id = task!.Id,
                    Name = "ChangedName",
                    Description = "",
                    DueDate = null
                });

                Assert.Equal(ManageTaskResult.Success, result);
            }
        }

        [Fact]
        public async Task EditTask_NameIsNotProvided_TaskIsNotEditted()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var task = await Helper.GetTaskByNameAsync(taskName);

                var result = await taskRepo.EditTaskAsync(new EditTaskRequest
                {
                    Id = task!.Id,
                    Name = "",
                    Description = "",
                    DueDate = null
                });

                Assert.Equal(ManageTaskResult.NameIsNull, result);
            }
        }

        [Fact]
        public async Task EditTask_IdIsNotProvided_TaskIsNotEditted()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                var result = await taskRepo.EditTaskAsync(new EditTaskRequest
                {
                    Id = new Guid(),
                    Name = "",
                    Description = "",
                    DueDate = null
                });

                Assert.Equal(ManageTaskResult.DoesNotExist, result);
            }
        }

        [Fact]
        public async Task ChangeName_IdIsProvided_NameIsChanged()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";
                var taskNewName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var task = await Helper.GetTaskByNameAsync(taskName);

                await taskRepo.ChangeNameAsync(new ChangeNameRequest
                {
                    Id = task!.Id,
                    Name = taskNewName
                });

                task = await Helper.GetTaskByNameAsync(taskNewName);

                Assert.NotNull(task);
                Assert.Equal(taskNewName, task!.Name);
            }
        }

        [Fact]
        public async Task DeleteTask_TaskExists_TaskIsDeleted()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var task = await Helper.GetTaskByNameAsync(taskName);

                var result = await taskRepo.DeleteTaskAsync(new DeleteTaskRequest
                {
                    Id = task!.Id
                });

                var taskInfo = await taskRepo.GetSingleTaskAsync(task!.Id);

                Assert.Equal(ManageTaskResult.Success, result);
                Assert.Equal(TaskStatus.Deleted, taskInfo!.Status);
            }
        }

        [Fact]
        public async Task DeleteTask_TaskDoesNotExist_TaskIsNotDeletedDeleted()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var result = await taskRepo.DeleteTaskAsync(new DeleteTaskRequest
                {
                    Id = new Guid()
                });

                Assert.Equal(ManageTaskResult.DoesNotExist, result);
            }
        }

        [Fact]
        public async Task FinishTask_TaskExists_TaskIsFinished()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var task = await Helper.GetTaskByNameAsync(taskName);

                var result = await taskRepo.FinishTaskAsync(new FinishTaskRequest
                {
                    Id = task!.Id
                });

                var taskInfo = await taskRepo.GetSingleTaskAsync(task!.Id);

                Assert.Equal(ManageTaskResult.Success, result);
                Assert.Equal(TaskStatus.Finished, taskInfo!.Status);
            }
        }

        [Fact]
        public async Task FinishTask_TaskDoesNotExist_TaskIsNotFinished()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var result = await taskRepo.FinishTaskAsync(new FinishTaskRequest
                {
                    Id = new Guid()
                });

                Assert.Equal(ManageTaskResult.DoesNotExist, result);
            }
        }

        [Fact]
        public async Task ReopenTask_TaskExists_TaskIsReopened()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var task = await Helper.GetTaskByNameAsync(taskName);

                await taskRepo.FinishTaskAsync(new FinishTaskRequest { Id = task!.Id });

                var result = await taskRepo.ReopenTaskAsync(new ReopenTaskRequest
                {
                    Id = task!.Id
                });

                var taskInfo = await taskRepo.GetSingleTaskAsync(task!.Id);

                Assert.Equal(ManageTaskResult.Success, result);
                Assert.Equal(TaskStatus.New, taskInfo!.Status);
            }
        }

        [Fact]
        public async Task ReopenTask_TaskDoesNotExist_TaskIsNotReopened()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var result = await taskRepo.ReopenTaskAsync(new ReopenTaskRequest
                {
                    Id = new Guid()
                });

                Assert.Equal(ManageTaskResult.DoesNotExist, result);
            }
        }

        [Fact]
        public async Task RestoreTask_TaskExists_TaskIsRestored()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });


                var task = await Helper.GetTaskByNameAsync(taskName);

                await taskRepo.DeleteTaskAsync(new DeleteTaskRequest { Id = task!.Id });

                await taskRepo.RestoreTaskAsync(new RestoreTaskRequest
                {
                    Id = task!.Id
                });

                var taskInfo = await taskRepo.GetSingleTaskAsync(task!.Id);

                Assert.Equal(TaskStatus.New, taskInfo!.Status);
            }
        }

        [Fact]
        public async Task RestoreTask_TaskDoesNotExists_TaskIsNotRestored()
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskName = "Name";

                await taskRepo.CreateTaskAsync(new CreateTaskRequest
                {
                    Name = taskName,
                    Description = "",
                    DueDate = null
                });

                var result = await taskRepo.RestoreTaskAsync(new RestoreTaskRequest
                {
                    Id = new Guid()
                });

                Assert.Equal(ManageTaskResult.DoesNotExist, result);
            }
        }
    }
}