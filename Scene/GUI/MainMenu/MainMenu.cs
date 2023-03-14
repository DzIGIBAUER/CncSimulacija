using Godot;

/// <summary>
/// Glavni meni prikazan prilikom ulaska u program.
///</summary>
public partial class MainMenu : Control {
	
    public override void _Ready() {
        // povezujemo signal za 'Razgledaj' dugme.
        Button razgledajDugme = GetNode<Button>("ContainerControl/HBoxContainer/MainMenuOptions/Razgledaj");
        razgledajDugme.Connect("pressed",new Callable(this,"Toggle"));

        // povezujemo signal za 'Postavke' dugme.
        Button postavkeDugme = GetNode<Button>("ContainerControl/HBoxContainer/MainMenuOptions/Postavke");
        postavkeDugme.Connect("pressed",new Callable(this,"OnPostavkeDugmePressed"));
    }


    public override void _Input(InputEvent @event) {
        base._Input(@event);

        if (Visible && @event is InputEventKey) {
            GetViewport().SetInputAsHandled();
        }
    }

    /// <summary>Sakriva/Prikazuje glavni meni i kursor.</summary>
    public void Toggle() {
        if (Visible) {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            Hide();
        } else {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            Show();
        }
    }



    private void OnPostavkeDugmePressed(){
        PostavkeControl postavkeControl = (PostavkeControl)GetNode("PostavkeControl");
        postavkeControl.Show();
    }

}