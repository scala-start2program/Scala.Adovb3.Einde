using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.Adovb3.Core.Entities
{
    public class Toestel
    {
        public string Id { get; private set; }
        public string Merk { get; set; }
        public string Serie { get; set; }
        public decimal Prijs { get; set; }
        public int Stock { get; set; }
        public string Soort { get; set; }

        public Toestel()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Toestel(string merk, string serie, decimal prijs, int stock, string soort)
        {
            Id = Guid.NewGuid().ToString();
            Merk = merk;
            Serie = serie;
            Prijs = prijs;
            Stock = stock;
            Soort = soort;
        }
        public Toestel(string id, string merk, string serie, decimal prijs, int stock, string soort)
        {
            Id = id;
            Merk = merk;
            Serie = serie;
            Prijs = prijs;
            Stock = stock;
            Soort = soort;
        }
        public override string ToString()
        {
            return $"{Merk}-{Serie} ({Soort})";
        }
    }
}
