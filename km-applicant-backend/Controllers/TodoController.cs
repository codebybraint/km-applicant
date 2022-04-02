using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using km_applicant_backend.Models;
using km_applicant_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/todo
        [HttpGet]
        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            var resultTodo = await todoRepository.GetAllTodosAsync();
            return resultTodo;
        }

        // GET api/todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodoByIdAsync(int id)
        {
            var resultTodo = await todoRepository.GetTodoByIdAsync(id);
            if (resultTodo == null) return NotFound();
            return resultTodo;
        }

        // POST api/todo
        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodoAsync([FromBody] Todo todo)
        {
            if (todo.title == null || todo.expirationDate < DateTime.Today || todo.percentageOfCompletion > 100)
            {
                return BadRequest();
            }
            var resultTodo = await todoRepository.CreateTodoAsync(todo);
            return CreatedAtAction(nameof(GetTodoByIdAsync), new { id = resultTodo.id }, resultTodo);
        }

        // PUT api/todo/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> UpdateTodoAsync(int id, [FromBody] Todo todo)
        {
            if (todo.id != id || todo.percentageOfCompletion > 100)
            {
                return BadRequest();
            }
            await todoRepository.UpdateTodoAsync(todo);
            // because UpdateTodoAsync is just Task, will not return any value from function
            return todo;
        }

        // DELETE api/todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoAsync(int id)
        {
            var resultTodo = await todoRepository.GetTodoByIdAsync(id);
            if (resultTodo == null) return NotFound();
            await todoRepository.DeleteTodoAsync(resultTodo.id);
            // because DeleteTodoAsync is just Task, will not return any value from function
            return Ok();
        }

        // GET api/todo/mark_done/5
        [HttpGet("mark_done/{id}")]
        public async Task<ActionResult<Todo>> MarkTodoAsDoneAsync(int id)
        {
            var resultTodo = await todoRepository.GetTodoByIdAsync(id);
            if (resultTodo == null) return NotFound();
            await todoRepository.MarkTodoAsDoneAsync(resultTodo);
            return resultTodo;
        }

        // GET api/todo/1/percentage=30
        [HttpGet("{id}/percentage={percentage}")]
        public async Task<ActionResult<Todo>> ChangeTodoPercentageAsync(int id, int percentage)
        {
            var resultTodo = await todoRepository.GetTodoByIdAsync(id);
            if (resultTodo == null) return NotFound();
            else if (percentage > 100) return BadRequest();
            await todoRepository.ChangeTodoPercentageAsync(resultTodo, percentage);
            return resultTodo;
        }

        // GET api/todo/incoming/1
        [HttpGet("incoming/{days}")]
        public async Task<IEnumerable<Todo>> GetIncomingTodoAsync(double days)
        {
            // Incoming feature
            // days = 0 (today)
            // days = 1 (next day)
            // days = 2 (day after next day)
            var resultTodo = await todoRepository.GetIncomingTodoAsync(days);
            return resultTodo;
        }
    }
}
