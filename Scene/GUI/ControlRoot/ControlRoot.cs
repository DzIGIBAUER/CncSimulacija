using Godot;
using System;


/// <summary>
/// Node koji upravlja viewportom 3D scene i glavnim meniem.
/// </summary>
public class ControlRoot : Control {

    private MainMenu _mainMenu;

    private PripremakKreator _kreator;

    public override void _Ready() {
        _mainMenu = (MainMenu)GetNode("MainMenu");
        
        _kreator = (PripremakKreator)GetNode("PripremakKreator");
        _kreator.Connect("PripremakIzabran", this, "OnPripremakIzabran", null, (uint)ConnectFlags.Oneshot);
        
        base._Ready();
    }

    public override void _Input(InputEvent @event) {

        if (@event.IsActionPressed("ui_cancel")) {
            _mainMenu.Toggle();
        }

        base._Input(@event);
    }

    public override void _UnhandledInput(InputEvent @event) {

        if (_mainMenu.Visible) {
            GetTree().SetInputAsHandled();
        }

        base._UnhandledInput(@event);
    }


    private void OnPripremakIzabran(CSGMesh pripremak) {
        SteznaGlava steznaGlava = (SteznaGlava)GetNode("ViewportContainer/Viewport/Workspace/Masina/SteznaGlava");
        steznaGlava.Pripremak = pripremak;
    }

}
