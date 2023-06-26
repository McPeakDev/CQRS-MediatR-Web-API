using CqrsMediatrNotesAPI.Handlers;
using CqrsMediatrNotesAPI.Interfaces;
using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Queries;
using FluentAssertions;
using Moq;

namespace CQRSMediatRWebAPITest.Queries
{
    public class ReadNotesHandlerTest {

        private Mock<IReadNotesRepository> _readRepo;
        private IEnumerable<Note>? _notes;
        public ReadNotesHandlerTest()
        {
            _readRepo = new Mock<IReadNotesRepository>();
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

            _readRepo
                .SetupSequence(r => r.GetAllNotes())
                .Returns(Task.FromResult<IEnumerable<Note>?>(_notes))
                .Returns(Task.FromResult<IEnumerable<Note>?>(null));

            _readRepo
                .Setup(r => r.GetNoteById(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(_notes.As<List<Note>>().Find(n => n.Id == id)));
        }

        [Fact]
        public async Task GetNotes_Should_Return_As_An_Array_Of_Notes_Or_Null()
        {
            var command = new GetAllNotesQuery();
            var handler = new GetNotesHandler(_readRepo.Object);

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().NotBeNull();
            result.Should().AllBeOfType<Note>();

            result = await handler.Handle(command, new CancellationToken());

            result.Should().BeNull();
        }


        [Fact]
        public async Task GetNotesByID_With_Valid_ID_Should_Return_A_Note()
        {
            var command = new GetNoteByIdQuery(1);
            var handler = new GetNoteByIdHandler(_readRepo.Object);

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().NotBeNull();
            result.Should().BeOfType<Note>();
            result.Should().BeEquivalentTo(_notes!.First());

        }

        [Fact]
        public async Task GetNotesByID_With_Invalid_ID_Should_Not_Return_A_Note()
        {

            var command = new GetNoteByIdQuery(3);
            var handler = new GetNoteByIdHandler(_readRepo.Object);

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeNull();
        }
    }
}
