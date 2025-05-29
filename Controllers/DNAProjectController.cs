using Azure.Core;
using DNA_Analyser.Data;
using DNA_Analyser.DTOs;
using DNA_Analyser.Entities;
using DNA_Analyser.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DNA_Analyser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DNAProjectController : ControllerBase
    {
        private readonly IDnaAnalysisService _analysis;
        private readonly IDnaDataService _sequenceService;
        private readonly ILogger<DNAProjectController> _logger;
        public DNAProjectController(IDnaAnalysisService analysis, IDnaDataService sequenceService, ILogger<DNAProjectController> logger)
        {
            _analysis = analysis;
            _sequenceService = sequenceService;
            _logger = logger;
        }

        //get - wprowadzone informacje o DNA - cala lista
        [HttpGet]
        public async Task<ActionResult<List<DnaSequence>>> Get()
        {
            var sequences = await _sequenceService.GetAllSequences();
            return Ok(sequences);
        }
        //Get - tylko jedna instancja
        [HttpGet("{id}")]
        public async Task<ActionResult<DnaSequence>> Get(int id)
        {
            var sekwencja = await _sequenceService.GetSequenceById(id);
            if (sekwencja == null)
            {
                return NotFound("Nie istnieje sekwencja DNA o podanym ID");
            }
            return Ok(sekwencja);
        }
        //Post - dodanie nowej sekwencji DNA
        [HttpPost]
        public async Task<ActionResult<List<DnaSequence>>> Post(CreateDnaSequenceDTO request)
        {
            var sekwencja = await _sequenceService.CreateSequenceInstance(request);
            return Ok(sekwencja);
        }
        //Put - aktualizacja informacji dot. sekwencji DNA
        [HttpPut("{id}")]
        public async Task<ActionResult<List<DnaSequence>>> Put(int id, UpdateDnaSequenceDTO request)
        {
            try
            {
                var sequence = await _sequenceService.UpdateSequenceById(id, request);
                return Ok(sequence); 
            }
            catch (ArgumentException)
            {
                return NotFound("Nie istnieje sekwencja DNA o podanym ID"); 
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deletedseq = await _sequenceService.DeleteSequence(id);
            if (deletedseq == true) return NoContent();
            else return NoContent();
        }

        //Get - dlugosc sekwencji DNA
        [HttpGet("length/{id}")]
        public async Task<ActionResult<int>> GetLengthOfSequence(int id)
        {
            var sekwencja = await _sequenceService.GetSequenceById(id);
            if (sekwencja == null)
            {
                return NotFound("Nie istnieje sekwencja DNA o podanym ID");
            }
            int length = _analysis.GetSequenceLength(sekwencja.Sequence);
            return Ok(length);
        }

        //Get - sekwencja komplementarna
        [HttpGet("complementary/{id}")]
        public async Task<ActionResult<string>> GetComplementarySequence(int id)
        {
            var sekwencja = await _sequenceService.GetSequenceById(id);
            if (sekwencja == null)
            {
                return NotFound("Nie istnieje sekwencja DNA o podanym ID");
            }
            string complementary = _analysis.GetComplementarySequence(sekwencja.Sequence);
            return Ok(complementary);
        }

        //Get - sekwencja odwrotna
        [HttpGet("reverse/{id}")]
        public async Task<ActionResult<string>> GetReverseSequence(int id)
        {
            var sekwencja = await _sequenceService.GetSequenceById(id);
            if (sekwencja == null)
            {
                return NotFound("Nie istnieje sekwencja DNA o podanym ID");
            }
            string reverse = _analysis.ReverseSequence(sekwencja.Sequence);
            return Ok(reverse);
        }

        //Get - sekwencja komplementarna i odwrotna
        [HttpGet("reverse-complementary/{id}")]
        public async Task<ActionResult<string>> GetReverseComplementarySequence(int id)
        {
            var sekwencja = await _sequenceService.GetSequenceById(id);
            if (sekwencja == null)
            {
                return NotFound("Nie istnieje sekwencja DNA o podanym ID");
            }
            string reverseComplementary = _analysis.GetComplementarySequence(_analysis.ReverseSequence(sekwencja.Sequence));
            return Ok(reverseComplementary);
        }

        //Get - zamiana dna na rna
        [HttpGet("rna/{id}")]
        public async Task<ActionResult<string>> GetRnaSequence(int id)
        {
            var sekwencja = await _sequenceService.GetSequenceById(id);
            if (sekwencja == null)
            {
                return NotFound("Nie istnieje sekwencja DNA o podanym ID");
            }
            string rna = _analysis.ConvertDNAtoRNA(sekwencja.Sequence);
            return Ok(rna);

        }

        [HttpGet("substring-positions/{id}")]
        public async Task<ActionResult<List<int>>> GetSubstringPositions(int id, [FromQuery] string substring)
        {
            var sekwencja = await _sequenceService.GetSequenceById(id);
            if (sekwencja == null)
            {
                return NotFound("Nie istnieje sekwencja DNA o podanym ID");
            }
            List <int> positions = _analysis.FindPositionOfSubstring(sekwencja.Sequence, substring);
            return Ok(positions);
        }
    }
}
