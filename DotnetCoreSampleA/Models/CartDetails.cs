using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCoreSampleA.Models
{
    public class CartDetails
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public long CD_id { get; set; }
        public long CD_Pr_id { get; set; }
        public long CD_Pr_Qty { get; set; }
        public long CD_Pr_price { get; set; }
        public long CD_Pr_Amnt { get; set; }
        public long CartForeignKey { get; set; }
        [ForeignKey("CartForeignKey")]
        public Cart Cart { get; set; }
        public long ProductForeignKey { get; set; }
        [ForeignKey("ProductForeignKey")]
        public Product Product { get; set; }
    }
}
