using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using back_courrier.Data;
using back_courrier.Models;
using back_courrier.Helper;
using back_courrier.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Net.NetworkInformation;

namespace back_courrier.Pages
{
    public class HistoriqueModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourrierService _courrierService;
        private readonly IUtilisateurService _employeService;
        public List<Utilisateur> Prochain;
        private Utilisateur _currentUser;
        [BindProperty]
        public Historique Historique { get; set; } = default!;
        public HistoriqueModel(ApplicationDbContext context, ICourrierService courrierService,
            IUtilisateurService employeService, IConfiguration configuration)
        {
            _context = context;
            this._courrierService = courrierService;
            this._employeService = employeService;
        }

        public async Task<IActionResult> OnGetAsync(int id, int idDepartement, int idStatut)
        {
            _currentUser = _employeService.GetUtilisateurByClaim(User);
            _currentUser.Poste = _context.Poste.FirstOrDefault(p => p.Id == _currentUser.IdPoste);
            Prochain = _employeService.GetUtilisateurSuivant(_currentUser, idDepartement, idStatut);
            if(Prochain != null) { 
                ViewData["Prochain"] = new SelectList(Prochain, "Id", "Nom");
            }
            Historique = _courrierService.GetHistoriqueByIdCourrierDestinataire(id);
            if (Historique == null)
            {
                return RedirectToPage("./ListeCourrier");
            }
            return Page();
        }



        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int id, int idDepartement, int idStatut)
        {
            if (!ModelState.IsValid || _context.Historique == null || Historique == null)
            {
                await OnGetAsync(id, idDepartement, idStatut);
                return Page();
            }
            try
            {
                _courrierService.TransfertCourrier(Historique);
                await _context.SaveChangesAsync();

                return RedirectToPage("./ListeCourrier");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                await OnGetAsync(id, idDepartement, idStatut);
                return Page();
            }
        }
    }
}
