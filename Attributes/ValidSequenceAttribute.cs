using System.ComponentModel.DataAnnotations;

namespace DNA_Analyser.Attributes
{
    public class ValidSequenceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string sequence)
            {
                foreach (char n in sequence)
                {
                    if (n != 'A' && n != 'C' && n != 'T' && n != 'G')
                        return new ValidationResult("Sequence must only contain valid nucleotides (A/C/G/T)");
                }
            }
            return ValidationResult.Success;
        }

    }
}
