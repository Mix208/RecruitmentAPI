using Microsoft.AspNetCore.Mvc; // Importe les outils nécessaires pour créer un contrôleur API

namespace RecruitmentAPI.API.Controllers // Namespace = chemin logique du code
{
    [ApiController] // Indique que c’est un contrôleur d’API (gère automatiquement les erreurs, validations, etc.)
    [Route("api/[controller]")] // Définit la route comme /api/healthcheck (car [controller] = nom du fichier sans "Controller")
    public class HealthCheckController : ControllerBase // Hérite de ControllerBase (pour API sans HTML)
    {
        [HttpGet] // Méthode GET sur /api/healthcheck
        public IActionResult GetStatus()
        {
            return Ok(new { status = "API OK" }); // Retourne un objet JSON avec un champ "status"
        }
    }
}
