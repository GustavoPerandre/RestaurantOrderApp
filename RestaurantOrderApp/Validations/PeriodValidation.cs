using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderApp.Validations
{
    public class PeriodValidation : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (!String.IsNullOrEmpty((string)value))
            {
                if (value.ToString().ToUpper().Equals("MORNING") || value.ToString().ToUpper().Equals("NIGHT"))
                    return base.IsValid(value);
            }
            return false;
        }
    }
}
