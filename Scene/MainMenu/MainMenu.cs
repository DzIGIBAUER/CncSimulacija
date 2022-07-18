using Godot;

/// <summary>
/// Glavni meni prikazan prilikom ulaska u program.
///</summary>
public class MainMenu : Control {

    private ViewportContainer vpContainer;
	
    public override void _Ready() {
        // povezujemo signal za 'Razgledaj' dugme.
        Button razgledajDugme = (Button)GetNode("ContainerControl/HBoxContainer/MainMenuOptions/Razgledaj");
        razgledajDugme.Connect("pressed", this, "Toggle");
    }

    /// <summary>Sakriva/Prikazuje glavni meni i kursor.</summary>
    public void Toggle() {
        if (Visible) {
            Input.SetMouseMode(Input.MouseMode.Captured);
            Hide();
        } else {
            Input.SetMouseMode(Input.MouseMode.Visible);
            Show();
        }
    }

}