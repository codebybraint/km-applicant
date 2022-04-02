using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using km_applicant_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace km_applicant_backend.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            this._context = context;
        }

        public async Task<Todo> CreateTodoAsync(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        // UpdateTodoAsync is just Task, will not return value
        public async Task DeleteTodoAsync(int id)
        {
            var _todo = await _context.Todos.FindAsync(id);
            _context.Todos.Remove(_todo);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo> GetTodoByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);

        }

        public async Task<IEnumerable<Todo>> GetIncomingTodoAsync(double additionalDay)
        {
            // sql statement, to select all todos,
            //where expiration date between now, until the given additional days
            var sql = "SELECT * FROM Todos WHERE " +
                "expirationDate BETWEEN {0} AND {1}";
            return await _context.Todos.FromSqlRaw(sql, DateTime.Now, DateTime.Today.AddDays(additionalDay + 1)).ToListAsync();
        }

        public async Task<Todo> ChangeTodoPercentageAsync(Todo todo, int percentage)
        {
            todo.percentageOfCompletion = percentage;
            await UpdateTodoAsync(todo);
            return todo;
        }

        public async Task<Todo> MarkTodoAsDoneAsync(Todo todo)
        {
            todo.percentageOfCompletion = 100;
            await UpdateTodoAsync(todo);
            return todo;
        }

        // UpdateTodoAsync is just Task, will not return value
        public async Task UpdateTodoAsync(Todo todo)
        {
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
