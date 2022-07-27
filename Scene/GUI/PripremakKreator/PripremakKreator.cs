using Godot;
using Godot.Collections;

public class PripremakKreator : Control {


    [Signal]
    delegate void PripremakIzabran(CSGMesh pripremak);

    private CSGMesh _pripremak;
    private VBoxContainer _precnikKontrola;
    private VBoxContainer _duzinaKontrola;

    public override void _Ready() {
        base._Ready();

        _pripremak = (CSGMesh)GetNode("ViewportContainer/Viewport/Spatial/Pripremak");
        _precnikKontrola = (VBoxContainer)GetNode("HBoxContainer/Kontrole/Precnik");
        _duzinaKontrola = (VBoxContainer)GetNode("HBoxContainer/Kontrole/Duzina");        

        _precnikKontrola.GetNode("HSlider").Connect("value_changed", this, "OnPrecnikSliderValueChange");
        _duzinaKontrola.GetNode("HSlider").Connect("value_changed", this, "OnDuzinaSliderValueChange");

        Button izaberiDugme = (Button)GetNode("HBoxContainer/Kontrole/Izaberi/Button");
        izaberiDugme.Connect("pressed", this, "OnIzaberiDugmePressed");
    }

    private void OnPrecnikSliderValueChange(float value) {
        Label vrednostLabel = (Label)_precnikKontrola.GetNode("TextContainer/Vrednost");
        vrednostLabel.Text = value.ToString();
        
        Vector3 newScale = _pripremak.Scale;
        newScale.x = value;
        newScale.z = value;
        _pripremak.Scale = newScale;

    }

    private void OnDuzinaSliderValueChange(float value) {
        Label vrednostLabel = (Label)_duzinaKontrola.GetNode("TextContainer/Vrednost");
        vrednostLabel.Text = value.ToString();

        Vector3 newScale = _pripremak.Scale;
        newScale.y = value;
        _pripremak.Scale = newScale;
    }
    

    private void OnIzaberiDugmePressed() {
        EmitSignal(nameof(PripremakIzabran), _pripremak);
    }

}