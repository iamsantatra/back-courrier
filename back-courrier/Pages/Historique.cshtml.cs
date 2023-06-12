using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using back_courrier.Data;
using back_courrier.Models;
using back_courrier.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace back_courrier.Pages
{
    public class HistoriqueModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourrierService _courrierService;
        private readonly IUtilisateurService _employeService;
        public List<Utilisateur> Prochain;
        public List<Utilisateur> Coursiers;
        public List<Utilisateur> Secretaires;
        public List<Utilisateur> Directeurs;
        private Utilisateur _currentUser;
        private readonly IConfiguration _configuration;
        [BindProperty]
        public int IdStatut { get; set; }
        [BindProperty]
        public int SelectedIdResponsable { get; set; }
        [BindProperty]
        public CourrierDestinataire CourrierDestinataire { get; set; } = default!;
        public HistoriqueModel(ApplicationDbContext context, ICourrierService courrierService,
            IUtilisateurService employeService, IConfiguration configuration)
        {
            _context = context;
            this._courrierService = courrierService;
            this._employeService = employeService;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync(int id, int idDepartement, int idStatut)
        {
            string dirRole = _configuration.GetValue<string>("Constants:Role:DirRole");
            string secRole = _configuration.GetValue<string>("Constants:Role:SecRole");
            string courRole = _configuration.GetValue<string>("Constants:Role:CouRole");
            string recRole = _configuration.GetValue<string>("Constants:Role:RecRole");
            ViewData["DirRole"] = dirRole;
            ViewData["SecRole"] = secRole;
            ViewData["CouRole"] = courRole;
            ViewData["RecRole"] = recRole;

            _currentUser = _employeService.GetUtilisateurByClaim(User);
            _currentUser.Poste = _context.Poste.FirstOrDefault(p => p.Id == _currentUser.IdPoste);
            Coursiers = _employeService.GetCoursier();
            Secretaires = _employeService.GetSecretaireByDep(idDepartement);
            Directeurs = _employeService.GetDirecteurByDep(idDepartement);
            IdStatut = idStatut;
            Prochain = _employeService.GetUtilisateurSuivant(_currentUser, idDepartement, idStatut);
/*            if (Prochain != null)
            {
                ViewData["Prochain"] = new SelectList(Prochain, "Id", "Nom");
            }*/
            if (Coursiers != null)
            {
                ViewData["Coursiers"] = new SelectList(Coursiers, "Id", "Nom");
            }
            if(Secretaires != null)
            {
                ViewData["Secretaires"] = new SelectList(Secretaires, "Id", "Nom");
            }
            if(Directeurs != null)
            {
                ViewData["Directeurs"] = new SelectList(Directeurs, "Id", "Nom");
            }
            CourrierDestinataire = _courrierService.GetDetailsCourrier(id);
            if (CourrierDestinataire == null)
            {
                return RedirectToPage("./ListeCourrier");
            }
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int id, int idDepartement, int idStatut)
        {
/*            if (!ModelState.IsValid || _context.CourrierDestinataire == null || CourrierDestinataire == null)
            {
                await OnGetAsync(id, idDepartement, idStatut);
                return Page();
            }*/
            try
            {
                string dirRole = _configuration.GetValue<string>("Constants:Role:DirRole");
                string secRole = _configuration.GetValue<string>("Constants:Role:SecRole");
                string courRole = _configuration.GetValue<string>("Constants:Role:CouRole");
                string recRole = _configuration.GetValue<string>("Constants:Role:RecRole");
                ViewData["DirRole"] = dirRole;
                ViewData["SecRole"] = secRole;
                ViewData["CouRole"] = courRole;
                ViewData["RecRole"] = recRole;

                _currentUser = _employeService.GetUtilisateurByClaim(User);
                _currentUser.Poste = _context.Poste.FirstOrDefault(p => p.Id == _currentUser.IdPoste);

                CourrierDestinataire.IdStatut = idStatut;
                _currentUser = _employeService.GetUtilisateurByClaim(User);
                _currentUser.Poste = _context.Poste.FirstOrDefault(p => p.Id == _currentUser.IdPoste);
                
                Utilisateur userSelected = _employeService.GetUtilisateurByid(CourrierDestinataire.IdResponsable);
                userSelected.Poste = _context.Poste.FirstOrDefault(p => p.Id == userSelected.IdPoste);
                
                if (userSelected.Poste.Code == courRole)
                {
                    _courrierService.TransfertCoursier(CourrierDestinataire);
                }
                if(userSelected.Poste.Code == secRole)
                {
                    _courrierService.TransfertSecretaire(CourrierDestinataire);
                }
                if (userSelected.Poste.Code == dirRole)
                {
                    _courrierService.TransfertDirecteur(CourrierDestinataire);
                }

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
