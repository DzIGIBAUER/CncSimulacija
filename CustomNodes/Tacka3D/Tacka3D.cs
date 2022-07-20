using Godot;


public class Tacka3D : Sprite3D {

    [Export(PropertyHint.Range, "0, 1")]
    public float Velicina = 1;

    private float _BasePixelSize = 0.01f;


    public override void _Process(float delta) {
        base._Process(delta);

        Camera aktivnaKamera = GetViewport().GetCamera();
        
        float distance = GlobalTransform.origin.DistanceTo(aktivnaKamera.GlobalTransform.origin);

        PixelSize = Velicina * distance * _BasePixelSize;
    }

    

}