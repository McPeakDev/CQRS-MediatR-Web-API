using CqrsMediatrNotesAPI.Commands;
using CqrsMediatrNotesAPI.Interfaces;
using MediatR;

namespace CqrsMediatrNotesAPI.Handlers
{
    public class AddNoteHandler : IRequestHandler<AddNoteCommand, bool> {
        private readonly IWriteNotesRepository _writeRepo;

        public AddNoteHandler(IWriteNotesRepository writeRepo){
            _writeRepo = writeRepo;
        }

        public async Task<bool> Handle(AddNoteCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_writeRepo.AddNote(request.Note));
        }
    }

    public class UpdateNoteHandler : IRequestHandler<UpdateNoteCommand, bool>
    {
        private readonly IWriteNotesRepository _writeRepo;

        public UpdateNoteHandler(IWriteNotesRepository writeRepo) {
            _writeRepo = writeRepo;
        }

        public async Task<bool> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            return await _writeRepo.UpdateNote(request.Note);
        }
    }

    public class DeleteNoteHandler : IRequestHandler<DeleteNoteCommand, bool>
    {
        private readonly IWriteNotesRepository _writeRepo;

        public DeleteNoteHandler(IWriteNotesRepository writeRepo) {
            _writeRepo = writeRepo;
        }

        public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            return await _writeRepo.DeleteNote(request.Id);
        }
    }
}
