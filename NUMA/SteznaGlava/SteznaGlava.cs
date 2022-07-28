using Godot;
using System;

/// <summary> Objekat stezne glave </summary>
public class SteznaGlava : MeshInstance {
	
    private CSGMesh _pripremak;
    /// <summary> Pripremak koji se nalazi u steznoj glavi. </summary>
    public CSGMesh Pripremak
    {
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


    /// <summary> Fizički postavlji mesh pripremka u steznu glavu. </summary>
    private void PostaviPripremak(CSGMesh pripremak) {
        GD.Print("Da stavim ", pripremak);
    }

}