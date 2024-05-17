using System.Text.Json.Serialization;

namespace h23final_serveur.Models
{
    public class Reaction
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string MimeType { get; set; } = null!;
        public string FileName { get; set; } = null!;
        [JsonIgnore]
        public virtual Message Message { get; set; } = null!;
        [JsonIgnore]
        public virtual List<User> Users { get; set; } = null!;
    }
}
