namespace CqrsMediatrNotesAPI.Interfaces {
    public interface IReadRepository<T> {
        Task<T?> FindById(int id, Func<T, int> predicate);
        Task<IEnumerable<T>?> GetAllNotes();
    }
}
