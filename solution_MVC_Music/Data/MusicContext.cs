using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using solution_MVC_Music.Models;

namespace solution_MVC_Music.Data
{
    public class MusicContext : DbContext
    {
        public MusicContext (DbContextOptions<MusicContext> options)
            : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Musician> Musicians { get; set; }
        public DbSet<Plays> Plays { get; set; }
        public DbSet<Performance> Performances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("MUSIC");

            //Many to Many Primary Key
            modelBuilder.Entity<Performance>()
            .HasKey(p => new { p.MusicianID, p.SongID});

            //Many to Many Primary Key
            modelBuilder.Entity<Plays>()
            .HasKey(p => new { p.MusicianID, p.InstrumentID });

            //Add a unique index to the Musician SIN
            modelBuilder.Entity<Musician>()
            .HasIndex(p => p.SIN)
            .IsUnique();

            //Add a unique index to the Album 
            //Name and Year Produced
            modelBuilder.Entity<Album>()
            .HasIndex(a => new { a.Name, a.YearProduced})
            .IsUnique();

            //NOTE: EACH OF THE FOLLOWING DELETE RESTRICTIONS
            //      CAN BE WRITTEN TWO WAYS: 
            //          FROM THE PARENT TABLE PERSPECTIVE OR
            //          FROM THE CHILD TABLE PERSPECTIVE

            //Prevent Cascade Delete Genre to Album (Parent Perspective)
            modelBuilder.Entity<Genre>()
                .HasMany<Album>(p => p.Albums)
                .WithOne(c => c.Genre)
                .HasForeignKey(c => c.GenreID)
                .OnDelete(DeleteBehavior.Restrict);
            //Prevent Cascade Delete Genre to Album (Child Perspective)
            //modelBuilder.Entity<Album>()
            //    .HasOne(c => c.Genre)
            //    .WithMany(p => p.Albums)
            //    .HasForeignKey(c => c.GenreID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Genre to Song (Parent Perspective)
            modelBuilder.Entity<Genre>()
                .HasMany<Song>(p => p.Songs)
                .WithOne(c => c.Genre)
                .HasForeignKey(c => c.GenreID)
                .OnDelete(DeleteBehavior.Restrict);
            //Prevent Cascade Delete Genre to Song (Child Perspective)
            //modelBuilder.Entity<Song>()
            //    .HasOne(c => c.Genre)
            //    .WithMany(p => p.Songs)
            //    .HasForeignKey(c => c.GenreID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Album to Song (Parent Perspective)
            modelBuilder.Entity<Album>()
                .HasMany<Song>(p => p.Songs)
                .WithOne(c => c.Album)
                .HasForeignKey(c => c.AlbumID)
                .OnDelete(DeleteBehavior.Restrict);
            //Prevent Cascade Delete Album to Song (Child Perspective)
            //modelBuilder.Entity<Song>()
            //    .HasOne(c => c.Album)
            //    .WithMany(p => p.Songs)
            //    .HasForeignKey(c => c.AlbumID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Instrument to Musician (Parent Perspective)
            modelBuilder.Entity<Instrument>()
                .HasMany<Musician>(p => p.Musicians)
                .WithOne(c => c.Instrument)
                .HasForeignKey(c => c.InstrumentID)
                .OnDelete(DeleteBehavior.Restrict);
            //Prevent Cascade Delete Instrument to Musician (Child Perspective)
            //modelBuilder.Entity<Musician>()
            //    .HasOne(c => c.Instrument)
            //    .WithMany(p => p.Musicians)
            //    .HasForeignKey(c => c.InstrumentID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Instrument to Plays (Parent Perspective)
            modelBuilder.Entity<Instrument>()
                .HasMany<Plays>(p => p.Plays)
                .WithOne(c => c.Instrument)
                .HasForeignKey(c => c.InstrumentID)
                .OnDelete(DeleteBehavior.Restrict);
            //Prevent Cascade Delete Instrument to Plays (Child Perspective)
            //modelBuilder.Entity<Plays>()
            //    .HasOne(c => c.Instrument)
            //    .WithMany(p => p.Plays)
            //    .HasForeignKey(c => c.InstrumentID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Musician to Performance (Parent Perspective)
            modelBuilder.Entity<Musician>()
                .HasMany<Performance>(p => p.Performances)
                .WithOne(c => c.Musician)
                .HasForeignKey(c => c.MusicianID)
                .OnDelete(DeleteBehavior.Restrict);
            //Prevent Cascade Delete Musician to Performance (Child Perspective)
            //modelBuilder.Entity<Performance>()
            //    .HasOne(c => c.Musician)
            //    .WithMany(p => p.Performances)
            //    .HasForeignKey(c => c.MusicianID)
            //    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
