using DNA_Analyser.Data;
using DNA_Analyser.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DNA_Analyser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DNAProjectController : ControllerBase
    {
        private readonly DataContext context;
        private readonly DNAAnalysisService analysis;
        public DNAProjectController(DataContext context, DNAAnalysisService analysis)
        {
            this.context = context;
            this.analysis = analysis;
        }

        //Get - wprowadzone informacje o DNA przed analizą - cala lista
        [HttpGet]
        public async Task<ActionResult<List<DNA1>>> Get()
        {
            return Ok(await context.Sekwencje.ToListAsync());
        }
        //Get - tylko jedna instancja
        [HttpGet("{id}")]
        public async Task<ActionResult<DNA1>> Get(int id)
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
        public async Task<ActionResult<List<DNA1>>> Post(DNA1 sekwencja)
        {
            context.Sekwencje.Add(sekwencja);
            await context.SaveChangesAsync();
            return Ok(await context.Sekwencje.ToListAsync());
        }
        //Put - aktualizacja informacji dot. sekwencji DNA
        [HttpPut("{id}")]
        public async Task<ActionResult<List<DNA1>>> Put(DNA1 request)
        {
            var sekwencja = await context.Sekwencje.FindAsync(request.Id);
            if (sekwencja == null)
            {
                return NotFound("Nie znaleziono sekwencji DNA o podanym ID.");
            }
            sekwencja.Name = request.Name;
            sekwencja.Sequence = request.Sequence;
            sekwencja.Description = request.Description;
            await context.SaveChangesAsync();
            return Ok(await context.Sekwencje.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<DNA1>>> Delete(DNA1 request)
        {
            var sekwencja = await context.Sekwencje.FindAsync(request.Id);
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
            int length = analysis.GetLength(sekwencja.Sequence);
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
    }
}
