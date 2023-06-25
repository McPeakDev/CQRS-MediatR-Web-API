using CqrsMediatrNotesAPI.Commands;
using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CqrsMediatrNotesAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase {

        private readonly IMediator _mediator;
        public NotesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult> GetNotes() {
            var notes = await _mediator.Send(new GetAllNotesQuery());
            return Ok(notes);
        }

        [HttpGet("{id:int}", Name = "GetNoteById")]
        public async Task<ActionResult> GetNoteById(int id) {
            var note = await _mediator.Send(new GetNoteByIdQuery(id));
            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult> AddNote([FromBody] Note note) {
            await _mediator.Send(new AddNoteCommand(note));
            return StatusCode(201);
        }

        public async Task<ActionResult> UpdateNote([FromBody] Note note) {
            var result = await _mediator.Send(new UpdateNoteCommand(note));
            return result ? StatusCode(204) : StatusCode(201);
        }

        [HttpDelete("{id:int}", Name = "DeleteNote")]
        public async Task<ActionResult> DeleteNote (int id) {
            var result = await _mediator.Send(new DeleteNoteCommand(id));
            return result ? StatusCode(204) : StatusCode(404);
        }
    }
}
