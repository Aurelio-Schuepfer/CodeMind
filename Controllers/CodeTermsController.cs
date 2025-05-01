using CodeMind.Data;
using CodeMind.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeMind.Controllers
{
    public class CodeTermsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CodeTermsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CodeTerms
        public async Task<IActionResult> Index()
        {
            return View(await _context.CodeTerm.ToListAsync());
        }

        // GET: CodeTerms/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // PoST: CodeTerms/SHowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View("Index", await _context.CodeTerm.Where(j => j.Term.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: CodeTerms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeTerm = await _context.CodeTerm
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codeTerm == null)
            {
                return NotFound();
            }

            return View(codeTerm);
        }

        // GET: CodeTerms/Create

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CodeTerms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Term,Meaning")] CodeTerm codeTerm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(codeTerm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(codeTerm);
        }

        // GET: CodeTerms/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeTerm = await _context.CodeTerm.FindAsync(id);
            if (codeTerm == null)
            {
                return NotFound();
            }
            return View(codeTerm);
        }

        // POST: CodeTerms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Term,Meaning")] CodeTerm codeTerm)
        {
            if (id != codeTerm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(codeTerm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodeTermExists(codeTerm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(codeTerm);
        }

        // GET: CodeTerms/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeTerm = await _context.CodeTerm
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codeTerm == null)
            {
                return NotFound();
            }

            return View(codeTerm);
        }

        // POST: CodeTerms/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var codeTerm = await _context.CodeTerm.FindAsync(id);
            if (codeTerm != null)
            {
                _context.CodeTerm.Remove(codeTerm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodeTermExists(int id)
        {
            return _context.CodeTerm.Any(e => e.Id == id);
        }
    }
}
