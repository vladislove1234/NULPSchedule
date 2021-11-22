using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using NULPSchedule.Models.Mocks;
using ShceduleParser.Models.Mocks;
using Xamarin.Essentials;

namespace NULPSchedule.Services
{
    public class LessonsDBContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        public LessonsDBContext()
        {
            SQLitePCL.Batteries_V2.Init();

            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Lessons.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
        }
    }
}
