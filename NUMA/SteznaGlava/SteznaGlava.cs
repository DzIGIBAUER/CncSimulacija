using Godot;
using System;

/// <summary> Objekat stezne glave. </summary>
public partial class SteznaGlava : MeshInstance3D {

	private CsgMesh3D _pripremak;
	/// <summary> Pripremak koji se nalazi u steznoj glavi. </summary>
	[Export] public CsgMesh3D Pripremak {
		get => _pripremak;
		set {
			if (value is CsgMesh3D) {
				PostaviPripremak(value);
				_pripremak = value;
			} else {
				throw new ArgumentException($"Očekivan objekat tipa CSGMesh3D a dobijen { value.GetType() }.");
			}
		}
	}


	private int _brzinaVretena = 0;
	public int BrzinaVretena {
		get => _brzinaVretena;
		set {
			if (value >= 0) {
				_brzinaVretena = value;
			} else {
				throw new ArgumentException($"Očekivana brzina obrtanja glavnog vretena veća od 0, a dobijena { value }.");
			}
		}
	}


	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		
		if (BrzinaVretena > 0) {
			// (obrtaj_po_sekundi) * pun_krug_radian * vreme_od_zadnjeg_frame-a
			float rotateAmount = (BrzinaVretena / 60f) * Mathf.Tau * (float)delta;
			Rotate(Vector3.Right, rotateAmount);
		}
	}

	/// <summary> Fizički postavlji mesh pripremka u steznu glavu. </summary>
	private void PostaviPripremak(CsgMesh3D pripremak) {
		//GD.Print("Da stavim ", pripremak);
	}

}
