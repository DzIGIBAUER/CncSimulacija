using Godot;
using System.Collections.Generic;

/// <summary>
/// Objekti vezani za funkcionalnost G koda.
/// </summary>
namespace GCode
{
    /// <summary>
    /// Objekat koji opisuje programsku rečenicu.
    ///</summary>
    public class ProgramskaRecenica : Reference {
        /// <summary>Recenica u izvornom obliku.</summary>
        public readonly string Raw;
        
        /// <summary>Sve programske reci u ovoj programskoj recenici.</summary>
        public readonly Dictionary<Adresa, string> ProgramskeReci = new Dictionary<Adresa, string>();

        public ProgramskaRecenica(string recenica) {
            ProgramskeReci = ParseRecenica(recenica);
            Raw = recenica;
        }


        public override string ToString() {
            return $"Programska rečenica({ Raw })";
        }
    

        /// <summary>Uzima programsku recenicu i vraca dictionary.</summary>
        private static Dictionary<Adresa, string> ParseRecenica(string recenica) {
            Dictionary<Adresa, string> parsedReci = new Dictionary<Adresa, string>();
            string[] sadrzajRecenice = recenica.Split(new char[0], System.StringSplitOptions.RemoveEmptyEntries);
        
            foreach (string rec in sadrzajRecenice) {
                char oznakaAdrese = rec[0];
                string parametar = rec.Substr(1, rec.Length-1);

                Adresa adresaReci = new Adresa(oznakaAdrese);
                parsedReci[adresaReci] = parametar;
            }

            return parsedReci;
        }

    }
}