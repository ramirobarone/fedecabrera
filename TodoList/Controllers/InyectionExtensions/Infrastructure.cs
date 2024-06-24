using Entities;
using Repository;
using Repository.Interfaces;

namespace TodoList.Controllers.InyectionExtensions
{
    public static class Infrastructure
    {
        public static void AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRepository<Todo>, Repository<Todo>>();

        }
    }
}
