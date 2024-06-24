using Entities;
using Repository.Interfaces;

namespace Repository
{
    public class Repository<T> : IRepository<Todo>
    {
        //List only for Marvin test
        private static List<Todo> todos = new List<Todo>
        {
            new Todo { Id = 1, Description = "Study" },
            new Todo { Id = 2, Description = "Drink coffee" },
            new Todo { Id = 3, Description = "Play football" }
        };

        public Task<Todo> Create(string description)
        {
            Task<Todo> taskCreate = Task<Todo>.Run(() =>
            {
                var addTodo = new Todo()
                {
                    Id = todos.Max(todo => todo.Id) + 1,
                    Description = description
                };

                todos.Add(addTodo);

                return addTodo;
            });

            return taskCreate;
        }

        public Task<bool> Delete(int id)
        {
            return Task<bool>.Run(() =>
            {
                Todo? _todoToDelete = todos?.FirstOrDefault(x => x.Id == id);

                if (_todoToDelete is null)
                    return false;

                return todos.Remove(_todoToDelete);
            });
        }
        public Task<IEnumerable<Todo>> Get()
        {
            return Task.FromResult(todos ?? Enumerable.Empty<Todo>());
        }
        public Task<Todo?> GetId(int id)
        {
            Todo? _todo = todos.AsParallel().FirstOrDefault(todo => todo.Id == id);

            return Task.FromResult(_todo);
        }

        public Task<Todo?> Put(int id, string description)
        {
            return Task<Todo>.Run(() =>
            {
                var miElemento = todos.FirstOrDefault(todo => todo.Id == id);

                if (miElemento is null)
                    return null;

                miElemento.Description = description;

                return miElemento;
            });
        }
    }
}