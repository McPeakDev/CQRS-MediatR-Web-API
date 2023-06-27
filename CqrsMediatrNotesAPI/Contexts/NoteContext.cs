using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using CqrsMediatrNotesAPI.Models;
using Microsoft.Extensions.Configuration;

namespace CqrsMediatrNotesAPI.Contexts {

    public record Write;
    public record Read;

    public class Context<T, To> : DbContext where To : class {
        public DbSet<Note> Notes { get; set; }
        public Context(DbContextOptions<Context<T, To>> options) : base(options) { }
    };
}
