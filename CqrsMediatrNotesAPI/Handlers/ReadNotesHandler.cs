using CqrsMediatrNotesAPI.Interfaces;
using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Queries;
using MediatR;

namespace CqrsMediatrNotesAPI.Handlers
{
    public class GetNotesHandler : IRequestHandler<GetAllNotesQuery, IEnumerable<Note>?>
    {
        private readonly IReadNotesRepository _readRepo;
        public GetNotesHandler(IReadNotesRepository readRepo) => _readRepo = readRepo;
        public async Task<IEnumerable<Note>?> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
        {
            return await _readRepo.GetAllNotes();
        }
    }

    public class GetNoteByIdHandler : IRequestHandler<GetNoteByIdQuery, Note?>
    {
        private readonly IReadNotesRepository _readRepo;
        public GetNoteByIdHandler(IReadNotesRepository readRepo) => _readRepo = readRepo;
        public async Task<Note?> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            return await _readRepo.GetNoteById(request.Id);
        }
    }
}
