using CqrsMediatrNotesAPI.Contexts;
using CqrsMediatrNotesAPI.Models;
using MediatR;

namespace CqrsMediatrNotesAPI.Queries {

    public record GetAllNotesQuery() : IRequest<IEnumerable<Note>?>;

    public record GetNoteByIdQuery(int Id) : IRequest<Note?>;


}
