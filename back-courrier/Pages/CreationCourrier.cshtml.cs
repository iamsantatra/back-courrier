
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using back_courrier.Data;
using back_courrier.Models;

namespace back_courrier.Pages
{
    public class CreationCourrierModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreationCourrierModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Utilisateurs = _context.Utilisateur.ToList();
            return Page();
        }

        [BindProperty]
        public Courrier Courrier { get; set; } = default!;

        public List<Utilisateur> Utilisateurs { get; set; }
        [BindProperty]
        public int SelectedUserId { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Courrier == null || Courrier == null)
            {
                return Page();
            }

            int selectedUserId = int.Parse(Request.Form["SelectedUserId"]);
            Courrier.Objet = selectedUserId.ToString();
            _context.Courrier.Add(Courrier);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
