using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Model.Models
{
    [Table("UserTokens")]
    public class UserTokens
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public int UserID { get; set; }
        [MaxLength(100)]
        public string LoginProvider { get; set;}
        [Required]
        [MaxLength(256)]
        public string NameToken { get; set; }
        [Required]
        [MaxLength(256)]
        public string ValueToken { get; set; }
    }
}
