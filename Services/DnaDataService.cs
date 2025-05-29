using DNA_Analyser.Data;
using DNA_Analyser.DTOs;
using DNA_Analyser.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DNA_Analyser.Services
{
    public class DnaDataService : IDnaDataService
    {
        private DataContext _context;
        private readonly ILogger<DnaDataService> _logger;
        public DnaDataService(DataContext context, ILogger<DnaDataService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<DnaSequence>> GetAllSequences()
        {
            _logger.LogInformation("Retrieving all stored DNA sequences.");
            try
            {
                _logger.LogInformation("Successfully retrieved DNA sequences");
                return await _context.Sequences.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all stored sequences");
                throw;
            }
        }

        public async Task<DnaSequence> GetSequenceById(int id)
        {
            _logger.LogInformation("Retreiving sequence of id: {Id}", id);
            try
            {
                var sekwencja = await _context.Sequences.FindAsync(id);
                if(sekwencja==null)
                {
                    _logger.LogInformation("Sequence of id: {Id} not found", id);
                }
                else
                {
                    _logger.LogInformation("Successfully retrieved sequence of id: {Id}", id);
                }
                return sekwencja;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving sequence of id: {Id}", id);
                throw;
            }
        }

        public async Task<List<DnaSequence>> CreateSequenceInstance(CreateDnaSequenceDTO request)
        {
            _logger.LogInformation("Adding new sequence");
            try
            {
                var sekwencja = new DnaSequence
                {
                    Name = request.Name,
                    Sequence = request.Sequence,
                    Description = request.Description
                };

                _context.Sequences.Add(sekwencja);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully created DNA sequence  of ID: {Id}", sekwencja.Id);
                return await _context.Sequences.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a new sequence");
                throw;
            }
        }

        public async Task<List<DnaSequence>> UpdateSequenceById(int id, UpdateDnaSequenceDTO request)
        {
            _logger.LogInformation("Updating sequence information with ID: {Id}", id);
            try
            {
                var sekwencja = await _context.Sequences.FindAsync(id);
                if (sekwencja == null)
                {
                    _logger.LogInformation("Sequence of id: {Id} not found", id);
                }
                sekwencja.Name = request.Name;
                sekwencja.Sequence = request.Sequence;
                sekwencja.Description = request.Description;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated DNA sequence  of ID: {Id}", sekwencja.Id);
                return await _context.Sequences.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating sequence with ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteSequence(int id)
        {
            _logger.LogInformation("Deleting sequence information with ID: {Id}", id);
            try
            {
                var sekwencja = await _context.Sequences.FindAsync(id);
                if (sekwencja == null)
                {
                    _logger.LogInformation("Sequence of id: {Id} not found", id);
                    return false;
                }
                _context.Sequences.Remove(sekwencja);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while deleting sequence with ID: {Id}", id);
                throw;
            }
        }
    }
}
