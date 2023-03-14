using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Objekti vezani za funkcionalnost G koda.
/// </summary>
namespace GCode
{
    /// <summary>
    /// Objekat koji opisuje tip adrese.
    ///</summary>
    public partial class Adresa : RefCounted {

        public enum TYPE {
            BLOCK,
            PREPARATORY,
            MISCELLANEOUS,
            X,
            Y,
            Z,
            SPEED,
            FEED_RATE,
        }
        
        /// <summary>Kojeg Enum TYPE je adresa.</summary>
        public readonly TYPE Tip;

        /// <summary>Karakter adrese.</summary>
        public readonly char Oznaka;

        /// <summary>Dictionary svih validnih adresa.</summary>
        public static Dictionary<char, TYPE> ValidneOznakeAdresa = new Dictionary<char, TYPE> {
            ['N'] = TYPE.BLOCK,
            ['M'] = TYPE.MISCELLANEOUS,
            ['G'] = TYPE.PREPARATORY,
            ['S'] = TYPE.SPEED,
            ['X'] = TYPE.X,
            ['Y'] = TYPE.Y,
            ['Z'] = TYPE.Z,
            ['F'] = TYPE.FEED_RATE,
        };

        public Adresa(char oznakaAdrese) {
            if (!ValidneOznakeAdresa.ContainsKey(oznakaAdrese)) {
                throw new ArgumentException($"Adresa sa oznakom '{oznakaAdrese}' ne postoji.");
            }

            Tip = ValidneOznakeAdresa[oznakaAdrese];
            Oznaka = oznakaAdrese;
        }

        public Adresa (TYPE tipAdrese) {
            if (!ValidneOznakeAdresa.ContainsValue(tipAdrese)) {
                throw new ArgumentException($"Adresa sa oznakom '{tipAdrese}' ne postoji.");
            }

            var tipKvp = ValidneOznakeAdresa.Single(kvp => kvp.Value == tipAdrese);

            Tip = tipKvp.Value;
            Oznaka = tipKvp.Key;
        }

    }

}
