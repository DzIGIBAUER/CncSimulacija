using Godot;
using System;

public class SteznaGlava : MeshInstance {
	
    private CSGMesh _pripremak;
    public CSGMesh Pripremak
    {
        get => _pripremak;
        set {
            if (value is CSGMesh) {
                PostaviPripremak(value);
                _pripremak = value;
            } else {
                throw new ArgumentException($"Očekivan objekat tipa CSGMesh a dobijen {value.GetType()}.");
            }
        }
    }



    private void PostaviPripremak(CSGMesh pripremak) {
        GD.Print("Da stavim ", pripremak);
    }

}