using CqrsMediatrNotesAPI.Models;

namespace CqrsMediatrNotesAPI.Interfaces {
    public interface IWriteNotesRepository {
        bool AddNote(Note note);
        Task<bool> UpdateNote(Note note);
        Task<bool> DeleteNote(int id);
    }
}
