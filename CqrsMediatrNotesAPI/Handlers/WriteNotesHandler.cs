using CqrsMediatrNotesAPI.Commands;
using CqrsMediatrNotesAPI.Interfaces;
using CqrsMediatrNotesAPI.Models;
using MediatR;

namespace CqrsMediatrNotesAPI.Handlers
{
    public class AddNoteHandler : IRequestHandler<AddNoteCommand, bool> {
        private readonly IWriteRepository<Note> _writeRepo;

        public AddNoteHandler(IWriteRepository<Note> writeRepo){
            _writeRepo = writeRepo;
        }

        public async Task<bool> Handle(AddNoteCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_writeRepo.Add(request.Note));
        }
    }

    public class UpdateNoteHandler : IRequestHandler<UpdateNoteCommand, bool>
    {
        private readonly IWriteRepository<Note> _writeRepo;

        public UpdateNoteHandler(IWriteRepository<Note> writeRepo) {
            _writeRepo = writeRepo;
        }

        public async Task<bool> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            return await _writeRepo.Update(request.Note, n => n.Id);
        }
    }

    public class DeleteNoteHandler : IRequestHandler<DeleteNoteCommand, bool>
    {
        private readonly IWriteRepository<Note> _writeRepo;

        public DeleteNoteHandler(IWriteRepository<Note> writeRepo) {
            _writeRepo = writeRepo;
        }

        public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            return await _writeRepo.Delete(request.Id, n => n.Id);
        }
    }
}
