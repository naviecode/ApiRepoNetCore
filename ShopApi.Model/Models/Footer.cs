using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Model.Models
{
    //[Table("Footers")]
    public class Footer
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        //[Required]
        public string Content { get; set; }
    }
}
