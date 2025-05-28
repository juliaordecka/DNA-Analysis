using DNA_Analyser.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DNA_Analyser.DTOs
{
    public class CreateDnaSequenceDTO
    {
        public string Name { get; set; } = string.Empty;
        [Required]
        [ValidSequence]
        public string Sequence { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
