using Microsoft.EntityFrameworkCore;
using TaskManagerLite.Data;
using TaskManagerLite.Interfaces;

namespace TaskManagerLite.Repositories
{
    public class BackgroundRepository : IBackgroundRepository
    {
        private readonly TaskContext _context;
        private const short _oldTasksDaySpan = 30;

        public BackgroundRepository(TaskContext context)
        {
            _context = context;
        }

        // Due to the fact, that I am using InMemory database
        // I am unable to gain direct access to the databse, thus, no pure SQL :(

        public async Task MarkCloseToOverdueTasksAsync()
        {
            var utcNow = DateTime.UtcNow.ToLocalTime();

            //var sql = $@"
            //    UPDATE Tasks
            //    SET Status = {(int)Entities.TaskStatus.CloseToOverdue}
            //    WHERE DATEDIFF(HOUR, DueDate, {utcNow}) <= {24}";

            //await _context.Database.ExecuteSqlRawAsync(sql);

            var tasksToUpdate = await _context.Tasks
                .Where(t => t.Status == Entities.TaskStatus.New
                    && t.DueDate != null 
                    && (t.DueDate - utcNow).Value.TotalHours <= 24)
                .ToListAsync();

            foreach (var task in tasksToUpdate)
            {
                task.Status = Entities.TaskStatus.CloseToOverdue;
            }

            await _context.SaveChangesAsync();
        }

        public async Task MarkOverdueTasksAsync()
        {
            var utcNow = DateTime.UtcNow.ToLocalTime();

            //var sql = $@"
            //    UPDATE Tasks
            //    SET Status = {(int)Entities.TaskStatus.Overdue}
            //    WHERE DATEDIFF(SECOND, DueDate, {utcNow}) <= {0}";

            //await _context.Database.ExecuteSqlRawAsync(sql);

            var tasks = await _context.Tasks.ToListAsync();

            var tasksToUpdate = await _context.Tasks
                .Where(t => t.Status != Entities.TaskStatus.Overdue 
                    && t.Status != Entities.TaskStatus.Finished
                    && t.Status != Entities.TaskStatus.Deleted
                    && t.DueDate != null 
                    && (t.DueDate - utcNow).Value.TotalSeconds <= 0)
                .ToListAsync();

            foreach (var task in tasksToUpdate)
            {
                task.Status = Entities.TaskStatus.Overdue;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOldTasksAsync()
        {
            var utcNow = DateTime.UtcNow.ToLocalTime();

            //var sql = $"DELETE FROM \"Tasks\" WHERE EXTRACT(DAY FROM (NOW() - \"DueDate\")) >= {_oldTasksDaySpan}";
            //await _context.Database.ExecuteSqlRawAsync(sql);

            var tasksToDelete = await _context.Tasks
                .Where(t => t.Status != Entities.TaskStatus.Deleted 
                    && t.DeletedDate != null 
                    && (utcNow - t.DeletedDate).Value.TotalDays >= _oldTasksDaySpan)
                .ToListAsync();

            _context.RemoveRange(tasksToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
