using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using CqrsMediatrNotesAPI.Models;
using Microsoft.Extensions.Configuration;

namespace CqrsMediatrNotesAPI.Contexts {
    public class NotesContext : DbContext {
        public DbSet<Note> Notes { get; set; }

        protected string DataSource { get; set; } = "notesRead.sqlite";

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DataSource}");
    }

    public class ReadNotesContext : NotesContext {};

    public class WriteNotesContext : NotesContext {
        public WriteNotesContext() {
            DataSource = "notesWrite.sqlite";
        }
    };
}
