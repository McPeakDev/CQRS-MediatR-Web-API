using CqrsMediatrNotesAPI.Models;
using MediatR;

namespace CqrsMediatrNotesAPI.Commands {
    public record AddNoteCommand(Note Note) : IRequest<bool>;

    public record UpdateNoteCommand(Note Note) : IRequest<bool>;

    public record DeleteNoteCommand(int Id) : IRequest<bool>;

}
