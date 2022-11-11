using System;
using System.Collections.Generic;

namespace HeroAcademyApi.Data
{
    public partial class Herois
    {
        public Herois()
        {
            HeroisSuperpoderes = new HashSet<HeroisSuperpoderes>();
        }

        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string NomeHeroi { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public float Altura { get; set; }
        public float Peso { get; set; }

        public virtual ICollection<HeroisSuperpoderes> HeroisSuperpoderes { get; set; }
    }
}
