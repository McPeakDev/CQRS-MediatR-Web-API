using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using CqrsMediatrNotesAPI.Interfaces;

namespace CqrsMediatrNotesAPI.Repositories
{
    public class WriteNotesRepository: IWriteNotesRepository {

        private readonly WriteNotesContext _db;
        private readonly DbSet<Note> _notes;

        private async Task<Note?> GetNoteById(int id) => await _notes.FirstOrDefaultAsync(p => p.Id == id);

        public WriteNotesRepository(WriteNotesContext db) {
            _db = db;
            _notes = db.Set<Note>();
        }

        public bool AddNote(Note note)
        {
            _notes.Add(note);

            _db.SaveChanges();
            
            return true;
        }

        public async Task<bool> UpdateNote(Note note)
        {
            var found = await GetNoteById(note.Id);

            if (found != null)
            {
                _notes.Update(note);
            }
            else
            {
                _notes.Add(note);
            }

            _db.SaveChanges();

            return found != null;
        }

        public async Task<bool> DeleteNote(int id)
        {
            var found = await GetNoteById(id);

            if (found != null)
            {
                _notes.Remove(found);
            }

            _db.SaveChanges();

            return found != null;
        }
    }
}
