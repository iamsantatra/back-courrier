
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using back_courrier.Data;
using back_courrier.Models;
using back_courrier.Helper;

namespace back_courrier.Pages
{
    public class CreationCourrierModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Utilisateur UtilisateurConn { get; set; }
        public CreationCourrierModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Get the object utilisateur from session
            UtilisateurConn = HttpContext.Session.GetObject<Utilisateur>("utilisateur");
            Departements = _context.Departement.ToList();
            Flags = new List<string> { "normal", "urgent", "important" };
            return Page();
        }

        [BindProperty]
        public List<string> Flags { get; set; }
        [BindProperty]
        public Courrier Courrier { get; set; } = default!;
        [BindProperty]
        public List<Departement> Departements { get; set; } 
        [BindProperty]
        public List<int> SelectedIdDepartement { get; set; }

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
                Courrier.IdReceptionniste = HttpContext.Session.GetObject<Utilisateur>("utilisateur").Id;
                Console.WriteLine(Courrier);
                _context.Courrier.Add(Courrier);
                await _context.SaveChangesAsync(); // Save changes to generate the Id for the Courrier entity

                // Ajout CourrierDestinataire
                foreach (var idDepartement in SelectedIdDepartement)
                {
                    CourrierDestinataire courrierDestinataire = new CourrierDestinataire();
                    courrierDestinataire.IdCourrier = Courrier.Id;
                    courrierDestinataire.IdDepartementDestinataire = idDepartement;
                    _context.CourrierDestinataire.Add(courrierDestinataire);
                    await _context.SaveChangesAsync(); // Save changes to generate the Id for the Courrier entity

                    // Ajout Historique
                    Historique historique = new Historique();
                    historique.IdCourrierDestinataire = courrierDestinataire.Id;
                    historique.IdStatut = 1;
                    historique.DateHistorique = Courrier.DateCreation;
                    _context.Historique.Add(historique);
                }
                await _context.SaveChangesAsync(); // Save changes for Historique entities

                transaction.Commit();
                return RedirectToPage("./ListeCourrier");
            } catch (Exception e)
            {
                transaction.Rollback();
                // show error message from Exception object to the page 
                ModelState.AddModelError(string.Empty, e.Message);
                /*throw;*/
                OnGet();
                return Page();
            }
        }
    }
}
