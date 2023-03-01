using Godot;
using GCode;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

/// <summary>
/// Upravljačka jedinica je interfejs G koda i maišne.
/// </summary>
public class UpravljackaJedinica : Node {

    private List<ProgramskaRecenica> _programskeRecenice = new List<ProgramskaRecenica>();

    private Masina _masina;

    private Funkcije _funkcije;

    private bool _simulated = false;

    public override void _Ready() {
        base._Ready();

        _masina = GetParent<Masina>();
        _funkcije = new Funkcije(_masina);
    }

    public void UcitajKod(params string[] kod) {
        foreach (string linija in kod) {
            _programskeRecenice.Add(new ProgramskaRecenica(linija));
        }
    }

    public void SpremiSimulaciju() {
        foreach(var recenica in _programskeRecenice) {
            var rec = recenica.Param(Adresa.TYPE.PREPARATORY, true) ?? recenica.Param(Adresa.TYPE.MISCELLANEOUS);

            // ako je ta funkcije definisana
            var imeFunkcije = $"{rec.Key.Oznaka}{rec.Value}";
            if (!_funkcije.HasMethod(imeFunkcije)) {
                throw new GCodeException($"Funkcija { imeFunkcije } nije definisana u rečenici { recenica }");
            }

            _funkcije.Call(imeFunkcije, recenica, true);

        }

        _simulated = true;
    }

    public async void PokreniSimulaciju() {
        if (!_simulated) {
            throw new InvalidOperationException("Simulacija pokrenuta pre spremanja.");
        }

        foreach(var recenica in _programskeRecenice) {
            var rec = recenica.Param(Adresa.TYPE.PREPARATORY, true) ?? recenica.Param(Adresa.TYPE.MISCELLANEOUS);

            var imeFunkcije = $"{rec.Key.Oznaka}{rec.Value}";

            // ne moze _funkcije.Call jer godot ne moze da prebaci Task objekat blabla...
            MethodInfo method = _funkcije.GetType().GetMethod(imeFunkcije);
            Task.WaitAll((Task)method.Invoke(_funkcije, new []{ recenica }));

            GD.Print(imeFunkcije);
            
        }
    }


}
