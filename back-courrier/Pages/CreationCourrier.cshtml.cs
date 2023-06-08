using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using back_courrier.Data;
using back_courrier.Models;
using Microsoft.AspNetCore.Authorization;
using back_courrier.Services;
using System.Security.Claims;

namespace back_courrier.Pages
{
    [Authorize(Roles = "REC")]
    public class CreationCourrierModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourrierService _courrierService;
        private readonly IUtilisateurService _employeService;
        private List<Departement> departements { get; set; }
        private List<Flag> flags { get; set; }
        public CreationCourrierModel(ApplicationDbContext context,
            ICourrierService courrierService,
            IUtilisateurService employeService)
        {
            _context = context;
            _courrierService = courrierService;
            _employeService = employeService;
            departements = _context.Departement.ToList();
            flags = _context.Flag.ToList();
        }

        public IActionResult OnGet()
        {
/*            var flags = new List<string> { "normal", "urgent", "important" };*/

            ViewData["IdExpediteur"] = new SelectList(_context.Departement, "Id", "Designation");
            ViewData["Flag"] = new SelectList(_context.Flag, "Id", "Designation");
            ViewData["IdRecepteur"] = new SelectList(_context.Utilisateur, "Id", "Nom");
            ViewData["Destinataires"] = new SelectList(_context.Departement, "Id", "Designation");
            return Page();
        }

        [BindProperty]
        public Courrier Courrier { get; set; } = default!;

        [BindProperty]
        public IFormFile FileUpload { get; set; }

        [BindProperty]
        public List<string> SelectedDestinataires { get; set; }
        [BindProperty]
        public string SelectedFlag { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            ClaimsPrincipal currentUser = User;
            /*if (!ModelState.IsValid || _context.Courrier == null || Courrier == null)*/
            if (_context.Courrier == null || Courrier == null)
            {
                return OnGet();
            }
            try {
                string pseudo = currentUser.Identity.Name;
                Utilisateur connectedUser = _employeService.GetUtilisateurByPseudo(pseudo);
                List<Departement> SelectedDepartements = departements
                    .Where(dept => SelectedDestinataires.Contains(dept.Id + "")).ToList();
                Flag Flag = flags
                    .Where(flag => SelectedFlag.Contains(flag.Id + "")).First();
                Courrier.Flag = Flag;
                connectedUser.Poste = _context.Poste.Find(connectedUser.IdPoste);
                _courrierService.CreationCourrier(Courrier, connectedUser, SelectedDepartements, FileUpload);
                await _context.SaveChangesAsync();
                return RedirectToPage("/ListeCourrier");

                /*return Page();*/

            } catch (Exception e) {

                ModelState.AddModelError(string.Empty, e.Message);
                return OnGet();
            }
        }
    }
}
