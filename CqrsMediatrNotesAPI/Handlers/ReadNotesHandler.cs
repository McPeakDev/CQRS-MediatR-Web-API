using CqrsMediatrNotesAPI.Interfaces;
using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Queries;
using MediatR;

namespace CqrsMediatrNotesAPI.Handlers
{
    public class GetNotesHandler : IRequestHandler<GetAllNotesQuery, IEnumerable<Note>?>
    {
        private readonly IReadRepository<Note> _readRepo;
        public GetNotesHandler(IReadRepository<Note> readRepo) => _readRepo = readRepo;
        public async Task<IEnumerable<Note>?> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
        {
            return await _readRepo.GetAllNotes();
        }
    }

    public class GetNoteByIdHandler : IRequestHandler<GetNoteByIdQuery, Note?>
    {
        private readonly IReadRepository<Note> _readRepo;
        public GetNoteByIdHandler(IReadRepository<Note> readRepo) => _readRepo = readRepo;
        public async Task<Note?> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            return await _readRepo.FindById(request.Id, n => n.Id);
        }
    }
}
