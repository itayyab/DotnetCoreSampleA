using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCoreSampleA.Models
{
    public class Cart
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Cart_id { get; set; }
        public string UserID { get; set; }
        public long TotalQty { get; set; }
        public long TotalAmount { get; set; }
        public string Status { get; set; }


        public List<CartDetails> CartDetails { get; set; }
    }
}
