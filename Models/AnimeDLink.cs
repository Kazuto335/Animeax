using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animeax.Models
{
    public class AnimeDLink
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DLink { get; set; }
        [Required]
        public int AAnimeID { get; set; }   
    }
}
