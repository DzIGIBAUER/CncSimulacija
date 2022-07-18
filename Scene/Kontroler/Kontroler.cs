using Godot;

/// <sumamry>
/// Kontroler koji nam omogućava free look kako bi se kretali u prostoru.
///</summary>
public class Kontroler : Camera {

    /// <summary>Koliko će se kamera pomeriti u jednom frame-u, pomnoženo sa delta.</summary>
    public float MoveFactor
    {
        get {return PodesavanjaNode.OsetljivostKretanja;}
        private set {}
    }

    /// <summary>Količina pokreta miša je pretvorena u radijane i pomnožena sa ovom vrednošću.</summary>
    public float RotateFactor
    {
        get {return PodesavanjaNode.OsetljivostMisa;}
        private set {}
    }


    private Podesavanja PodesavanjaNode;

    public static float MinMoveFactor = 0.25f;
    public static float MinRotateFactor = 0.2f;

    public override void _Ready() {

        PodesavanjaNode = (Podesavanja)GetNode("/root/Podesavanja");

        base._Ready();
    }

    public override void _Process(float delta) {

        if (!GetTree().IsInputHandled()) {
            ProcessMovement(delta);
        }
        
        base._PhysicsProcess(delta);
    }


    private void ProcessMovement(float delta) {
        Vector3 moveVector = Vector3.Zero;

        if (Input.IsActionPressed("napred", true)) {
            moveVector += Vector3.Forward;
        }

        if (Input.IsActionPressed("nazad", true)) {
            moveVector += Vector3.Back;
        }

        if (Input.IsActionPressed("levo", true)) {
            moveVector += Vector3.Left;
        }

        if (Input.IsActionPressed("desno", true)) {
            moveVector += Vector3.Right;
        }

        TranslateObjectLocal(moveVector * MoveFactor * delta);
    }


    public override void _UnhandledInput(InputEvent @event) {
        if (@event is InputEventMouseMotion mouseMotionEvent) {
            // Pokret misa nam daje Vector2 cija x komponenta je horizontalna, a y vertikalna
            // Pokret po x osi je rotiranje kamere oko y ose, a pokret po y je rotiranje po x osi.
            // Kamera se ne rotira po z osi, nema potrebe da radi barell roll.

            Vector3 rotateAmount = new Vector3(-Mathf.Deg2Rad(mouseMotionEvent.Relative.y), -Mathf.Deg2Rad(mouseMotionEvent.Relative.x), 0);
            rotateAmount.x = Mathf.Clamp(rotateAmount.x, Mathf.Deg2Rad(-90), Mathf.Deg2Rad(90));

            Rotation += rotateAmount * RotateFactor;
        }

        base._UnhandledInput(@event);
    }

}