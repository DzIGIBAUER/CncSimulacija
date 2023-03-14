using Godot;

/// <summary>Control sa GUI postavkama.</summary>
public partial class PostavkeControl : Panel {
	

    public override void _Ready() {
        base._Ready();

        Button otkaziDugme = GetNode<Button>("VBoxContainer/HBoxContainer/Otkazi");
        otkaziDugme.Connect("pressed",new Callable(this,"OnOtkaziDugmePressed"));

    }


    private void OnOtkaziDugmePressed() {
        Hide();
    }

}