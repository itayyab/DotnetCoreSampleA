using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCoreSampleA
{
    public class Categories
    {
        [Key]
        public long Cat_id { get; set; }

        public string Cat_name { get; set; }

    }
}
