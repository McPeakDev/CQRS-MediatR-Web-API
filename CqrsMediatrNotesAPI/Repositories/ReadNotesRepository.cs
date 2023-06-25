using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using CqrsMediatrNotesAPI.Interfaces;

namespace CqrsMediatrNotesAPI.Repositories
{
    public class ReadNotesRepository: IReadNotesRepository {

        private readonly DbSet<Note> _notes;

        public ReadNotesRepository(ReadNotesContext db) {
            _notes = db.Set<Note>();
        }

        public async Task<Note?> GetNoteById(int id) => await _notes.FirstOrDefaultAsync<Note>(p => p.Id == id);

        public async Task<IEnumerable<Note>?> GetAllNotes() => await Task.FromResult(_notes.Any() ? _notes: null);
    }
}
