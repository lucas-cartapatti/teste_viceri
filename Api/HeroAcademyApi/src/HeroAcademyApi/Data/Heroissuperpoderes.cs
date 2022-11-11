using System;
using System.Collections.Generic;

namespace HeroAcademyApi.Data
{
    public partial class HeroisSuperpoderes
    {
        public int Id { get; set; }
        public int HeroiId { get; set; }
        public int SuperpoderId { get; set; }

        public virtual Herois Heroi { get; set; } = null!;
        public virtual Superpoderes Superpoder { get; set; } = null!;
    }
}
