using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace solution_MVC_Music.Models
{
    public class Performance
    {
        public int SongID { get; set; }
        public Song Song { get; set; }

        public int MusicianID { get; set; }
        public Musician Musician { get; set; }
    }
}
