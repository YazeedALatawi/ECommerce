using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class options
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int count { get; set; }

        public int productOptionsId { get; set; }
        [ForeignKey("productOptionsId")]
        public productOptions productOptions { get; set; }

    }
}
