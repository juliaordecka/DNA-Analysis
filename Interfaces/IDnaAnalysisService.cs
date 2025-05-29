namespace DNA_Analyser.Interfaces
{
    //operacje na string
    public interface IDnaAnalysisService
    {
        int GetSequenceLength(string sequence);
        string GetComplementarySequence(string sequence);
        string ReverseSequence(string sequence);
        string ConvertDNAtoRNA(string sequence);
        List<int> FindPositionOfSubstring(string sequence, string substring);

    }
}
