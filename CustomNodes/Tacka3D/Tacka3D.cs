using Godot;


/// <summary> Tačka koja je prikazana u 3D prostoru. </summary>
public partial class Tacka3D : Sprite3D {

    /// <summary> Veličina tačke. </summary>
    [Export(PropertyHint.Range, "0, 1")]
    public float Velicina = 1;

    /// <summary> Vrednost sa kojom se množi veličina, kako veličina ne bi morala da bude vrlo mala vrednost. </summary>
    private float _BasePixelSize = 0.01f;

    
    public override void _Process(double delta) {
        base._Process(delta);

        Camera3D aktivnaKamera = GetViewport().GetCamera3D();
        
        float distance = GlobalTransform.Origin.DistanceTo(aktivnaKamera.GlobalTransform.Origin);

        PixelSize = Velicina * distance * _BasePixelSize;
    }

    

}