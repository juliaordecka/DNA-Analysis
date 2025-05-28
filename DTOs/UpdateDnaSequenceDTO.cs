using DNA_Analyser.Attributes;

namespace DNA_Analyser.DTOs
{
    public class UpdateDnaSequenceDTO
    {
        public string Name {  get; set; } = string.Empty;

        [ValidSequence]
        public string Sequence { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
