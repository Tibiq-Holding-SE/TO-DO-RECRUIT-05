using System.ComponentModel.DataAnnotations;

namespace TaskManagerLite.Entities.Tasks
{
#nullable disable
    public class SimpleTask
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public TaskStatus Status { get; set; }

        //For entity framework
        public SimpleTask()
        {}

        public SimpleTask(string name, string description, DateTime? dueDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            DueDate = dueDate;
            Status = TaskStatus.New;
        }
    }

}
