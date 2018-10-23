using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using solution_MVC_Music.Data;
using solution_MVC_Music.Models;

namespace solution_MVC_Music.Controllers
{
    public class GenresController : Controller
    {
        private readonly MusicContext _context;

        public GenresController(MusicContext context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            return View(await _context.Genres.ToListAsync());
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Genre genre)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(genre);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            //Go get the Genre to update
            var genreToUpdate = await _context.Genres.FindAsync(id);

            //Check that you got it or exit with a not found error
            if (genreToUpdate == null)
            {
                return NotFound();
            }

            //Try updating it with the values posted
            if (await TryUpdateModelAsync<Genre>(genreToUpdate, "",
                d => d.Name))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genreToUpdate.ID))
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
            //Validaiton Error so give the user another chance.
            return View(genreToUpdate);

        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Go get the Genre to update
            var genre = await _context.Genres
                //.Include(g => g.Albums) //Uncomment these to check for chldren
                //.Include(g => g.Songs)  //before you try to delete
                .SingleOrDefaultAsync(p => p.ID == id);

            //Uncomment these to check for chldren before you try to delete
            //bool tryToDelete = true;
            //int albumCount = genre.Albums.Count();
            //int songCount = genre.Songs.Count();
            //if (albumCount > 0)
            //{
            //    ModelState.AddModelError("", "Unable to save changes. You cannot delete a Genre assigned to an Album.  Number of Albums: " + albumCount.ToString());
            //    tryToDelete = false;
            //}
            //if (songCount > 0)
            //{
            //    ModelState.AddModelError("", "Unable to save changes. You cannot delete a Genre assigned to a Song.  Number of Songs: " + songCount.ToString());
            //    tryToDelete = false;
            //}
            //if (tryToDelete)
            //{
            try
            {
                    _context.Genres.Remove(genre);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dex)
                {
                    if (dex.InnerException.Message.Contains("FK_Albums_Genres_GenreID"))
                    {
                        ModelState.AddModelError("", "Unable to save changes. You cannot delete a Genre assigned to an Album.");
                    }
                    else if (dex.InnerException.Message.Contains("FK_Songs_Genres_GenreID"))
                    {
                        ModelState.AddModelError("", "Unable to save changes. You cannot delete a Genre assigned to a Song.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            //}

            return View(genre);

        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.ID == id);
        }
    }
}
