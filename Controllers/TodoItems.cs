using System.Collections;
namespace TodoApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TodoApi.Database;
    using TodoApi.Models;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodoItems : ControllerBase
    {
        private readonly DatabaseContext databaseContext;

        public TodoItems(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItems>>> GetTodoItem()
        {
            var result = await databaseContext.TodoItems.ToListAsync();

            return Ok(new { todoItems = result});
        }

        [HttpPost]
        public async Task<ActionResult<TodoItems>> PostTodoItem(TodoItem todoItem)
        {
            databaseContext.TodoItems.Add(todoItem);
            await databaseContext.SaveChangesAsync();
            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            // return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }
    }
}