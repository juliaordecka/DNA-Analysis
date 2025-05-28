using DNA_Analyser.Data;
using DNA_Analyser.Entities;
using DNA_Analyser.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DNA_Analyser.DTOs;

namespace DNA_Analyser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DNAProjectController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IDNAAnalysisService _analysis;
        public DNAProjectController(DataContext context, IDNAAnalysisService analysis)
        {
            _context = context;
            _analysis = analysis;
        }

        //get - wprowadzone informacje o DNA - cala lista
        [HttpGet]
        public async Task<ActionResult<List<DnaSequence>>> Get()
        {
            return Ok(await _context.Sequences.ToListAsync());
        }
        //Get - tylko jedna instancja
        [HttpGet("{id}")]
        public async Task<ActionResult<DnaSequence>> Get(int id)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            return Ok(sekwencja);
        }
        //Post - dodanie nowej sekwencji DNA
        [HttpPost]
        public async Task<ActionResult<List<DnaSequence>>> Post(CreateDnaSequenceDTO request)
        {
            var sekwencja = new DnaSequence
            {
                Name = request.Name,
                Sequence = request.Sequence,
                Description = request.Description
            };

            _context.Sequences.Add(sekwencja);
            await _context.SaveChangesAsync();
            return Ok(sekwencja);
        }
        //Put - aktualizacja informacji dot. sekwencji DNA
        [HttpPut("{id}")]
        public async Task<ActionResult<List<DnaSequence>>> Put(int id, UpdateDnaSequenceDTO request)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            sekwencja.Name = request.Name;
            sekwencja.Sequence = request.Sequence;
            sekwencja.Description = request.Description;
            await _context.SaveChangesAsync();
            return Ok(sekwencja);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<DnaSequence>>> Delete(int id)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            _context.Sequences.Remove(sekwencja);
            await _context.SaveChangesAsync();
            return Ok(await _context.Sequences.ToListAsync());
        }

        //Get - dlugosc sekwencji DNA
        [HttpGet("length/{id}")]
        public async Task<ActionResult<int>> GetLengthOfSequence(int id)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            int length = _analysis.GetSequenceLength(sekwencja.Sequence);
            return Ok(length);
        }

        //Get - sekwencja komplementarna
        [HttpGet("complementary/{id}")]
        public async Task<ActionResult<string>> GetComplementarySequence(int id)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            string complementary = _analysis.GetComplementarySequence(sekwencja.Sequence);
            return Ok(complementary);
        }

        //Get - sekwencja odwrotna
        [HttpGet("reverse/{id}")]
        public async Task<ActionResult<string>> GetReverseSequence(int id)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            string reverse = _analysis.ReverseSequence(sekwencja.Sequence);
            return Ok(reverse);
        }

        //Get - sekwencja komplementarna i odwrotna
        [HttpGet("reverse-complementary/{id}")]
        public async Task<ActionResult<string>> GetReverseComplementarySequence(int id)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            string reverseComplementary = _analysis.GetComplementarySequence(_analysis.ReverseSequence(sekwencja.Sequence));
            return Ok(reverseComplementary);
        }

        //Get - zamiana dna na rna
        [HttpGet("rna/{id}")]
        public async Task<ActionResult<string>> GetRnaSequence(int id)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            string rna = _analysis.DNAtoRNA(sekwencja.Sequence);
            return Ok(rna);

        }

        [HttpGet("substring-positions/{id}")]
        public async Task<ActionResult<List<int>>> GetSubstringPositions(int id, [FromQuery] string substring)
        { 
            var sekwencja = await _context.Sequences.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            List <int> positions = _analysis.FindPositionOfSubstring(sekwencja.Sequence, substring);
            return Ok(positions);
        }
    }
}
