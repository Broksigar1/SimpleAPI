using System.ComponentModel.DataAnnotations;

namespace Model
{
    public record RenameCategoryDto
    {
        [Required]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "Name must not be empty or includes only whitespaces")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "NewName must not be empty or includes only whitespaces")]
        public string NewName { get; set; }
    }
}
