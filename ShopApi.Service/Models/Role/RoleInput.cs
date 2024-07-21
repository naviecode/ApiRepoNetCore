using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Models.Role
{
    public class RoleInput
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NomalizeName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string MetaKeyWord { get; set; }
        public string MetaDesc { get; set; }
        public bool Status { get; set; }
    }
}
