namespace TaskManagerLite.Entities;

public enum TaskStatus
{
    New, 
    CloseToOverdue, // 1 day till due date
    Overdue,
    Deleted,
    Finished
}
