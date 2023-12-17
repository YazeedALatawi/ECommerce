using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class productOptions
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public int prdouctId { get; set; }

        [ForeignKey("prdouctId")]
        public Product product { get; set; }
        public List<options>? Options { get; set; }

    }
}
