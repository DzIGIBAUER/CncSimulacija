using Godot;

/// <sumamry>
/// Kontroler koji nam omogućava free look kako bi se kretali u prostoru.
///</summary>
public class Kontroler : Camera {

    /// <summary>Koliko će se kamera pomeriti u jednom frame-u, pomnoženo sa delta.</summary>
    public float MoveFactor
    {
        get {return _Global.OstljivostKretanja;}
        private set {MoveFactor = value;}
    }

    /// <summary>Količina pokreta miša je pretvorena u radijane i pomnožena sa ovom vrednošću.</summary>
    public float RotateFactor
    {
        get {return _Global.OsetljivostMisa;}
        private set {RotateFactor = value;}
    }

    private Global _Global;

    private Vector3 _MoveMotion;

    public static float MinMoveFactor = 0.25f;
    public static float MinRotateFactor = 0.2f;

    public override void _Ready() {
        base._Ready();

        _Global = (Global)GetNode("/root/Global");
    }

    public override void _Process(float delta) {
        if (!GetTree().IsInputHandled()) {
            ProcessMovement(delta);
        }
        
        base._PhysicsProcess(delta);
    }


    private void ProcessMovement(float delta) {
        if (Input.IsActionJustPressed("napred")) {
            _MoveMotion += Vector3.Forward;

        } else if (Input.IsActionJustReleased("napred")) {
            _MoveMotion -= Vector3.Forward;
        }

        if (Input.IsActionJustPressed("nazad")) {
            _MoveMotion += Vector3.Back;
        
        } else if (Input.IsActionJustReleased("nazad")) {
            _MoveMotion -= Vector3.Back;
        }

        if (Input.IsActionJustPressed("levo")) {
            _MoveMotion += Vector3.Left;
        
        } else if (Input.IsActionJustReleased("levo")) {
            _MoveMotion -= Vector3.Left;
        }

        if (Input.IsActionJustPressed("desno")) {
            _MoveMotion += Vector3.Right;
        
        } else if (Input.IsActionJustReleased("desno")) {
            _MoveMotion -= Vector3.Right;
        }

        TranslateObjectLocal(_MoveMotion * MoveFactor * delta);
    }


    public override void _UnhandledInput(InputEvent @event) {
        if (@event is InputEventMouseMotion mouseMotionEvent) {
            // Pokret misa nam daje Vector2 cija x komponenta je horizontalna, a y vertikalna
            // Pokret po x osi je rotiranje kamere oko y ose, a pokret po y je rotiranje po x osi.
            // Kamera se ne rotira po z osi, nema potrebe da radi barell roll.

            Vector3 rotateAmount = new Vector3(-Mathf.Deg2Rad(mouseMotionEvent.Relative.y), -Mathf.Deg2Rad(mouseMotionEvent.Relative.x), 0);
            rotateAmount *= RotateFactor;
            rotateAmount.x = Mathf.Clamp(rotateAmount.x, Mathf.Deg2Rad(-90), Mathf.Deg2Rad(90));

            Rotation += rotateAmount;
        }

        base._UnhandledInput(@event);
    }

}