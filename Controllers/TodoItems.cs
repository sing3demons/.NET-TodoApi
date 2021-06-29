using System.Collections;
namespace TodoApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
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

            if (databaseContext.TodoItems.Count() == 0)
            {
                databaseContext.TodoItems.Add(new TodoItem { Name = "Learn VBA" });
                databaseContext.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItems>>> GetTodoItem()
        {
            var result = await databaseContext.TodoItems.ToListAsync();

            return Ok(new { todoItems = result });
        }

        //GET: api/v1/todoItems/2
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItems>> GetTodoItem(long id)
        {
            var todoItem = await databaseContext.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return Ok(new { todoItem = todoItem });
        }

        [HttpPost]
        public async Task<ActionResult<TodoItems>> PostTodoItem(TodoItem todoItem)
        {
            databaseContext.TodoItems.Add(todoItem);
            await databaseContext.SaveChangesAsync();
            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            // return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id) return BadRequest();

            databaseContext.Entry(todoItem).State = EntityState.Modified;

            await databaseContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodaItem(long id)
        {
            var todoItem = await databaseContext.TodoItems.FindAsync(id);
            if(todoItem==null)return NotFound();

            databaseContext.TodoItems.Remove(todoItem);
            await databaseContext.SaveChangesAsync();
            return NoContent();
        }
    }
}