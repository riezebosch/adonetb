using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CodeFirstDemo
{
    //[Table("Demos")]
    public class Demo
    {
        [Key]
        public int DemoIdentifier { get; set; }

        [Required]
        [MaxLength(40)]
        public string Cursus { get; set; }

        [Index("OmschrijvingIndex", Order = 1, IsUnique = true)]
        [MaxLength(250)]
        public string Omschrijving { get; set; }

        [Index("OmschrijvingIndex", Order = 2, IsUnique = true)]
        [MaxLength(10)]
        public string DummyData { get; set; }

        public int? Duur { get; set; }
    }
}
