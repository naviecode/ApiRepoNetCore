using ShopApi.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Model.Models
{
    [Table("Orders")]
    public class Order : Auditable
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerNM { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerAddress { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerEmail { get; set; }
        [Required]
        [MaxLength(50)]
        public string CustomerMobile { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerMessage { get; set; }
        [MaxLength(256)]
        public string PaymentMethod { get; set; }
        [Required]
        public string PaymentStatus { get; set; }

        //public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
