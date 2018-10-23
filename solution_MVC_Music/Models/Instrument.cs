using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace solution_MVC_Music.Models
{
    public class Instrument
    {
        public Instrument()
        {
            Musicians = new HashSet<Musician>();
            Plays = new HashSet<Plays>();
        }

        public int ID { get; set; }

        [Display(Name = "Instrument")]
        [Required(ErrorMessage = "You cannot leave the name of the Instrument blank.")]
        [StringLength(50, ErrorMessage = "Instrument name cannot be more than 50 characters long.")]
        public string Name { get; set; }

        public ICollection<Musician> Musicians { get; set; }
        public ICollection<Plays> Plays { get; set; }
    }
}
