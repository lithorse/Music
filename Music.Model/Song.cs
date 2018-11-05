
using System.ComponentModel.DataAnnotations;

namespace Music.Model
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
