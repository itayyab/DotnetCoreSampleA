using DotnetCoreSampleA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotnetCoreSampleA
{
    public class Category
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Cat_id { get; set; }
        public string Cat_name { get; set; }
        
        public List<Product> Products { get; set; }
    }
}
