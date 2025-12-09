using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMCT.Tests
{
    public class TastItemTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnBadRequest_WhenTitleTooShort()
        {
            var context = GetInMemoryContext();
            var controller = new HMCT.Controllers.TaskItem(context);

            var request = new HMCT.Controllers.TaskItem.CreateTaskRequest
            {
                Title = "ab", // too short
                Description = "test",
                Taskstatus = "Pending",
                DueDateTime = DateTime.Now.AddDays(1)
            };

            controller.ModelState.AddModelError("Title", "MinLength");

            var result = await controller.createTask(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public async Task CreateTask_ShouldReturnOk_WhenRequestIsValid()
        {
            var context = GetInMemoryContext();
            var controller = new HMCT.Controllers.TaskItem(context);

            var request = new HMCT.Controllers.TaskItem.CreateTaskRequest
            {
                Title = "Clean Kitchen",
                Description = "Do a proper cleaning",
                Taskstatus = "InProgress",
                DueDateTime = DateTime.Now.AddDays(2)
            };

            var result = await controller.createTask(request);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
