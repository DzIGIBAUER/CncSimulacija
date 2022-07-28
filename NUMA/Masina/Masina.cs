using Godot;

public class Masina : MeshInstance {
	
    /// <summary>Radni prostor mašine.</summary>
    public AABB WorkZone {get; private set;}


    public override void _Ready() {
        base._Ready();

        InitWorkZone();
    }


    /// <summary>
    /// Računa i čuva radni prostor na osnovu mašinske nulte i referentne tačke.
    /// </summary>
    private void InitWorkZone() {
        Tacka3D nultaTackaMasine = GetNode<Tacka3D>("NultaTackaMasine");
        Tacka3D referentnaTackaMasine = GetNode<Tacka3D>("ReferentnaTackaMasine");

        Vector3 workZoneStartPos = nultaTackaMasine.Translation;
        Vector3 workZoneSize = referentnaTackaMasine.Translation - workZoneStartPos;

        WorkZone = new AABB(workZoneStartPos, workZoneSize);
    }

}