using Godot;
using GCode;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

/// <summary>
/// Upravljačka jedinica je interfejs G koda i maišne.
/// </summary>
public partial class UpravljackaJedinica : Node {

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

	public async Task SpremiSimulaciju() {
		var oldAlatPos = _masina.Alat.GlobalPosition;
		foreach(var recenica in _programskeRecenice) {
			var rec = recenica.Param(Adresa.TYPE.PREPARATORY, true) ?? recenica.Param(Adresa.TYPE.MISCELLANEOUS);

			// ako je ta funkcije definisana
			var imeFunkcije = $"{rec.Key.Oznaka}{rec.Value}";


            
			if (_funkcije.GetType().GetMethod(imeFunkcije) == null) {
				throw new GCodeException($"Funkcija { imeFunkcije } nije definisana.");
			}
            
			MethodInfo method = _funkcije.GetType().GetMethod(imeFunkcije);
            var task = (Task)method.Invoke(_funkcije, new object[] { recenica, true });
			if (task != null) await task;
		}
		_masina.Alat.GlobalPosition = oldAlatPos;

		_simulated = true;
	}

	public async Task PokreniSimulaciju() {
		if (!_simulated) {
			throw new InvalidOperationException("Simulacija pokrenuta pre spremanja.");
		}

        foreach(var recenica in _programskeRecenice) {
			var rec = recenica.Param(Adresa.TYPE.PREPARATORY, true) ?? recenica.Param(Adresa.TYPE.MISCELLANEOUS);

			var imeFunkcije = $"{rec.Key.Oznaka}{rec.Value}";
            
            // ne moze _funkcije.Call jer godot ne prepoznaju funkciju ako je async Task, nmp sto...
            //var result = _funkcije.Call(imeFunkcije, recenica, false);

            MethodInfo method = _funkcije.GetType().GetMethod(imeFunkcije);
            var task = (Task)method.Invoke(_funkcije, new object[] { recenica, false });

			if (task != null) await task;
			
		}
    }


}
