using DNA_Analyser.Data;
using DNA_Analyser.DTOs;
using DNA_Analyser.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DNA_Analyser.Services
{
    public class DnaDataService : IDnaDataService
    {
        private DataContext _context;
        public DnaDataService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<DnaSequence>> GetAllSequences()
        {
            return await _context.Sequences.ToListAsync();
        }

        public async Task<DnaSequence> GetSequenceById(int id)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            return sekwencja;
        }

        public async Task<List<DnaSequence>> CreateSequenceInstance(CreateDnaSequenceDTO request)
        {
            var sekwencja = new DnaSequence
            {
                Name = request.Name,
                Sequence = request.Sequence,
                Description = request.Description
            };

            _context.Sequences.Add(sekwencja);
            await _context.SaveChangesAsync();
            return await _context.Sequences.ToListAsync();
        }

        public async Task<List<DnaSequence>> UpdateSequenceById(int id, UpdateDnaSequenceDTO request)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            sekwencja.Name = request.Name;
            sekwencja.Sequence = request.Sequence;
            sekwencja.Description = request.Description;
            await _context.SaveChangesAsync();
            return await _context.Sequences.ToListAsync();
        }

        public async Task<bool> DeleteSequence(int id)
        {
            var sekwencja = await _context.Sequences.FindAsync(id);
            if (sekwencja == null)
            {
                return false;
            }
            _context.Sequences.Remove(sekwencja);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
