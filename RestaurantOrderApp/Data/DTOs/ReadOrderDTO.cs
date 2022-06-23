using RestaurantOrderApp.Validations;
using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderApp.Data.DTOs
{
    public class ReadOrderDTO
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [PeriodValidation(ErrorMessage = "The Period field is required and must be Morning or Night")]
        public string Period { get; set; }

        [MinLength(1, ErrorMessage = "At least one item required in dish order")]
        public string dishList { get; set; }
    }
}
