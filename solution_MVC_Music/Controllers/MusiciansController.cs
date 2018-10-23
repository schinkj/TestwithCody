using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using solution_MVC_Music.Data;
using solution_MVC_Music.Models;
using solution_MVC_Music.ViewModels;

namespace solution_MVC_Music.Controllers
{
    public class MusiciansController : Controller
    {
        private readonly MusicContext _context;

        public MusiciansController(MusicContext context)
        {
            _context = context;
        }

        // GET: Musicians
        public async Task<IActionResult> Index(int? InstrumentID, int? SongID, string SearchString, string sortDirection,  string actionButton, string sortField = "Musician")
        {
            //Suggestion to give the default SortField was made by Ken Dubois
            //That way you don't click the column heading and see nothing happen.
            ViewData["SongID"] = new SelectList(_context.Songs.OrderBy(c => c.Title), "ID", "Title");
            PopulateDropDownLists();
            ViewData["Filtering"] = "";  //Assume not filtering

            //Start with Includes
            var musicians = from m in _context.Musicians
                .Include(m => m.Instrument)
                .Include(m=>m.Plays).ThenInclude(p=>p.Instrument)
                .Include(m => m.Performances).ThenInclude(p => p.Song)
                select m;

            //Add as many filters as needed
            if (InstrumentID.HasValue)
            {
                musicians = musicians.Where(p => p.InstrumentID == InstrumentID);
                ViewData["Filtering"] = " in";
            }
            if (SongID.HasValue)
            {
                musicians = musicians.Where(p => p.Performances.Any(c => c.SongID == SongID));
                ViewData["Filtering"] = " in";
            }
            if (!String.IsNullOrEmpty(SearchString))
            {
                musicians = musicians.Where(p => p.LastName.ToUpper().Contains(SearchString.ToUpper())
                                       || p.FirstName.ToUpper().Contains(SearchString.ToUpper()));
                ViewData["Filtering"] = " in";
            }
            //Before we sort, see if we have called for a change of filtering or sorting
            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted so lets sort!
            {
                if (actionButton != "Filter")//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = String.IsNullOrEmpty(sortDirection) ? "desc" : "";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }
            //Now we know which field and direction to sort by, but a Switch is hard to use for 2 criteria
            //so we will use an if() structure instead.
            if (sortField == "Phone")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    musicians = musicians
                        .OrderBy(p => p.Phone);
                }
                else
                {
                    musicians = musicians
                        .OrderByDescending(p => p.Phone);
                }
            }
            else if (sortField == "Age")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    musicians = musicians
                        .OrderBy(p => p.DOB);
                }
                else
                {
                    musicians = musicians
                        .OrderByDescending(p => p.DOB);
                }
            }
            else if (sortField == "Primary Instrument")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    musicians = musicians
                        .OrderBy(p => p.Instrument.Name);
                }
                else
                {
                    musicians = musicians
                        .OrderByDescending(p => p.Instrument.Name);
                }
            }
            else //Sorting by Patient Name - Default Sorting
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    musicians = musicians
                        .OrderBy(p => p.LastName)
                        .ThenBy(p => p.FirstName);
                }
                else
                {
                    musicians = musicians
                        .OrderByDescending(p => p.LastName)
                        .ThenByDescending(p => p.FirstName);
                }
            }
            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            return View(await musicians.ToListAsync());
        }

        // GET: Musicians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.Musicians
                .Include(m => m.Instrument)
                .Include(m => m.Plays).ThenInclude(p => p.Instrument)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (musician == null)
            {
                return NotFound();
            }

            return View(musician);
        }

        // GET: Musicians/Create
        public IActionResult Create()
        {
            PopulateDropDownLists();
            var musician = new Musician();
            PopulateAssignedInstrumentData(musician);  
            return View();
        }

        // POST: Musicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,MiddleName,LastName,Phone,DOB,SIN,InstrumentID")] Musician musician, string[] selectedInstruments)
        {
            try
            {
                //Add the selected conditions
                if (selectedInstruments != null)
                {
                    musician.Plays = new List<Plays>();
                    foreach (var inst in selectedInstruments)
                    {
                        var instToAdd = new Plays { MusicianID = musician.ID, InstrumentID = int.Parse(inst) };
                        musician.Plays.Add(instToAdd);
                    }
                }
                if (ModelState.IsValid)
                {
                    _context.Add(musician);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException dex)
            {
                if (dex.InnerException.Message.Contains("IX_Musicians_SIN"))
                {
                    ModelState.AddModelError("", "Unable to save changes. Remember, you cannot have duplicate SIN numbers.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            PopulateAssignedInstrumentData(musician);
            PopulateDropDownLists(musician);
            return View(musician);
        }

        // GET: Musicians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.Musicians
                .Include(m => m.Instrument)
                .Include(m => m.Plays).ThenInclude(p => p.Instrument)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (musician == null)
            {
                return NotFound();
            }
            PopulateAssignedInstrumentData(musician);
            PopulateDropDownLists(musician);
            return View(musician);
        }

        // POST: Musicians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedInstruments)//, [Bind("ID,FirstName,MiddleName,LastName,Phone,DOB,SIN,InstrumentID")] Musician musician)
        {
            var musicianToUpdate = await _context.Musicians
                .Include(m => m.Instrument)
                .Include(m => m.Plays).ThenInclude(p => p.Instrument)
                .FirstOrDefaultAsync(m => m.ID == id);
            //Check that you got it or exit with a not found error
            if (musicianToUpdate == null)
            {
                return NotFound();
            }
            //Update the instruments palyed
            UpdateMusicianInstruments(selectedInstruments, musicianToUpdate);

            //Try updating it with the values posted
            if (await TryUpdateModelAsync<Musician>(musicianToUpdate, "",
                p => p.SIN, p => p.FirstName, p => p.MiddleName, p => p.LastName, p => p.DOB,
                p => p.Phone, p => p.InstrumentID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicianExists(musicianToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException dex)
                {
                    if (dex.InnerException.Message.Contains("IX_Musicians_SIN"))
                    {
                        ModelState.AddModelError("", "Unable to save changes. Remember, you cannot have duplicate SIN numbers.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }
            PopulateAssignedInstrumentData(musicianToUpdate);
            PopulateDropDownLists(musicianToUpdate);
            return View(musicianToUpdate);
        }

        // GET: Musicians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.Musicians
                .Include(m => m.Instrument)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (musician == null)
            {
                return NotFound();
            }

            return View(musician);
        }

        // POST: Musicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musician = await _context.Musicians
                .Include(m => m.Instrument)
                .FirstOrDefaultAsync(m => m.ID == id);
            try
            {
                _context.Musicians.Remove(musician);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dex)
            {
                if (dex.InnerException.Message.Contains("FK_Performances_Musicians_MusicianID"))
                {
                    ModelState.AddModelError("", "Unable to save changes. You cannot delete a Musician who performed on any songs.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(musician);
        }

        //This is a twist on the PopulateDropDownLists approach
        //  Create methods that return each SelectList separately
        //  and one method to put them all into ViewData.
        //This approach allows for AJAX requests to refresh
        //DDL Data at a later date.
        private SelectList InstrumentSelectList(int? id)
        {
            var dQuery = from i in _context.Instruments
                         orderby i.Name
                         select i;
            return new SelectList(dQuery, "ID", "Name", id);
        }
        private void PopulateDropDownLists(Musician musician = null)
        {
            ViewData["InstrumentID"] = InstrumentSelectList(musician?.InstrumentID);
        }

        private void PopulateAssignedInstrumentData(Musician musician)
        {
            var allInstruments = _context.Instruments;
            var mInstruments = new HashSet<int>(musician.Plays.Select(b => b.InstrumentID));
            var viewModel = new List<PlaysVM>();
            foreach (var inst in allInstruments)
            {
                viewModel.Add(new PlaysVM
                {
                    InstrumentID = inst.ID,
                    InstrumentName = inst.Name,
                    Assigned = mInstruments.Contains(inst.ID)
                });
            }
            ViewData["Instruments"] = viewModel;
        }

        private void UpdateMusicianInstruments(string[] selectedInstruments, Musician musicianToUpdate)
        {
            if (selectedInstruments == null)
            {
                musicianToUpdate.Plays = new List<Plays>();
                return;
            }

            var selectedInstrumentsHS = new HashSet<string>(selectedInstruments);
            var musicianInsts = new HashSet<int>
                (musicianToUpdate.Plays.Select(c => c.InstrumentID));//IDs of the currently selected insruments
            foreach (var inst in _context.Instruments)
            {
                if (selectedInstrumentsHS.Contains(inst.ID.ToString()))
                {
                    if (!musicianInsts.Contains(inst.ID))
                    {
                        musicianToUpdate.Plays.Add(new Plays { MusicianID = musicianToUpdate.ID, InstrumentID = inst.ID });
                    }
                }
                else
                {
                    if (musicianInsts.Contains(inst.ID))
                    {
                        Plays instrumentToRemove = musicianToUpdate.Plays.SingleOrDefault(c => c.InstrumentID == inst.ID);
                        _context.Remove(instrumentToRemove);
                    }
                }
            }
        }

        private bool MusicianExists(int id)
        {
            return _context.Musicians.Any(e => e.ID == id);
        }
    }
}
