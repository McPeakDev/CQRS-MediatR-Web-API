using CqrsMediatrNotesAPI.Commands;
using CqrsMediatrNotesAPI.Handlers;
using CqrsMediatrNotesAPI.Interfaces;
using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Queries;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Moq;
using System;
using System.Linq.Expressions;

namespace CQRSMediatRWebAPITest.Queries
{
    public class WriteNotesHandlerTest {

        private readonly Mock<IWriteRepository<Note>> _writeRepo;
        private readonly IEnumerable<Note>? _notes;
        public WriteNotesHandlerTest()
        {
            _writeRepo = new Mock<IWriteRepository<Note>>();
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
                .Setup(r => r.Add(It.IsAny<Note>()))
                .Callback((Note note) => {
                    _notes.As<List<Note>>().Add(note);
                })
                .Returns(true);

            _writeRepo
                .Setup(r => r.Update(It.IsAny<Note>(), It.IsAny<Func<Note, int>>()))
                .Callback((Note note, Func<Note, int> predicate) => {
                    var index = _notes.As<List<Note>>().FindIndex(n => predicate(n) == note.Id);
                    if (index != -1) {
                        _notes.As<List<Note>>()[index] = note;
                    } else {
                        _notes.As<List<Note>>().Add(note);
                    }
                })
                .Returns(Task.FromResult(true));

            _writeRepo
                .Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<Func<Note, int>>()))
                .Callback((int id, Func<Note, int> predicate) => {
                    var note = _notes.As<List<Note>>().Find(n => predicate(n) == id);
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
