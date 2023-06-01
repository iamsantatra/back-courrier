using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using back_courrier.Data;
using back_courrier.Models;
using back_courrier.Helper;

namespace back_courrier.Pages
{
    public class CreationCourrierModel : PageModel
    {
        // crée une variable constant statut de valeur 1
        private const int STATUT = 1;

        private readonly ApplicationDbContext _context;
        public Utilisateur _utilisateurConn { get; set; }
        public CreationCourrierModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Get the object utilisateur from session
            _utilisateurConn = HttpContext.Session.GetObject<Utilisateur>("utilisateur");
            _departements = _context.Departement.ToList();
            _flags = new List<string> { "normal", "urgent", "important" };
            return Page();
        }

        [BindProperty]
        public List<string> _flags { get; set; } 

        [BindProperty]
        public Courrier Courrier { get; set; } = default!;
        [BindProperty]
        public List<Departement> _departements { get; set; } 
        [BindProperty]
        public List<Departement> SelectedDepartement { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Courrier == null || Courrier == null)
            {
                OnGet();
                return Page();
            }
            var transaction = _context.Database.BeginTransaction();
            try
            {
                // Ajout courrier
                Courrier.Receptionniste = _utilisateurConn;
                _context.Courrier.Add(Courrier);
                await _context.SaveChangesAsync(); // Save changes to generate the Id for the Courrier entity

                // Ajout CourrierDestinataire
                foreach (var Departement in SelectedDepartement)
                {
                    CourrierDestinataire courrierDestinataire = new CourrierDestinataire();
                    courrierDestinataire.Courrier = Courrier;
                    courrierDestinataire.DepartementDestinataire = Departement;
                    _context.CourrierDestinataire.Add(courrierDestinataire);
                    await _context.SaveChangesAsync(); // Save changes to generate the Id for the Courrier entity
                    // Ajout Historique
                    Historique historique = new Historique();
                    historique.CourrierDestinataire = courrierDestinataire;
                    historique.Statut = _context.Statut.Find(STATUT);
                    historique.DateHistorique = Courrier.DateCreation;
                    _context.Historique.Add(historique);
                }
                await _context.SaveChangesAsync(); // Save changes for Historique entities

                transaction.Commit();
                return RedirectToPage("./ListeCourrier");
            }
            catch (Exception e)
            {
                // show error message from Exception object to the page 
                ModelState.AddModelError(string.Empty, e.Message);
                // stay in this page and don't forget to cal the OnGet method
                transaction.Rollback();
                OnGet();
                return Page();
                /*throw;*/
            }
        }
    }
}
