using System.Text.Json.Serialization;

namespace h23final_serveur.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime SentAt { get; set; }
        [JsonIgnore]
        public virtual Channel Channel { get; set; } = null!;
        public virtual List<Reaction>? Reactions { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; } = null!;
    }
}
