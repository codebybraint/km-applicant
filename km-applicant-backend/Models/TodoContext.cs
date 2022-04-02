using System;
using Microsoft.EntityFrameworkCore;

namespace km_applicant_backend.Models
{
    public class TodoContext : DbContext
    {
            public TodoContext(DbContextOptions<TodoContext> options)
                : base(options) => Database.EnsureCreated();
            public DbSet<Todo> Todos { get; set; }
    }
}
