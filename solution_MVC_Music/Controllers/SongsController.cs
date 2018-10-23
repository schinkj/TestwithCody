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
    public class SongsController : Controller
    {
        private readonly MusicContext _context;

        public SongsController(MusicContext context)
        {
            _context = context;
        }

        // GET: Songs
        public async Task<IActionResult> Index()
        {
            var musicContext = _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genre)
                .Include(s => s.Performances).ThenInclude(m => m.Musician);
            return View(await musicContext.ToListAsync());
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Album).ThenInclude(a=>a.Genre)
                .Include(s => s.Genre)
                //.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Songs/Create
        public IActionResult Create()
        {
            var s = new Song();
            PopulatePerformanceData(s);
            PopulateDropDownLists();
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,AlbumID,GenreID")] Song song, string[] selectedOptions)
        {
            try
            {
                UpdatePerformances(selectedOptions, song);
                if (ModelState.IsValid)
                {
                    _context.Add(song);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            PopulatePerformanceData(song);
            PopulateDropDownLists(song);
            return View(song);
        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genre)
                .Include(s=>s.Performances).ThenInclude(m=>m.Musician)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (song == null)
            {
                return NotFound();
            }
            PopulatePerformanceData(song);
            PopulateDropDownLists(song);
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedOptions)//, [Bind("ID,Title,AlbumID,GenreID")] Song song)
        {
            var songToUpdate = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genre)
                .Include(s => s.Performances).ThenInclude(m => m.Musician)
                .FirstOrDefaultAsync(s => s.ID == id);
            //Check that you got it or exit with a not found error
            if (songToUpdate == null)
            {
                return NotFound();
            }

            UpdatePerformances(selectedOptions, songToUpdate);

            //Try updating it with the values posted
            if (await TryUpdateModelAsync<Song>(songToUpdate, "",
                s => s.Title, s => s.AlbumID, s => s.GenreID))
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
                    if (!SongExists(songToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            PopulatePerformanceData(songToUpdate);
            PopulateDropDownLists(songToUpdate);
            return View(songToUpdate);
        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Album).ThenInclude(a => a.Genre)
                .Include(s => s.Genre)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genre)
                .FirstOrDefaultAsync(m => m.ID == id);
            try
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(song);
        }

        private void PopulatePerformanceData(Song song)
        {
            var allMusicians = _context.Musicians;
            var songMusicians = new HashSet<int>(song.Performances.Select(b => b.MusicianID));
            var selected = new List<MusicianVM>();
            var available = new List<MusicianVM>();
            foreach (var s in allMusicians)
            {
                if (songMusicians.Contains(s.ID))
                {
                    selected.Add(new MusicianVM
                    {
                        MusicianID = s.ID,
                        MusicianName = s.FormalName
                    });
                }
                else
                {
                    available.Add(new MusicianVM
                    {
                        MusicianID = s.ID,
                        MusicianName = s.FormalName
                    });
                }
            }

            ViewData["selOpts"] = new MultiSelectList(selected.OrderBy(s => s.MusicianName), "MusicianID", "MusicianName");
            ViewData["availOpts"] = new MultiSelectList(available.OrderBy(s => s.MusicianName), "MusicianID", "MusicianName");
        }

        private void UpdatePerformances(string[] selectedOptions, Song songToUpdate)
        {
            if (selectedOptions == null)
            {
                songToUpdate.Performances = new List<Performance>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var songMusicians = new HashSet<int>(songToUpdate.Performances.Select(b => b.MusicianID));
            foreach (var m in _context.Musicians)
            {
                if (selectedOptionsHS.Contains(m.ID.ToString()))
                {
                    if (!songMusicians.Contains(m.ID))
                    {
                        songToUpdate.Performances.Add(new Performance
                        {
                            MusicianID = m.ID,
                            SongID = songToUpdate.ID
                        });
                    }
                }
                else
                {
                    if (songMusicians.Contains(m.ID))
                    {
                        Performance performanceToRemove = songToUpdate.Performances.SingleOrDefault(d => d.MusicianID == m.ID);
                        _context.Remove(performanceToRemove);
                    }
                }
            }
        }

        //This is a twist on the PopulateDropDownLists approach
        //  Create methods that return each SelectList separately
        //  and one method to put them all into ViewData.
        //This approach allows for AJAX requests to refresh
        //DDL Data at a later date.
        private SelectList GenreSelectList(int? id)
        {
            var dQuery = from g in _context.Genres
                         orderby g.Name
                         select g;
            return new SelectList(dQuery, "ID", "Name", id);
        }
        private SelectList AlbumSelectList(int? id)
        {
            var dQuery = from a in _context.Albums.Include("Genre")
                         orderby a.Name, a.YearProduced
                         select a;
            return new SelectList(dQuery, "ID", "FullSummary", id);
        }
        private void PopulateDropDownLists(Song song = null)
        {
            ViewData["GenreID"] = GenreSelectList(song?.GenreID);
            ViewData["AlbumID"] = AlbumSelectList(song?.AlbumID);
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.ID == id);
        }
    }
}
