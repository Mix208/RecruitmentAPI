using Microsoft.AspNetCore.Mvc;                 // Fournit les attributs et types pour construire des contrôleurs d'API
using Microsoft.EntityFrameworkCore;            // Nécessaire pour certaines méthodes EF Core (ex: AnyAsync, SaveChangesAsync)
using RecruitmentAPI.API.Data;                  // Pour accéder à AppDbContext
using RecruitmentAPI.API.Models;
using Microsoft.AspNetCore.Authorization;    
           // Pour accéder aux entités (Job, etc.)

namespace RecruitmentAPI.API.Controllers
{
    [Authorize] // Assure que toutes les actions nécessitent une authentification
    [ApiController]                             // Active les comportements automatiques d'une API (validation modèle, etc.)
    [Route("api/[controller]")]                 // Route de base : /api/jobs
    public class JobsController : ControllerBase // ControllerBase = contrôleur sans vues (parfait pour API REST)
    {
        private readonly AppDbContext _context; // Conserve le DbContext en champ privé pour l'utiliser dans toutes les actions

        public JobsController(AppDbContext context) // Le DbContext est injecté via l'injection de dépendances
        {
            _context = context;
        }

        // GET: api/jobs
        [AllowAnonymous] // Permet aux utilisateurs non authentifiés de voir les jobs
        [HttpGet]                                // Indique que cette méthode répond à une requête HTTP GET
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs() // Retourne une liste de Job en JSON
        {
            // Récupère tous les jobs, inclut les candidatures associées
            return await _context.Jobs
                .Include(j => j.Applications)
                .ToListAsync();
        }

        // GET: api/jobs/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]                        // Route paramétrée : /api/jobs/5
        public async Task<ActionResult<Job>> GetJobById(int id) // Récupère un job par son Id
        {
            // Cherche le job correspondant, inclut les candidatures
            var job = await _context.Jobs
                .Include(j => j.Applications)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)                     // Si non trouvé → 404 NotFound
                return NotFound();

            return job;                          // Sinon → 200 OK + job en JSON
        }

        // POST: api/jobs
        [HttpPost]                               // Reçoit un job à créer (en JSON)
        public async Task<ActionResult<Job>> CreateJob(Job job)
        {
            _context.Jobs.Add(job);              // Marque l'entité comme "à insérer"
            await _context.SaveChangesAsync();   // Exécute l'INSERT en base

            // Retourne 201 Created + l'URL de la ressource créée (GET /api/jobs/{id})
            return CreatedAtAction(nameof(GetJobById), new { id = job.Id }, job);
        }

        // PUT: api/jobs/{id}
        [HttpPut("{id}")]                        // Route : /api/jobs/5
        public async Task<IActionResult> UpdateJob(int id, Job updatedJob)
        {
            if (id != updatedJob.Id)             // Sécurité : l'id URL doit correspondre à l'id JSON
                return BadRequest("L'id de l'URL ne correspond pas à l'id du corps de requête.");

            var job = await _context.Jobs.FindAsync(id); // Récupère le job existant dans la base

            if (job == null)                     // Si l'entité n'existe pas → 404
                return NotFound();

            // Mise à jour champ par champ pour éviter l'overposting
            job.Title = updatedJob.Title;
            job.Description = updatedJob.Description;
            job.ContractType = updatedJob.ContractType;
            job.Location = updatedJob.Location;
            job.UserId = updatedJob.UserId;      // ⚠️ À sécuriser plus tard avec l'auth + rôles
            // job.CreatedAt : on ne le touche généralement pas lors d'un update

            await _context.SaveChangesAsync();   // Enregistre les modifications (UPDATE en base)

            return NoContent();                  // 204 : succès, rien à renvoyer
        }

        // DELETE: api/jobs/{id}
        [HttpDelete("{id}")]                     // Route : /api/jobs/5
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id); // Cherche le job par son id

            if (job == null)                     // Si pas trouvé → 404
                return NotFound();

            _context.Jobs.Remove(job);           // Marque le job pour suppression
            await _context.SaveChangesAsync();   // Exécute le DELETE en base

            return NoContent();                  // 204 : succès sans contenu
        }
    }
}
