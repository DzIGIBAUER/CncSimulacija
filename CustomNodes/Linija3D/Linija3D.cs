using Godot;

[Tool]
public class Linija3D : MeshInstance {

    /// <summary>Signal koji je emit-ovan kada dodje to klika na Linija3D.</summary>
    [Signal]
    delegate void MouseClick(bool pressed);

    /// </summary>Vidljivost linje kada je u fokusu.</summary>
    static byte PrimaryAlpha = 255;
    /// </summary>Vidljivost linje kada nije u fokusu.</summary>
    static byte SecondaryAlpha = 100;

    private float _debljina = 7;

    /// <summary>Debljina linije.</summary>
    [Export(PropertyHint.Range, "0, 10, 0.1")]
    public float Debljina {
        get => _debljina;
        set {
            Scale = new Vector3(value*_scaleFactor, Duzina, value*_scaleFactor);
            _debljina = value;
        }
    }


    private float _duzina = 3;

    /// <summary>Dužina linije.</summary>
    [Export]
    public float Duzina {
        get => _duzina;
        set {
            Vector3 newScale = Scale;
            newScale.y = value;
            Scale = newScale;
            _duzina = value;
        }
    }


    /// <summary>Faktor kojim se množi debljina linije.</summary>
    private float _scaleFactor = 0.05f;


    public override void _Ready() {
        base._Ready();

        Area area = (Area)GetNode("Area");
        area.Connect("mouse_entered", this, "OnAreaMouseEnter");
        area.Connect("mouse_exited", this, "OnAreaMouseExit");
        area.Connect("input_event", this, "OnAreaInputEvent");
    }


    /// </summary>Kada kursor predje preko linije, pojačaćemo vidljivost linije.</summary>
    private void OnAreaMouseEnter() {
        SpatialMaterial material = (SpatialMaterial)GetActiveMaterial(0);
        material.AlbedoColor = Color.Color8(255, 255, 255, PrimaryAlpha);
    }

    /// </summary>Kada kursor predje preko linije, smanjićemo vidljivost linije.</summary>
    private void OnAreaMouseExit() {
        SpatialMaterial material = (SpatialMaterial)GetActiveMaterial(0);
        material.AlbedoColor = Color.Color8(255, 255, 255, SecondaryAlpha);
    }


    /// <summary>Sluša za svaki input_event na Area ove Linije3D.</summary>
    private void OnAreaInputEvent(Camera camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx) {

        if (@event is InputEventMouseButton mouseEvent) {
            EmitSignal(nameof(MouseClick), mouseEvent.Pressed);
        }

    }

}