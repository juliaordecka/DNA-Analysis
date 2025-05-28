namespace DNA_Analyser.Services
{
    public interface IDNAAnalysisService
    {
        int GetSequenceLength(string sequence);
        string GetComplementarySequence(string sequence);
        string ReverseSequence(string sequence);
        string DNAtoRNA(string sequence);
        List<int> FindPositionOfSubstring(string sequence, string substring);

    }
}
