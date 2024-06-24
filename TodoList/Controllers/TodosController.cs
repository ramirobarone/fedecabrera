using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Repository.Interfaces;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TodosController : ControllerBase
    {
        private readonly IRepository<Todo> _todoRepository;
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<TodosController> _logger;

        public TodosController(IRepository<Todo> todoRepository, IFeatureManager featureManager, ILogger<TodosController> logger)
        {
            _todoRepository = todoRepository;
            _featureManager = featureManager;
            _logger = logger;
        }

        [HttpGet]
        [FeatureGate(FeatureFlags.FeatureGet)] // Esta es otra forma de aplicar el GetFeature donde se evita colocar codigo en el endpoint
        [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status501NotImplemented)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status405MethodNotAllowed)]

        public async Task<IActionResult> Get()
        {
            if (!await _featureManager.IsEnabledAsync(FeatureFlags.FeatureGet))
                return NotFound("Feature not enabled");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<Todo> _todos = await _todoRepository.Get();

            return Ok(_todos);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Todo? myItem = await _todoRepository.GetId(id);

            if (myItem is null)
                return NotFound($"Item with ID {id} not found.");

            return Ok(myItem);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Todo), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Todo), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status501NotImplemented)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTodo([FromBody] string description)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Todo todo = await _todoRepository.Create(description);

            return Created("/Todo", todo);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Todo), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status501NotImplemented)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] string description)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            Todo? todo = await _todoRepository.Put(id, description);

            if (todo is null)
                return NotFound($"Item with ID {id} not found.");

            return Ok($"Id {id} updated");
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Todo), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Todo), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status501NotImplemented)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool remove = await _todoRepository.Delete(id);

            return !remove ? NotFound($"Item with ID {id} not found.") : Ok($"Id {id} removed");
        }
    }
}