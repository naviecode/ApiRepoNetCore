using ShopApi.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Model.Models
{
    public class PostCategoryVM
    {
        public int ID { get; set; }
        public string Name { get; set;}
        public string Alias { get; set;}
        public string Description { get; set;}
        public int? ParentID { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? HomeFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string MetaKeyWord { get; set; }
        public string MetaDesc { get; set; }
        public bool Status { get; set; }
        //public virtual IEnumerable<PostVM> Posts { get; set; }
    }
}
