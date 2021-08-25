using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotnetCoreSampleA.Models
{
    public class Product
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Pr_id { get; set; }
        public string Pr_name { get; set; }
        public string Pr_desc { get; set; }
        public string Pr_Picture { get; set; }
        public long Pr_price { get; set; }
        public long CategoryForeignKey { get; set; }
       
        [ForeignKey("CategoryForeignKey")]
        public Category Category { get; set; }
    }
}
