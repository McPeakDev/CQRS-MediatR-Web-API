using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using CqrsMediatrNotesAPI.Interfaces;

namespace CqrsMediatrNotesAPI.Repositories
{
    public class ReadRepository<T>: IReadRepository<T> where T: class {

        private readonly DbSet<T> _dataSet;

        public ReadRepository(Context<Read, T> db) {
            _dataSet = db.Set<T>();
        }

        public async Task<T?> FindById(int id, Func<T, int> predicate) => (await _dataSet.ToListAsync()).Find(n => predicate(n) == id);

        public async Task<IEnumerable<T>?> GetAllNotes() => await Task.FromResult(_dataSet.Any() ? _dataSet : null);
    }
}
