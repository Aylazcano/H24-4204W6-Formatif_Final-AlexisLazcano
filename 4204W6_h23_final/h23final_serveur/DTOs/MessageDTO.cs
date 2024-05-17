using h23final_serveur.Models;

namespace h23final_serveur.DTOs
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public List<Reaction>? Reactions { get; set; }
        public string UserName { get; set; } = null!;
    }
}
