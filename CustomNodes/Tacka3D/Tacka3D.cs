using Godot;


/// <summary> Tačka koja je prikazana u 3D prostoru. </summary>
public class Tacka3D : Sprite3D {

    /// <summary> Veličina tačke. </summary>
    [Export(PropertyHint.Range, "0, 1")]
    public float Velicina = 1;

    /// <summary> Vrednost sa kojom se množi veličina, kako veličina ne bi morala da bude vrlo mala vrednost. </summary>
    private float _BasePixelSize = 0.01f;

    
    public override void _Process(float delta) {
        base._Process(delta);

        Camera aktivnaKamera = GetViewport().GetCamera();
        
        float distance = GlobalTransform.origin.DistanceTo(aktivnaKamera.GlobalTransform.origin);

        PixelSize = Velicina * distance * _BasePixelSize;
    }

    

}