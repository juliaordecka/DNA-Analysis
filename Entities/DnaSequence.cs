using System.ComponentModel.DataAnnotations;

namespace DNA_Analyser.Entities
{
    public class DnaSequence
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Sequence { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }
}
