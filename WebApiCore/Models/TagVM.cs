using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Model.Models
{
    public class TagVM
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
