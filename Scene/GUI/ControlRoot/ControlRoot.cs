using Godot;


/// <summary>
/// Node koji upravlja viewportom 3D scene i glavnim meniem.
/// </summary>
public partial class ControlRoot : Control {

	private MainMenu _mainMenu;

	public override void _Ready() {
		_mainMenu = GetNode<MainMenu>("MainMenu");
		
		
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
			GetViewport().SetInputAsHandled();
		}

		base._UnhandledInput(@event);
	}


	private void OnPripremakIzabran(Pripremak pripremak) {
		SteznaGlava steznaGlava = GetNode<SteznaGlava>("SubViewportContainer/SubViewport/Workspace/Masina/SteznaGlava");
		steznaGlava.Pripremak = pripremak;
	}

}
