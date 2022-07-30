using Godot;
using System;

/// <summary> Objekat stezne glave. </summary>
public class SteznaGlava : MeshInstance {
	
    [Export]
    private NodePath _pripremakPath;

    private CSGMesh _pripremak;
    /// <summary> Pripremak koji se nalazi u steznoj glavi. </summary>
    public CSGMesh Pripremak {
        get => _pripremak;
        set {
            if (value is CSGMesh) {
                PostaviPripremak(value);
                _pripremak = value;
            } else {
                throw new ArgumentException($"Očekivan objekat tipa CSGMesh a dobijen { value.GetType() }.");
            }
        }
    }


    private int _brzinaVretena = 40;
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


    public override void _Ready() {
        base._Ready();

        if (!_pripremakPath.IsEmpty()) {
            Pripremak = GetNode<CSGMesh>(_pripremakPath);
        }
    }


    public override void _PhysicsProcess(float delta) {
        base._PhysicsProcess(delta);

        if (BrzinaVretena > 0) {
            // (obrtaj_po_sekundi) * pun_krug_radian * vreme_od_zadnjeg_frame-a
            float rotateAmount = (BrzinaVretena / 60f) * Mathf.Tau * delta;
            //Rotate(MasinaProxy.Z, rotateAmount);
        }
    }

    /// <summary> Fizički postavlji mesh pripremka u steznu glavu. </summary>
    private void PostaviPripremak(CSGMesh pripremak) {
        GD.Print("Da stavim ", pripremak);
    }

}