using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using km_applicant_backend.Models;
using km_applicant_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace km_applicant_backend.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository todoRepository;

        public TodoController(ITodoRepository repository)
        {
            todoRepository = repository;
        }

        // GET: api/
        [HttpGet]
        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            var _todo = await todoRepository.GetAllTodosAsync();
            return _todo;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodoByIdAsync(int id)
        {
            var _todo = await todoRepository.GetTodoByIdAsync(id);
            if (_todo == null) return NotFound();
            return _todo;
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodoAsync([FromBody] Todo todo)
        {
            if (todo.title == null || todo.expirationDate < DateTime.Today || todo.percentageOfCompletion > 100)
            {
                return BadRequest();
            }
            var _todo = await todoRepository.CreateTodoAsync(todo);
            return CreatedAtAction(nameof(GetTodoByIdAsync), new { id = todo.id }, todo);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> UpdateTodoAsync(int id, [FromBody] Todo todo)
        {
            if (todo.id != id || todo.percentageOfCompletion > 100)
            {
                return BadRequest();
            }
            await todoRepository.UpdateTodoAsync(todo);
            return todo;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoAsync(int id)
        {
            var _todo = await todoRepository.GetTodoByIdAsync(id);
            if (_todo == null) return NotFound();
            await todoRepository.DeleteTodoAsync(_todo.id);
            return Ok();
        }

        // GET api/values/mark_done/5
        [HttpGet("mark_done/{id}")]
        public async Task<ActionResult<Todo>> MarkTodoAsDoneAsync(int id)
        {
            var _todo = await todoRepository.GetTodoByIdAsync(id);
            if (_todo == null) return NotFound();
            await todoRepository.MarkTodoAsDoneAsync(_todo);
            return _todo;
        }

        // GET api/values/percentage/@id=1&@percentage=12
        [HttpGet("{id}/percentage={percentage}")]
        public async Task<ActionResult<Todo>> ChangeTodoPercentageAsync(int id, int percentage)
        {
            var _todo = await todoRepository.GetTodoByIdAsync(id);
            if (_todo == null) return NotFound();
            else if (percentage > 100) return BadRequest();
            await todoRepository.ChangeTodoPercentageAsync(_todo, percentage);
            return _todo;
        }
        /** Incoming feature 
         *  0 = today
         *  1 = next day, so on
         */
        // GET api/values/incoming/5
        [HttpGet("incoming/{days}")]
        public async Task<IEnumerable<Todo>> GetIncomingTodoAsync(double days)
        {
            var _todo = await todoRepository.GetIncomingTodoAsync(days);
            return _todo;
        }
    }
}
