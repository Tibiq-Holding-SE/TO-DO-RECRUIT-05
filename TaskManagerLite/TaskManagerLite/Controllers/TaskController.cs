using Microsoft.AspNetCore.Mvc;
using TaskManagerLite.Entities;
using TaskManagerLite.Interfaces;

namespace TaskManagerLite.Controllers
{
    [Route("Tasks")]
    public class TaskController : Controller
    {
        [HttpGet("tasks")]
        public IActionResult Tasks()
        {
            return View("Tasks");
        }

        //Get partial tasks info (List menu)
        [HttpPost("tasks")]
        public async Task<IActionResult> Tasks([FromServices] ITaskRepository taskRepo, [FromBody] TaskFilters? filters)
        {
            var result = await taskRepo.GetTasksAsync(filters);
            return Json(result);
        }

        //Get full task info
        [HttpGet("task")]
        public async Task<IActionResult> GetTask([FromServices] ITaskRepository taskRepo, [FromQuery] Guid Id)
        {
            var task = await taskRepo.GetSingleTaskAsync(Id);
            return Ok(task);
        }

        //Create task
        [HttpPost("task")]
        public async Task<IActionResult> CreateTasks([FromServices] ITaskRepository taskRepo, [FromBody] CreateTaskRequest request)
        {
            var result = await taskRepo.CreateTaskAsync(request);
            return Ok(result);
        }

        //Open dialog
        [HttpGet("create-task")]
        public IActionResult CreateTasks()
        {
            return PartialView("_CreateTask");
        }

        //Open dialog
        [HttpGet("edit-task")]
        public async Task<IActionResult> EditTasks([FromServices] ITaskRepository taskRepo,[FromQuery] Guid id)
        {
            var task = await taskRepo.GetSingleTaskAsync(id);
            return PartialView("_EditTask", task);
        }

        //Open dialog
        [HttpGet("delete-task")]
        public IActionResult DeleteTask([FromQuery] Guid id)
        {
            var model = new DeleteTaskRequest
            {
                Id = id
            };

            return PartialView("_DeleteTask", model);
        }

        //Edit task
        [HttpPost("edit-task")]
        public async Task<IActionResult> EditTasks([FromServices] ITaskRepository taskRepo, [FromBody] EditTaskRequest request)
        {
            var result = await taskRepo.EditTaskAsync(request);
            return Ok(result);
        }

        //Change name
        [HttpPost("change-name")]
        public async Task<IActionResult> ChangeName([FromServices] ITaskRepository taskRepo, [FromBody] ChangeNameRequest request)
        {
            await taskRepo.ChangeNameAsync(request);
            return Ok();
        }

        //Delete task
        [HttpPost("delete-task")]
        public async Task<IActionResult> DeleteTask([FromServices] ITaskRepository taskRepo, [FromBody] DeleteTaskRequest request)
        {
            var response = await taskRepo.DeleteTaskAsync(request);
            return Ok(response);
        }

        //Finish task
        [HttpPost("finish-task")]
        public async Task<IActionResult> FinishTask([FromServices] ITaskRepository taskRepo, [FromBody] FinishTaskRequest request)
        {
            var response = await taskRepo.FinishTaskAsync(request);
            return Ok(response);
        }

        //Reopen task (if finished)
        [HttpPost("reopen-task")]
        public async Task<IActionResult> ReopenTask([FromServices] ITaskRepository taskRepo, [FromBody] ReopenTaskRequest request)
        {
            var response = await taskRepo.ReopenTaskAsync(request);
            return Ok(response);
        }

        //Restore task (if deleted)
        [HttpPost("restore-task")]
        public async Task<IActionResult> RestoreTask([FromServices] ITaskRepository taskRepo, [FromBody] RestoreTaskRequest request)
        {
            var response = await taskRepo.RestoreTaskAsync(request);
            return Ok(response);
        }
    }
}
