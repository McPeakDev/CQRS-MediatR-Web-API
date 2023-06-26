using CqrsMediatrNotesAPI.Commands;
using CqrsMediatrNotesAPI.Handlers;
using CqrsMediatrNotesAPI.Interfaces;
using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Queries;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Moq;

namespace CQRSMediatRWebAPITest.Queries
{
    public class WriteNotesHandlerTest {

        private Mock<IWriteNotesRepository> _writeRepo;
        private IEnumerable<Note>? _notes;
        public WriteNotesHandlerTest()
        {
            _writeRepo = new Mock<IWriteNotesRepository>();
            _notes = new List<Note> {
                new Note {
                    Id = 1,
                    Title= "Note #1",
                    Body = "Body #1"
                },
                new Note {
                    Id = 2,
                    Title= "Note #2",
                    Body = "Body #2"
                }
            };

            _writeRepo
                .Setup(r => r.AddNote(It.IsAny<Note>()))
                .Callback((Note note) => {
                    _notes.As<List<Note>>().Add(note);
                })
                .Returns(true);

            _writeRepo
                .Setup(r => r.UpdateNote(It.IsAny<Note>()))
                .Callback((Note note) => {
                    var index = _notes.As<List<Note>>().FindIndex(n => n.Id == note.Id);
                    if (index != -1) {
                        _notes.As<List<Note>>()[index] = note;
                    } else {
                        _notes.As<List<Note>>().Add(note);
                    }
                })
                .Returns(Task.FromResult(true));

            _writeRepo
                .Setup(r => r.DeleteNote(It.IsAny<int>()))
                .Callback((int id) => {
                    var note = _notes.As<List<Note>>().Find(n => n.Id == id);
                    if (note != null) {
                        _notes.As<List<Note>>().Remove(note);
                    }
                })
                .Returns(Task.FromResult(true));
        }

        [Fact]
        public async Task AddNote_Should_Add_The_Note()
        {
            var note = new Note {
                Id = 3,
                Title = "Note #3",
                Body = "Note #3"
            };


            var command = new AddNoteCommand(note);
            var handler = new AddNoteHandler(_writeRepo.Object);

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeTrue();
            _notes.Should().Contain(note);
        }

        [Fact]
        public async Task UpdateNote_Should_Update_The_Note() {
            var note = new Note {
                Id = 2,
                Title = "Note #2",
                Body = "I'm updated."
            };

            var command = new UpdateNoteCommand(note);
            var handler = new UpdateNoteHandler(_writeRepo.Object);

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeTrue();
            _notes.Should().Contain(note);

        }

        [Fact]
        public async Task UpdateNote_Should_Add_The_Note() {
            var note = new Note {
                Id = 4,
                Title = "Note #4",
                Body = "Note #4."
            };

            var command = new UpdateNoteCommand(note);
            var handler = new UpdateNoteHandler(_writeRepo.Object);

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeTrue();
            _notes.Should().Contain(note);
        }

        [Fact]
        public async Task DeleteNote_Should_Delete_The_Note() {
            var id = 1;
            var note = _notes.As<List<Note>>().First(n => n.Id == id);

            var command = new DeleteNoteCommand(id);
            var handler = new DeleteNoteHandler(_writeRepo.Object);

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeTrue();
            _notes.Should().NotContain(note);

        }
    }
}
