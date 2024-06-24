using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Todo
    {

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public required string Description { get; set; }

    }
}
