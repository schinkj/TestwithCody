using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using solution_MVC_Music.Models;

namespace solution_MVC_Music.Data
{
    public static class MusicSeedData
    {
        public static void MakeSomeData(IServiceProvider serviceProvider)
        {
            using (var context = new MusicContext(
                serviceProvider.GetRequiredService<DbContextOptions<MusicContext>>()))
            {
                //This approach to seeding data uses int and string arrays with loops to
                //create the data using random values
                Random random = new Random();

                //Prepare some string arrays for building objects
                string[] firstNames = new string[] { "Fred", "Barney", "Wilma", "Betty", "Dave", "Tim", "Elton", "Paul",  };
                string[] lastsNames = new string[] { "Stovell", "Jones", "Bloggs", "Flintstone", "Rubble", "Brown", "John", "McCartney" };
                string[] genres = new string[] { "Classical", "Rock", "Pop", "Jazz", "Country", "Ambient", "Techno" };
                string[] instruments = new string[] { "Lead Guitar", "Base Guitar", "Drums", "Keyboards", "Vocals", "Harmonica", "Didgeridoo" };

                //Genre
                if (!context.Genres.Any())
                {
                    //loop through the array of Genre names
                    foreach(string g in genres)
                    {
                        Genre genre = new Genre()
                        {
                            Name=g
                        };
                        context.Genres.Add(genre);
                    }
                    context.SaveChanges();
                }

                //Note: we will use int arrays to hold valid Primary Key values we can 
                //randomly assingn as foreign keys
                //Create a collection of the primary keys of the Genres
                int[] genreIDs = context.Genres.Select(g => g.ID).ToArray();

                //Album
                if (!context.Albums.Any())
                {
                    context.Albums.AddRange(
                     new Album
                     {
                         Name = "Rocket Food",
                         YearProduced = "2000",
                         Price = 19.99m,
                         GenreID = genreIDs[random.Next(genreIDs.Count())]
                     },
                     new Album
                     {
                         Name = "Songs of the Sea",
                         YearProduced = "1999",
                         Price = 9.99m,
                         GenreID = genreIDs[random.Next(genreIDs.Count())]
                     },
                     new Album
                     {
                         Name = "The Horse",
                         YearProduced = "1929",
                         Price = 99.99m,
                         GenreID = genreIDs[random.Next(genreIDs.Count())]
                     },
                     new Album
                     {
                         Name = "Freedom",
                         YearProduced = "2012",
                         Price = 29.99m,
                         GenreID = genreIDs[random.Next(genreIDs.Count())]
                     });
                    context.SaveChanges();
                }

                //Create a collection of the primary keys of the Albums
                int[] albumIDs = context.Albums.Select(a => a.ID).ToArray();

                //Song
                if (!context.Songs.Any())
                {
                    //Double loop through the arrays of names 
                    //and build song title as you go
                    foreach (string f in firstNames)
                    {
                        foreach (string g in genres)
                        {
                            Song s = new Song()
                            {
                                Title = f + " " + g,//looks silly but gives unique names for the songs
                                GenreID = genreIDs[random.Next(genreIDs.Count())],
                                AlbumID = albumIDs[random.Next(albumIDs.Count())]
                            };
                            context.Songs.Add(s);
                        }
                    }
                    context.SaveChanges();
                }
                //Create a collection of the primary keys of the Songss
                int[] songIDs = context.Songs.Select(a => a.ID).ToArray();

                //Instrument
                if (!context.Instruments.Any())
                {
                    //loop through the array of Instrument names
                    foreach (string iname in instruments)
                    {
                        Instrument inst = new Instrument()
                        {
                             Name=iname
                        };
                        context.Instruments.Add(inst);
                    }
                    context.SaveChanges();
                }
                //Create a collection of the primary keys of the Instruments
                int[] instrumentIDs = context.Instruments.Select(a => a.ID).ToArray();

                //Musician
                if (!context.Musicians.Any())
                {
                    // Start birthdate for randomly produced employees 
                    // We will subtract a random number of days from today
                    DateTime startDOB = DateTime.Today;

                    //Double loop through the arrays of names 
                    //and build the Musician as we go
                    foreach (string f in firstNames)
                    {
                        foreach (string l in lastsNames)
                        {
                            Musician m = new Musician()
                            {
                                FirstName = f,
                                MiddleName = f.Substring(1,1).ToUpper(),//take second letter of first name
                                LastName = l,
                                SIN = random.Next(213214131, 989898989).ToString(),//Big enough int for required digits
                                //For the phone, needed one more digit than a random int can generate so
                                //concatenated 2 together as strings and then converted
                                Phone = Convert.ToInt64(random.Next(2,10).ToString() + random.Next(213214131, 989898989).ToString()),
                                InstrumentID = instrumentIDs[random.Next(instrumentIDs.Count())],
                                DOB = startDOB.AddDays(-random.Next(6500, 25000))
                            };
                            context.Musicians.Add(m);
                        }
                    }
                    context.SaveChanges();
                }
                //Create a collection of the primary keys of the Musicians
                int[] musicianIDs = context.Musicians.Select(a => a.ID).ToArray();

                //Performance
                //Add a few musicians as performers on each song
                if (!context.Performances.Any())
                {
                    //i loops through the primary keys of the songs
                    //j is just a counter so we add a few musicians to a song
                    //k lets us step through all musicians so we can make sure each gets used
                    int k = 0;//Start with the first Musician
                    foreach (int i in songIDs)
                    {
                        int howMany = random.Next(1, 7);//How many musicians on a song
                        howMany = (howMany > musicianIDs.Count()) ? 6 : howMany; //Don't try to assign more musicians then are in the system
                        for (int j = 1; j <= howMany; j++)
                        {
                            k = (k >= musicianIDs.Count()) ? 0 : k;
                            Performance p = new Performance()
                            {
                                SongID = i,
                                MusicianID = musicianIDs[k]
                            };
                            context.Performances.Add(p);
                            k++;
                        }
                    }
                    context.SaveChanges();
                }
                //Plays
                //Add a few instruments to each musician
                if (!context.Plays.Any())
                {
                    //i loops through the primary keys of the musicians
                    //j is just a counter so we add a few instruments to a musician
                    //k lets us step through all instruments so we can make sure each gets used
                    int k = 0;//Start with the first instrument
                    foreach (int i in musicianIDs)
                    {
                        int howMany = random.Next(5);//add a few instruments to a musician
                        howMany = (howMany > instrumentIDs.Count()) ? 4 : howMany; //Don't try to assign more instruments then are in the system
                        for (int j = 1; j <= howMany; j++)
                        {
                            k = (k >= instrumentIDs.Count()) ? 0 : k;
                            Plays p = new Plays()
                            {
                                MusicianID  = i,
                                InstrumentID = instrumentIDs[k]
                            };
                            context.Plays.Add(p);
                            k++;
                        }
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
