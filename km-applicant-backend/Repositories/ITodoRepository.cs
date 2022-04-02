using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using km_applicant_backend.Models;

namespace km_applicant_backend.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllTodosAsync();
        Task<Todo> GetTodoByIdAsync(int id);
        Task<Todo> CreateTodoAsync(Todo todo);
        Task UpdateTodoAsync(Todo todo);
        Task DeleteTodoAsync(int id);
        Task<Todo> ChangeTodoPercentageAsync(Todo todo, int percentage);
        Task<Todo> MarkTodoAsDoneAsync(Todo todo);
        Task<IEnumerable<Todo>> GetIncomingTodoAsync(double additionalDay);
    }
}
