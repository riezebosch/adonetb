using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;

namespace CodeFirstDemo
{
    /// <summary>
    /// Met wat hacken een eigen convetion in elkaar geklust.
    /// LET OP! Garantie tot de deur.
    /// </summary>
    class TblUnderscoreConvention : Convention
    {

        public TblUnderscoreConvention()
        {
            this.Types().Configure(a => a.ToTable("tbl_" + a.ClrType.Name));
        }
    }
}
