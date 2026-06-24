using System.ComponentModel.DataAnnotations;

namespace Model
{
    public sealed record Product(
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer")]
        int CategoryId,

        [Required]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "Name must not be empty or includes only whitespaces")]
        string Name,

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Count must be a non-negative integer")]
        int Count);
}
