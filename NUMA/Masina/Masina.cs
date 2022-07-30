using Godot;

/// <summary> Objekat Mašine. </summary>
public class Masina : MeshInstance {

    public RadniProstor RadniProstor;

    public override void _Ready() {
        base._Ready();

        /// <summary> Radni prpstor ove mašine. </summary>
        RadniProstor = GetNode<RadniProstor>("RadniProstor");

        //Alat a = GetNode<Alat>("Alat");
        //a.Pomeri(Vector3.Forward, 55);

        
    }
    
}
