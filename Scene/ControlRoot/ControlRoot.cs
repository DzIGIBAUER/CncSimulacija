using Godot;
using System;


/// <summary>
/// Node koji upravlja viewportom 3D scene i glavnim meniem.
/// </summary>
public class ControlRoot : Control {

    

    private MainMenu _MainMenu;


    public override void _Ready() {
        _MainMenu = (MainMenu)GetNode("MainMenu");
        
        
        
        base._Ready();
    }

    public override void _Input(InputEvent @event) {

        if (@event.IsActionPressed("ui_cancel")) {
            _MainMenu.Toggle();
        }

        base._Input(@event);
    }

    public override void _UnhandledInput(InputEvent @event) {

        if (_MainMenu.Visible) {
            GetTree().SetInputAsHandled();
        }

        base._UnhandledInput(@event);
    }

}
