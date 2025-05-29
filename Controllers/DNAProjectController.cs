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
        private readonly IDnaDataService _dataService;
        private readonly ILogger<DNAProjectController> _logger;
        public DNAProjectController(IDnaAnalysisService analysis, IDnaDataService dataService, ILogger<DNAProjectController> logger)
        {
            _analysis = analysis;
            _dataService = dataService;
            _logger = logger;
        }

        //get - wprowadzone informacje o DNA - cala lista
        [HttpGet]
        public async Task<ActionResult<List<DnaSequence>>> Get()
        {
            try
            {
                var sequences = await _dataService.GetAllSequences();
                return Ok(sequences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Retrieving sequences error");
                return StatusCode(500);
            }
        }

        //Get - tylko jedna instancja
        [HttpGet("{id}")]
        public async Task<ActionResult<DnaSequence>> Get(int id)
        {
            try
            {
                var sekwencja = await _dataService.GetSequenceById(id);
                if (sekwencja == null)
                {
                    return NotFound("Sequence of given id does not exist");
                }
                return Ok(sekwencja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Retrieving sequence of with ID: {Id} error", id);
                return StatusCode(500);
            }
        }
        //Post - dodanie nowej sekwencji DNA
        [HttpPost]
        public async Task<ActionResult<List<DnaSequence>>> Post(CreateDnaSequenceDTO request)
        {
            try
            {
                var sekwencja = await _dataService.CreateSequenceInstance(request);
                return Ok(sekwencja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating new sequence");
                return StatusCode(500);
            }
        }
        //Put - aktualizacja informacji dot. sekwencji DNA
        [HttpPut("{id}")]
        public async Task<ActionResult<List<DnaSequence>>> Put(int id, UpdateDnaSequenceDTO request)
        {
            try
            {
                var sekwencja = await _dataService.UpdateSequenceById(id, request);
                if (sekwencja == null)
                {
                    return NotFound("Sequence of given id does not exist");
                }
                return Ok(sekwencja); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating sequence");
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deletedseq = await _dataService.DeleteSequence(id);
                if (deletedseq == true) return NoContent();
                else return NotFound("Nie istnieje sekwencja DNA o podanym ID.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting sequence");
                return StatusCode(500);
            }
        }

        //Get - dlugosc sekwencji DNA
        [HttpGet("length/{id}")]
        public async Task<ActionResult<int>> GetLengthOfSequence(int id)
        {
            try
            {
                var sekwencja = await _dataService.GetSequenceById(id);
                if (sekwencja == null)
                {
                    return NotFound("Sequence of given id does not exist");
                }
                int length = _analysis.GetSequenceLength(sekwencja.Sequence);
                return Ok(length);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while obtaining length");
                return StatusCode(500);
            }
        }

        //Get - sekwencja komplementarna
        [HttpGet("complementary/{id}")]
        public async Task<ActionResult<string>> GetComplementarySequence(int id)
        {
            try
            {
                var sekwencja = await _dataService.GetSequenceById(id);
                if (sekwencja == null)
                {
                    return NotFound("Sequence of given id does not exist");
                }
                string complementary = _analysis.GetComplementarySequence(sekwencja.Sequence);
                return Ok(complementary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while obtaining complementary sequence");
                return StatusCode(500);
            }
        }

        //Get - sekwencja odwrotna
        [HttpGet("reverse/{id}")]
        public async Task<ActionResult<string>> GetReverseSequence(int id)
        {
            try
            {
                var sekwencja = await _dataService.GetSequenceById(id);
                if (sekwencja == null)
                {
                    return NotFound("Sequence of given id does not exist");
                }
                string reverse = _analysis.ReverseSequence(sekwencja.Sequence);
                return Ok(reverse);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while obtaining reverse sequence");
                return StatusCode(500);
            }
        }

        //Get - sekwencja komplementarna i odwrotna
        [HttpGet("reverse-complementary/{id}")]
        public async Task<ActionResult<string>> GetReverseComplementarySequence(int id)
        {
            try
            {
                var sekwencja = await _dataService.GetSequenceById(id);
                if (sekwencja == null)
                {
                    return NotFound("Sequence of given id does not exist");
                }
                string reverseComplementary = _analysis.GetComplementarySequence(_analysis.ReverseSequence(sekwencja.Sequence));
                return Ok(reverseComplementary);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while obtaining reverse-complementary sequence");
                return StatusCode(500);
            }
        }

        //Get - zamiana dna na rna
        [HttpGet("rna/{id}")]
        public async Task<ActionResult<string>> GetRnaSequence(int id)
        {
            try
            {
                var sekwencja = await _dataService.GetSequenceById(id);
                if (sekwencja == null)
                {
                    return NotFound("Sequence of given id does not exist");
                }
                string rna = _analysis.ConvertDNAtoRNA(sekwencja.Sequence);
                return Ok(rna);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while obtaining RNA sequence");
                return StatusCode(500);
            }

        }
        //get - pozycje danego podciagu wystepujacego w sekwencji
        [HttpGet("substring-positions/{id}")]
        public async Task<ActionResult<List<int>>> GetSubstringPositions(int id, [FromQuery] string substring)
        {
            try
            {
                var sekwencja = await _dataService.GetSequenceById(id);
                if (sekwencja == null)
                {
                    return NotFound("Sequence of given id does not exist");
                }
                List<int> positions = _analysis.FindPositionOfSubstring(sekwencja.Sequence, substring);
                return Ok(positions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while finding substring positions in sequence");
                return StatusCode(500);
            }
        }
    }
}
