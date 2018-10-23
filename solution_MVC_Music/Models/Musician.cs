using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace solution_MVC_Music.Models
{
    public class Musician : IValidatableObject
    {
        public Musician()
        {
            Performances = new HashSet<Performance>();
            Plays = new HashSet<Plays>();
        }

        public int ID { get; set; }

        [Display(Name = "Musician")]
        public string FullName
        {
            get
            {
                return FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }

        public string FormalName
        {
            get
            {
                return LastName + ", " + FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? "" :
                        (" " + (char?)MiddleName[0] + ".").ToUpper());
            }
        }

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int a = today.Year - DOB.Year
                    - ((today.Month < DOB.Month || (today.Month == DOB.Month && today.Day < DOB.Day) ? 1 : 0));
                return a; /*Note: You could add .PadLeft(3) but spaces disappear in a web page. */
            }
        }

        public string FormattedSIN
        {
            get
            {
                return SIN.Substring(0, 3) + "-" + SIN.Substring(3, 3) + "-" + SIN.Substring(6, 3);
            }
        }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(30, ErrorMessage = "First name cannot be more than 30 characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(30, ErrorMessage = "Middle name cannot be more than 30 characters long.")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(50, ErrorMessage = "Last name cannot be more than 50 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64 Phone { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }
        
        [Required(ErrorMessage = "You cannot leave the SIN blank.")]
        [RegularExpression("^\\d{9}$", ErrorMessage = "The SIN must be exactly 9 numeric digits.")]
        [StringLength(9)]//DS Note: we only include this to limit the size of the database field to 10
        public string SIN { get; set; }

        [Display(Name = "Principal Instrument")]
        [Range(1,int.MaxValue,ErrorMessage ="You must select the principal instrument the musician plays.")]
        public int InstrumentID { get; set; }
        public Instrument Instrument { get; set; }

        public ICollection<Performance> Performances { get; set; }
        public ICollection<Plays> Plays { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB > DateTime.Today)
            {
                yield return new ValidationResult("Date of Birth cannot be in the future.", new[] { "DOB" });
            }
            else if(Age<=5)//Why not make good use of the summary property we have available?
            {
                yield return new ValidationResult("Musician must be at least 5 years old.", new[] { "DOB" });
            }
        }
    }
}
