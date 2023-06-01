using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using back_courrier.Data;
using back_courrier.Models;
using back_courrier.Services;
using back_courrier.Helper;
using System.Net.NetworkInformation;

namespace back_courrier.Pages
{
    public class DetailsVueCourrierModel : PageModel
    {
        private readonly back_courrier.Data.ApplicationDbContext _context;

        public DetailsVueCourrierModel(back_courrier.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public VueListeCourrier VueListeCourrier { get; set; } = default!;
        public List<Utilisateur> Prochain { get; set; } = default!;
        [BindProperty]
        public Historique Historique { get; set; } = default!;

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
                var vuelistecourrier = await _context.VueListeCourrier.FirstOrDefaultAsync(m => m.IdCourrierDestinataire == id);
                if (vuelistecourrier == null)
                {
                    return NotFound();
                }
                else
                {
                    VueListeCourrier = vuelistecourrier;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id, int idDepartement, int idStatut)
        {
            if (_context.Historique == null || Historique == null || Prochain == null)
            {
                await OnGetAsync(id, idDepartement, idStatut);
                return Page();
            }
            try {
                _context.Historique.Add(Historique);
                await _context.SaveChangesAsync(); // Save changes to generate the Id for the Courrier entity
                return RedirectToPage("./ListeCourrier");
            } catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                await OnGetAsync(id, idDepartement, idStatut);
                return Page();
            }
        }
    }
}
