using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solution_MVC_Music.Models
{
    public class Plays
    {
        public int InstrumentID { get; set; }
        public Instrument Instrument { get; set; }

        public int MusicianID { get; set; }
        public Musician Musician { get; set; }

    }
}
