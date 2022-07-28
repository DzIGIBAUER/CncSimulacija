using Godot;

/// <summary>Control sa GUI postavkama.</summary>
public class PostavkeControl : Panel {
	

    public override void _Ready() {
        base._Ready();

        Button otkaziDugme = GetNode<Button>("VBoxContainer/HBoxContainer/Otkazi");
        otkaziDugme.Connect("pressed", this, "OnOtkaziDugmePressed");

    }


    private void OnOtkaziDugmePressed() {
        Hide();
    }

}