using Godot;

/// <sumamry>
/// Kontroler koji nam omogućava free look kako bi se kretali u prostoru.
///</summary>
public class Kontroler : Camera {

    /// <summary>Koliko će se kamera pomeriti u jednom frame-u, pomnoženo sa delta.</summary>
    public float MoveFactor = 0.25f;

    /// <summary>Količina pokreta miša je pretvorena u radijane i pomnožena sa ovom vrednošću.</summary>
    public float RotateFactor = 0.5f;


    /// <summary> Kretanje koji treba biti primenjena na kontroleru. </summary>
    private Vector3 _motion = Vector3.Zero;


    public override void _PhysicsProcess(float delta) {
        base._PhysicsProcess(delta);

        TranslateObjectLocal(_motion * MoveFactor * delta);
    }


    public override void _UnhandledInput(InputEvent @event) {
        base._UnhandledInput(@event);

        if (@event is InputEventMouseMotion mouseMotionEvent) {
            // Pokret misa nam daje Vector2 cija x komponenta je horizontalna, a y vertikalna
            // Pokret po x osi je rotiranje kamere oko y ose, a pokret po y je rotiranje po x osi.
            // Kamera se ne rotira po z osi, nema potrebe da radi barell roll.

            Vector3 rotateAmount = new Vector3(-Mathf.Deg2Rad(mouseMotionEvent.Relative.y), -Mathf.Deg2Rad(mouseMotionEvent.Relative.x), 0);
            rotateAmount.x = Mathf.Clamp(rotateAmount.x, Mathf.Deg2Rad(-90), Mathf.Deg2Rad(90));

            Rotation += rotateAmount * RotateFactor;

        } else if (@event is InputEventKey keyEvent) {

            if (keyEvent.IsActionPressed("napred")) {
                _motion += Vector3.Forward;
            } else if (keyEvent.IsActionReleased("napred")) {
                _motion -= Vector3.Forward;
            }

            if (keyEvent.IsActionPressed("nazad")) {
                _motion += Vector3.Back;
            } else if (keyEvent.IsActionReleased("nazad")) {
                _motion -= Vector3.Back;
            }

            if (keyEvent.IsActionPressed("levo")) {
                _motion += Vector3.Left;
            } else if (keyEvent.IsActionReleased("levo")) {
                _motion -= Vector3.Left;
            }

            if (keyEvent.IsActionPressed("desno")) {
                _motion += Vector3.Right;
            } else if (keyEvent.IsActionReleased("desno")) {
                _motion -= Vector3.Right;
            }

        }
    }

}