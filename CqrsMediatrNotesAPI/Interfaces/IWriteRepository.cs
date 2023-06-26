namespace CqrsMediatrNotesAPI.Interfaces {
    public interface IWriteRepository<T> {
        bool Add(T data);
        Task<bool> Update(T data, Func<T, int> findPredicate);
        Task<bool> Delete(int id, Func<T, int> findPredicate);
    }
}
