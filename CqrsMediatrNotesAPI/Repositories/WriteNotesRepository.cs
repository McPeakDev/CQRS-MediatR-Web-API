using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using CqrsMediatrNotesAPI.Interfaces;

namespace CqrsMediatrNotesAPI.Repositories
{
    public class WriteNotesRepository: IWriteNotesRepository {

        private readonly WriteNotesContext _db;
        private readonly DbSet<Note> _notes;
        private readonly TextWriter _errorWriter = Console.Error;

        private async Task<Note?> GetNoteById(int id) => await _notes.FirstOrDefaultAsync(p => p.Id == id);

        public WriteNotesRepository(WriteNotesContext db) {
            _db = db;
            _notes = db.Set<Note>();
        }

        public bool AddNote(Note note)
        {
            var changeCount = -1;

            try {
                _notes.Add(note);

                changeCount = _db.SaveChanges();
            } catch (Exception ex) {
                //Could add in Logging to a file here.
                _errorWriter.WriteLine(ex.ToString());
            }

            return changeCount != -1;
        }

        public async Task<bool> UpdateNote(Note note)
        {
            var changeCount = -1;

            try {
                var found = await GetNoteById(note.Id);

                if (found != null)
                {
                    _notes.Update(note);
                }
                else
                {
                    _notes.Add(note);
                }

                changeCount = _db.SaveChanges();
            } catch (Exception ex) {
                //Could add in Logging to a file here.
                _errorWriter.WriteLine(ex.ToString());
            }

            return changeCount != -1;
        }

        public async Task<bool> DeleteNote(int id)
        {
            var changeCount = -1;

            try {
                var found = await GetNoteById(id);

                if (found != null) {
                    _notes.Remove(found);
                }

                changeCount = _db.SaveChanges();
            } catch (Exception ex) {
                //Could add in Logging to a file here.
                _errorWriter.WriteLine(ex.ToString());
            }

            return changeCount != -1;
        }
    }
}
