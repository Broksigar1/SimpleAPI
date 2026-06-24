using System.ComponentModel.DataAnnotations;

namespace Model
{
    public record ChangeProductCountDto
    {
        [Required]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "Name must not be empty or includes only whitespaces")]
        public string Name { get; init; }


        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Count must be a non-negative integer")]
        public int Count { get; init; }
    }
}
