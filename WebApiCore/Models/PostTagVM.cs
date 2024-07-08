using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Model.Models
{
    public class PostTagVM
    {
        public int PostID { get; set; }
        public string TagID { get; set; }
        //public virtual TagVM Tag { get; set; }

        //public virtual PostVM Post { get; set; }
    }
}
