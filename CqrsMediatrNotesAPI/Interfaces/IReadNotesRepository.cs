using CqrsMediatrNotesAPI.Models;

namespace CqrsMediatrNotesAPI.Interfaces {
    public interface IReadNotesRepository {
        Task<Note?> GetNoteById(int id);
        Task<IEnumerable<Note>?> GetAllNotes();
    }
}
