using Godot;

/// <summary> Objekat Mašine. </summary>
public class Masina : MeshInstance {

    /// <summary> Radni prostor ove mašine. </summary>
    public RadniProstor RadniProstor{get; private set;}

    /// <summary> Alat ove mašine. </summary>
    public Alat Alat{get; private set;}

    public override void _Ready() {
        base._Ready();

        RadniProstor = GetNode<RadniProstor>("RadniProstor");

        Alat = GetNode<Alat>("Alat");
    }


    /// <summary> Pomera alat koristeći koordinate radnog prostora. </summary>
    public void PomeriAlat(GCode.Point gde, float brzina) {
        Alat.Pomeri(RadniProstor.ConvertFrom(gde), brzina);
    }
    
    
}
