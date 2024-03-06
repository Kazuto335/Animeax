using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Animeax.Models
{
    public class AAnime
    {
        [Key] 
        public int ID { get; set; }
        [Required]
       
        public string Name { get; set; }

        public string OName { get; set; }
        [Required]
        [Range(0.1,10)]
        public float Score { get; set; }
        [Required]
        public string Genres { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Description { get; set; }
       
        public string Aired { get; set; }
       
        public string Status { get; set; } 
        public string Rating { get; set; }
        
        public string Extra { get; set; } 
        [Required]
        public string ImgURL { get; set; }
    }

}
