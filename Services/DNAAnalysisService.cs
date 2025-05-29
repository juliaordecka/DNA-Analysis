using System.Text;

namespace DNA_Analyser.Services
{
    public class DnaAnalysisService : IDnaAnalysisService
    {
        private readonly ILogger<DnaAnalysisService> _logger;

        public DnaAnalysisService(ILogger<DnaAnalysisService> logger)
        {
            _logger = logger;
        }

        //otrzymanie dlugosci sekwencji
        public int GetSequenceLength(string sequence)
        {
            if (string.IsNullOrEmpty(sequence))
            {
                return 0;
            }
            return sequence.Length;
        }

        //otrzymanie sekwencji komplementarnej
        public string GetComplementarySequence(string sequence)
        {
            if (string.IsNullOrEmpty(sequence))
            {
                return string.Empty;
            }
            //string builder dla duzych sekwencji
            var complementary = new StringBuilder(sequence.Length);
            foreach (var nucleotide in sequence)
            {
                switch (nucleotide)
                {
                    case 'A':
                        complementary.Append('T');
                        break;
                    case 'T':
                        complementary.Append('A');
                        break;
                    case 'C':
                        complementary.Append('G');
                        break;
                    case 'G':
                        complementary.Append('C');
                        break;
                    default:
                        throw new ArgumentException($"Invalid nucleotide: {nucleotide}");
                }
            }
            return complementary.ToString();
        }

        //otrzymanie sekwencji odwrotnej
        public string ReverseSequence(string sequence)
        {
            if (string.IsNullOrEmpty(sequence))
            {
                return string.Empty;
            }
       
            var reverse = new StringBuilder(sequence.Length);
            for (int i = sequence.Length - 1; i >= 0; i--)
            {
                reverse.Append(sequence[i]);
            }
            return reverse.ToString();
        }
        //zamiana dna na rna
        public string ConvertDNAtoRNA(string sequence)
        {             
            if (string.IsNullOrEmpty(sequence))
            {
                return string.Empty;
            }
            var rna = new StringBuilder(sequence.Length);
            foreach (var nucleotide in sequence)
            {
                if (nucleotide == 'T')
                {
                    rna.Append('U');
                }
                else
                {
                    rna.Append(nucleotide);
                }
            }
            return rna.ToString();
        }
        //sprawdzenie na ktorej pozycji wystepuje dany podciag - zwraca pozycje pierwszego nukleotydu podciagu
        public List<int> FindPositionOfSubstring(string sequence, string substring)
        {
            List<int> positions = [];
            if (string.IsNullOrEmpty(sequence) || string.IsNullOrEmpty(substring))
            {
                return positions;
            }
            int index = 0;
            int foundIndex = sequence.IndexOf(substring, index);

            while (foundIndex != -1)
            {
                positions.Add(foundIndex+1);
                index = foundIndex + 1;
                foundIndex = sequence.IndexOf(substring, index);
            }

            return positions;
        }
        


    }
}
