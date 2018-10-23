using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solution_MVC_Music.ViewModels
{
    public class PlaysVM
    {
        public int InstrumentID { get; set; }
        public string InstrumentName { get; set; }
        public bool Assigned { get; set; }
    }
}
