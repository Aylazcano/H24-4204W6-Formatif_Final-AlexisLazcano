using System.Text.Json.Serialization;

namespace h23final_serveur.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public virtual List<Message>? Messages { get; set; }
    }
}
