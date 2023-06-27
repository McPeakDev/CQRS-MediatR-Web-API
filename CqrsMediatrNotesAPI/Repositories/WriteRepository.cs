using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using CqrsMediatrNotesAPI.Interfaces;
using System.Linq.Expressions;

namespace CqrsMediatrNotesAPI.Repositories
{
    public class WriteRepository<T>: IWriteRepository<T> where T: class {

        private readonly Context<Write, T> _db;
        private readonly DbSet<T> _dataSet;
        private readonly TextWriter _errorWriter = Console.Error;

        private async Task<T?> FindByID(int id, Func<T, int> predicate) => (await _dataSet.ToListAsync()).Find(n => predicate(n) == id);

        public WriteRepository(Context<Write, T> db) {
            _db = db;
            _dataSet = db.Set<T>();
        }

        public bool Add(T data)
        {
            var changeCount = -1;

            try {
                _dataSet.Add(data);

                changeCount = _db.SaveChanges();
            } catch (Exception ex) {
                //Could add in Logging to a file here.
                _errorWriter.WriteLine(ex.ToString());
            }

            return changeCount != -1;
        }

        public async Task<bool> Update(T data, Func<T, int> findPredicate)
        {
            var changeCount = -1;
            var id = findPredicate.Invoke(data);

            try {
                var found = await FindByID(id, findPredicate);

                if (found != null)
                {
                    _dataSet.Update(data);
                }
                else
                {
                    _dataSet.Add(data);
                }

                changeCount = _db.SaveChanges();
            } catch (Exception ex) {
                //Could add in Logging to a file here.
                _errorWriter.WriteLine(ex.ToString());
            }

            return changeCount != -1;
        }

        public async Task<bool> Delete(int id, Func<T, int> findPredicate)
        {
            var changeCount = -1;

            try {
                var found = await FindByID(id, findPredicate);

                if (found != null) {
                    _dataSet.Remove(found);
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
