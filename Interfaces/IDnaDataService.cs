using DNA_Analyser.DTOs;
using DNA_Analyser.Entities;
using Microsoft.AspNetCore.Mvc;
//rejestracja serwisu w Program.cs
namespace DNA_Analyser.Interfaces
{

    //tutaj operacje get, update, delete
    public interface IDnaDataService
    {
        Task<List<DnaSequence>> GetAllSequences();
        Task<DnaSequence> GetSequenceById(int id);
        Task<List<DnaSequence>> CreateSequenceInstance(CreateDnaSequenceDTO request);
        Task<List<DnaSequence>> UpdateSequenceById(int id, UpdateDnaSequenceDTO request);
        Task<bool> DeleteSequence(int id);


    }
}
