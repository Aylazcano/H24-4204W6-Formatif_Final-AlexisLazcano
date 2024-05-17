using Microsoft.AspNetCore.Identity;

namespace h23final_serveur.Models
{
    public class User : IdentityUser
    {
        public virtual List<Message>? Messages { get; set; } 
        public virtual List<Reaction>? Reactions { get; set; }
    }
}
