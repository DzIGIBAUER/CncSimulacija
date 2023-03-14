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
    /// Objekat koji opisuje programsku rečenicu.
    ///</summary>
    public partial class ProgramskaRecenica : RefCounted {
        /// <summary>Recenica u izvornom obliku.</summary>
        public string Raw { get; }

        /// <summary>Sve programske reci u ovoj programskoj recenici.</summary>
        public Dictionary<Adresa, ParamValue> ProgramskeReci { get; } = new Dictionary<Adresa, ParamValue>();

        public RecenicaData Data = new RecenicaData{};

        public ProgramskaRecenica(string recenica) {
            ProgramskeReci = ParseRecenica(recenica);
            Raw = recenica;
        }

        public KeyValuePair<Adresa, ParamValue> Param(Adresa.TYPE tipAdrese, ParamValue defaultParamValue) {
            try {
                return Param(tipAdrese);
            }
            catch (GCodeException) {
                return new KeyValuePair<Adresa, ParamValue>(new Adresa(tipAdrese), defaultParamValue);
            }
        }

        public KeyValuePair<Adresa, ParamValue>? Param(Adresa.TYPE tipAdrese, bool optional = false) {
            try {
                return Param(tipAdrese);
            }
            catch (GCodeException) {
                return null;
            }
        }

        public KeyValuePair<Adresa, ParamValue> Param(Adresa.TYPE tipAdrese) {

            var programskaRec = ProgramskeReci.FirstOrDefault(
                kvp => kvp.Key.Tip == tipAdrese
            );

            KeyValuePair<Adresa, ParamValue> defaultKvp = default;

            if (programskaRec.Equals(defaultKvp)) {
                var oznakaAdrese = Adresa.ValidneOznakeAdresa.Single(kvp => kvp.Value == tipAdrese);
                throw new GCodeException($"Nije pronađen parametar { oznakaAdrese } u rečenici { this }.", this);
            }

            return programskaRec;
        }

        public override string ToString() {
            return Raw;
        }
    

        /// <summary>Uzima programsku recenicu i vraca dictionary.</summary>
        private static Dictionary<Adresa, ParamValue> ParseRecenica(string recenica) {
            Dictionary<Adresa, ParamValue> parsedReci = new Dictionary<Adresa, ParamValue>();
            string[] sadrzajRecenice = recenica.Split(new char[0], System.StringSplitOptions.RemoveEmptyEntries);
        
            foreach (string rec in sadrzajRecenice) {
                char oznakaAdrese = rec[0];
                string parametarStr = rec.Substr(1, rec.Length-1);
                var parametar = new ParamValue(parametarStr);

                Adresa adresaReci = new Adresa(oznakaAdrese);
                // throw new GCodeException($"Programska reč { rec } nije validna.");
                
                
                parsedReci[adresaReci] = parametar;
            }

            return parsedReci;
        }

    }

    public partial class ParamValue {

        public string Text { get; private set; }
        public int Number { get; private set; }

        public ParamValue(string text) {
            Text = text;
            Number = Int32.Parse(text);
        }

        public ParamValue(int number) {
            Number = number;
            Text = number.ToString();
        }

        public override string ToString() {
            return Text;
        }
    
    }

    public struct RecenicaData {
        public ArrayMesh ResultPart { get; set; }
        public Transform3D ResultPartGlobalTransform { get; set; }
    }
}