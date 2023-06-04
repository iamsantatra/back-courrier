using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using back_courrier.Data;
using back_courrier.Models;
using back_courrier.Helper;
using back_courrier.Services;
using Microsoft.EntityFrameworkCore;

namespace back_courrier.Pages
{
    public class HistoriqueModel : PageModel
    {
        private readonly back_courrier.Data.ApplicationDbContext _context;

        public HistoriqueModel(back_courrier.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Utilisateur> Prochain { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int id, int idDepartement, int idStatut)
        {
            if (id == null || _context.VueListeCourrier == null)
            {
                return NotFound();
            }
            try
            {
                Utilisateur UtilisateurConn = HttpContext.Session.GetObject<Utilisateur>("utilisateur");
                Prochain = TransfertService.GetProchainUtilisateur(_context, UtilisateurConn, idDepartement, idStatut);
                Historique historique = await _context.Historique.Where(h => h.IdCourrierDestinataire == id).OrderByDescending(h => h.DateHistorique).FirstOrDefaultAsync();
                if (historique == null)
                {
                    return NotFound();
                }
                Historique = historique;
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
            return Page();
        }
        [BindProperty]
        public Historique Historique { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Historique == null || Historique == null)
            {
                return Page();
            }
            Historique.IdStatut = Historique.IdStatut + 1;
            _context.Historique.Add(Historique);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ListeCourrier");
        }
    }
}
