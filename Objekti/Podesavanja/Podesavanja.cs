using Godot;

/// <summary>
/// Singleton koji učitava i čuva podešavanja korisnika.
/// </summary>
public class Podesavanja : Node {

    
    private float _osetljivostKretanja = 0.25f;
    /// <summary>Osetljivost kretanja kamere u modu razgledanja.</summary>
    public float OsetljivostKretanja
    {
        get {return _osetljivostKretanja;}
        set {_osetljivostKretanja = (value < Kontroler.MinMoveFactor) ? Kontroler.MinMoveFactor : value;}
    }



    private float _osetljivostMisa = 0.5f;
    /// <summary>Osetljivost miša.</summary>
    public float OsetljivostMisa
    {
        get {return _osetljivostMisa;}
        set {_osetljivostMisa = (value < Kontroler.MinRotateFactor) ? Kontroler.MinRotateFactor : value;}
    }

}