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
        private readonly DataContext context;
        private readonly IDNAAnalysisService analysis;
        public DNAProjectController(DataContext context, IDNAAnalysisService analysis)
        {
            this.context = context;
            this.analysis = analysis;
        }

        //get - wprowadzone informacje o DNA - cala lista
        [HttpGet]
        public async Task<ActionResult<List<DnaSequence>>> Get()
        {
            return Ok(await context.Sekwencje.ToListAsync());
        }
        //Get - tylko jedna instancja
        [HttpGet("{id}")]
        public async Task<ActionResult<DnaSequence>> Get(int id)
        {
            var sekwencja = await context.Sekwencje.FindAsync(id);
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

            context.Sekwencje.Add(sekwencja);
            await context.SaveChangesAsync();
            return Ok(sekwencja);
        }
        //Put - aktualizacja informacji dot. sekwencji DNA
        [HttpPut("{id}")]
        public async Task<ActionResult<List<DnaSequence>>> Put(int id, UpdateDnaSequenceDTO request)
        {
            var sekwencja = await context.Sekwencje.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            sekwencja.Name = request.Name;
            sekwencja.Sequence = request.Sequence;
            sekwencja.Description = request.Description;
            await context.SaveChangesAsync();
            return Ok(sekwencja);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<DnaSequence>>> Delete(int id)
        {
            var sekwencja = await context.Sekwencje.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            context.Sekwencje.Remove(sekwencja);
            await context.SaveChangesAsync();
            return Ok(await context.Sekwencje.ToListAsync());
        }

        //Get - dlugosc sekwencji DNA
        [HttpGet("length/{id}")]
        public async Task<ActionResult<int>> GetLengthOfSequence(int id)
        {
            var sekwencja = await context.Sekwencje.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            int length = analysis.GetSequenceLength(sekwencja.Sequence);
            return Ok(length);
        }

        //Get - sekwencja komplementarna
        [HttpGet("complementary/{id}")]
        public async Task<ActionResult<string>> GetComplementarySequence(int id)
        {
            var sekwencja = await context.Sekwencje.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            string complementary = analysis.GetComplementarySequence(sekwencja.Sequence);
            return Ok(complementary);
        }

        //Get - sekwencja odwrotna
        [HttpGet("reverse/{id}")]
        public async Task<ActionResult<string>> GetReverseSequence(int id)
        {
            var sekwencja = await context.Sekwencje.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            string reverse = analysis.ReverseSequence(sekwencja.Sequence);
            return Ok(reverse);
        }

        //Get - sekwencja komplementarna i odwrotna
        [HttpGet("reverse-complementary/{id}")]
        public async Task<ActionResult<string>> GetReverseComplementarySequence(int id)
        {
            var sekwencja = await context.Sekwencje.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            string reverseComplementary = analysis.GetComplementarySequence(analysis.ReverseSequence(sekwencja.Sequence));
            return Ok(reverseComplementary);
        }

        //Get - zamiana dna na rna
        [HttpGet("rna/{id}")]
        public async Task<ActionResult<string>> GetRnaSequence(int id)
        {
            var sekwencja = await context.Sekwencje.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            string rna = analysis.DNAtoRNA(sekwencja.Sequence);
            return Ok(rna);

        }

        [HttpGet("substring-positions/{id}")]
        public async Task<ActionResult<List<int>>> GetSubstringPositions(int id, [FromQuery] string substring)
        { 
            var sekwencja = await context.Sekwencje.FindAsync(id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            List <int> positions = analysis.FindPositionOfSubstring(sekwencja.Sequence, substring);
            return Ok(positions);
        }
    }
}
