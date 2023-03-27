using Godot;
using System;

/// <summary> Objekat Alata. </summary>
public partial class Alat : Node3D {

	[Signal]
	public delegate void TargetReachedEventHandler();

	private float _brzinaHoda = 0;
	/// <summary> Brzina kojom će se alat kretati u sekundama. </summary>
	public float BrzinaHoda {
		get => _brzinaHoda;
		set {
			if (value >= 0) {
				_brzinaHoda = value;
			} else {
				throw new ArgumentException($"Očekivana brzina hoda alata veća od 0, a dobijena { value }.");
			}
		}
	}

	/// <summary> Pozicija do koje alat treba da stigne. </summary>
	private Vector3 ?CiljnaPozicija;


	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		if (CiljnaPozicija == null) return;
		
		if (Position.IsEqualApprox((Vector3)CiljnaPozicija)) {
			CiljnaPozicija = null;
			EmitSignal(nameof(TargetReached));
			return;
		}

		float moveAmount = BrzinaHoda * (float)delta;
		Position = Position.MoveToward((Vector3)CiljnaPozicija, moveAmount);
	}


	/// <summary> Pomera alat određenom brzinom na novu poziciju u koord. sistemu mašine. </summary>
	public void Pomeri(Vector3 gde, float brzina) {
		BrzinaHoda = brzina;
		CiljnaPozicija = gde;
		
	}

}
