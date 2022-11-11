using System;
using System.Collections.Generic;

namespace HeroAcademyApi.Data
{
    public partial class Superpoderes
    {
        public Superpoderes()
        {
            HeroisSuperpoderes = new HashSet<HeroisSuperpoderes>();
        }

        public int Id { get; set; }
        public string Superpoder { get; set; } = null!;
        public string Descricao { get; set; } = null!;

        public virtual ICollection<HeroisSuperpoderes> HeroisSuperpoderes { get; set; }
    }
}
