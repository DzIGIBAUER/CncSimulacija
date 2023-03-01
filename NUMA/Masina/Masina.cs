using Godot;

/// <summary> Objekat Mašine. </summary>
public class Masina : MeshInstance {

    /// <summary> Radni prostor ove mašine. </summary>
    public RadniProstor RadniProstor{get; private set;}

    /// <summary> Alat ove mašine. </summary>
    public Alat Alat{get; private set;}

    /// <summary> Upravljačka jedinica ove mašine. </summary>
    public UpravljackaJedinica UpravljackaJedinica{get; private set;}

    public SteznaGlava SteznaGlava{get; private set;}

    public CSGSim CSGSim{ get; private set; }

    public override void _Ready() {
        base._Ready();

        RadniProstor = GetNode<RadniProstor>("RadniProstor");

        Alat = GetNode<Alat>("Alat");

        UpravljackaJedinica = GetNode<UpravljackaJedinica>("UpravljackaJedinica");

        SteznaGlava = GetNode<SteznaGlava>("SteznaGlava");

        CSGSim = GetNode<CSGSim>("CSGSim");
    }


    /// <summary> Pomera alat koristeći koordinate radnog prostora. </summary>
    public void PomeriAlat(GCode.Point gde, float brzina) {
        Alat.Pomeri(RadniProstor.ConvertFrom(gde), brzina);
    }
    
    
}
