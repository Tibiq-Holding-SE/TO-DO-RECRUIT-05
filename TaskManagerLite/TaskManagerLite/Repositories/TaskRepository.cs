using Microsoft.EntityFrameworkCore;
using TaskManagerLite.Data;
using TaskManagerLite.Entities;
using TaskManagerLite.Entities.Tasks;
using TaskManagerLite.Interfaces;

namespace TaskManagerLite.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private TaskContext _context;

        public TaskRepository(TaskContext context)
        {
            _context = context;
        }

        public async Task<ManageTaskResult> CreateTaskAsync(CreateTaskRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return ManageTaskResult.NameIsNull;

            var sameDueDateCount = await CheckDueDateCountAsync(request.DueDate);

            if (sameDueDateCount >= 2)
                return ManageTaskResult.SameDueDate;

            await _context.Tasks.AddAsync(new SimpleTask(
                request.Name, 
                request.Description, 
                request.DueDate
                ));

            await _context.SaveChangesAsync();
            return ManageTaskResult.Success;
        }

        public async Task<ManageTaskResult> EditTaskAsync(EditTaskRequest request)
        {
            var sameDueDateCount = await CheckDueDateCountAsync(request.DueDate);

            if (sameDueDateCount >= 2)
                return ManageTaskResult.SameDueDate;

            var existingTask = await _context.Tasks.Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync();

            if (existingTask == null)
                return ManageTaskResult.DoesNotExist;

            if (string.IsNullOrEmpty(request.Name))
                return ManageTaskResult.NameIsNull;

            existingTask.Description = request.Description;

            existingTask.DueDate = request.DueDate;

            await _context.SaveChangesAsync();
            
            return ManageTaskResult.Success;
        }

        public async Task ChangeNameAsync(ChangeNameRequest request)
        {
            var existingTask = await _context.Tasks.Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync();

            if (existingTask == null)
                return;

            existingTask.Name = request.Name;
            await _context.SaveChangesAsync();
        }

        public async Task<ManageTaskResult> DeleteTaskAsync(DeleteTaskRequest request)
        {
            var existingTask = await _context.Tasks.Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync();

            if (existingTask == null)
                return ManageTaskResult.DoesNotExist;

            existingTask.Status = Entities.TaskStatus.Deleted;
            existingTask.DeletedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ManageTaskResult.Success;
        }

        public async Task<ManageTaskResult> FinishTaskAsync(FinishTaskRequest request)
        {
            var existingTask = await _context.Tasks.Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync();

            if (existingTask == null)
                return ManageTaskResult.DoesNotExist;

            existingTask.Status = Entities.TaskStatus.Finished;

            await _context.SaveChangesAsync();

            return ManageTaskResult.Success;
        }

        public async Task<ManageTaskResult> ReopenTaskAsync(ReopenTaskRequest request)
        {
            var existingTask = await _context.Tasks.Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync();

            if (existingTask == null)
                return ManageTaskResult.DoesNotExist;

            existingTask.Status = Entities.TaskStatus.New;

            await _context.SaveChangesAsync();

            return ManageTaskResult.Success;
        }

        public async Task<ManageTaskResult> RestoreTaskAsync(RestoreTaskRequest request)
        {
            var existingTask = await _context.Tasks.Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync();

            if (existingTask == null)
                return ManageTaskResult.DoesNotExist;

            existingTask.Status = Entities.TaskStatus.New;
            existingTask.DeletedDate = null;

            await _context.SaveChangesAsync();

            return ManageTaskResult.Success;
        }

        public async Task<GetTask?> GetSingleTaskAsync(Guid taskId)
        {
            var task = await _context.Tasks.Where(t => t.Id == taskId)
                .Select(t => new GetTask
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Status = t.Status
                }).AsNoTracking().FirstOrDefaultAsync();

            return task;
        }

        public async Task<SearchResult> GetTasksAsync(TaskFilters? filters)
        {
            var query = _context.Tasks.Where(t => true);

            if(filters != null)
            {
                // Search something close to name.
                // I would normaly use Soundex, but, apparently, it is accessible only in "normal" databases :(
                if (!string.IsNullOrEmpty(filters.Name)) 
                    query = query.Where(t => EF.Functions.Like(t.Name, filters.Name));

                // Do not display deleted tasks
                if (!filters.ShowDeleted)
                    query = query.Where(t => t.Status != Entities.TaskStatus.Deleted);

                // Do not display expired tasks
                if (!filters.ShowOverdue)
                    query = query.Where(t => t.Status != Entities.TaskStatus.Overdue);

                // Do not display expired tasks
                if (!filters.ShowFinished)
                    query = query.Where(t => t.Status != Entities.TaskStatus.Finished);
            }

            var tasks = await query.Select(t => new GetShortTask
            {
                Id = t.Id,
                Name = t.Name,
                Status = t.Status
            }).ToListAsync();

            return new SearchResult(tasks);
        }

        private async Task<int> CheckDueDateCountAsync(DateTime? dueDate)
        {
            if (dueDate == null)
                return 0;

            return await _context.Tasks.Where(t => t.DueDate == dueDate)
                .CountAsync();
        }
    }
}
