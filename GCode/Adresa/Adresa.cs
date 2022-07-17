using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Objekti vezani za funkcionalnost G koda.
/// </summary>
namespace GCode
{
    /// <summary>
    /// Objekat koji opisuje tip adrese.
    ///</summary>
    public class Adresa : Reference {

        public enum TYPE {
            BLOCK,
            PREPARATORY,
            MISCELLANEOUS,
            X,
            Y,
            Z,
            SPEED,
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
            ['Y'] = TYPE.Z,
        };

        public Adresa(char oznakaAdrese) {
            if (!ValidneOznakeAdresa.ContainsKey(oznakaAdrese)) {
                throw new ArgumentException($"Adresa sa oznakom '{oznakaAdrese}' ne postoji.");
            }

            Tip = ValidneOznakeAdresa[oznakaAdrese];
            Oznaka = oznakaAdrese;
        }

    }

}
