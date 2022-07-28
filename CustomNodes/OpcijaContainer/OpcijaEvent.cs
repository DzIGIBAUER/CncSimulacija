using Godot;

/// <summary>
/// Objekat koji je prosledjen kada se emituje signal.
/// Postoji iz razloga što može da bude prosledjen samo objekat koji inherituje Godot.Object.
/// </summary>
public class OpcijaEvent : Resource {

    /// <summary> Naziv opcije koja je promenjena. </summary>    
    public readonly string Naziv;

    /// <summary> Nova vrednost opcije. </summary>
    public readonly object Vrednost;


    public OpcijaEvent(string naziv, object vrednost) {
        Naziv = naziv;
        Vrednost = vrednost;

    }

}
