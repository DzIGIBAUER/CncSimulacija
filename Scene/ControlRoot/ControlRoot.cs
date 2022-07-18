using Godot;
using System;


/// <summary>
/// Node koji upravlja viewportom 3D scene i glavnim meniem.
/// </summary>
public class ControlRoot : Control {

    

    private MainMenu MainMenuNode;


    public override void _Ready() {
        MainMenuNode = (MainMenu)GetNode("MainMenu");
        
        
        
        base._Ready();
    }

    public override void _Input(InputEvent @event) {

        if (@event.IsActionPressed("ui_cancel")) {
            MainMenuNode.Toggle();
        }

        base._Input(@event);
    }

    public override void _UnhandledInput(InputEvent @event) {

        if (MainMenuNode.Visible) {
            GetTree().SetInputAsHandled();
        }

        base._UnhandledInput(@event);
    }


    public void PodesavanjaPromenjena() {
        
    }

}
