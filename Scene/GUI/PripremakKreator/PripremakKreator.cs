using Godot;

/// <summary> GUI za kreiranje pripremka(biranje dužine, prečnika...). </summary>
public class PripremakKreator : Control {


    /// <summary> Signal koji se emituje kada je pripremak kreiran. </summary>
    [Signal]
    delegate void PripremakIzabran(CSGMesh pripremak);

    /// <summary> Pripremak kojim renutno manipulišemo. </summary>
    private CSGMesh _pripremak;

    /// <summary> Container node koju u sebi ima slider i label. </summaru>
    private VBoxContainer _precnikKontrola;
    
    /// <summary> Container node koju u sebi ima slider i label. </summaru>
    private VBoxContainer _duzinaKontrola;

    public override void _Ready() {
        base._Ready();

        _pripremak = GetNode<CSGMesh>("ViewportContainer/Viewport/Spatial/Pripremak");
        _precnikKontrola = GetNode<VBoxContainer>("HBoxContainer/Kontrole/Precnik");
        _duzinaKontrola = GetNode<VBoxContainer>("HBoxContainer/Kontrole/Duzina");        

        _precnikKontrola.GetNode("HSlider").Connect("value_changed", this, "OnPrecnikSliderValueChange");
        _duzinaKontrola.GetNode("HSlider").Connect("value_changed", this, "OnDuzinaSliderValueChange");

        Button izaberiDugme = GetNode<Button>("HBoxContainer/Kontrole/Izaberi/Button");
        izaberiDugme.Connect("pressed", this, "OnIzaberiDugmePressed");
    }

    private void OnPrecnikSliderValueChange(float value) {
        Label vrednostLabel = _precnikKontrola.GetNode<Label>("TextContainer/Vrednost");
        vrednostLabel.Text = value.ToString();
        
        Vector3 newScale = _pripremak.Scale;
        newScale.x = value;
        newScale.z = value;
        _pripremak.Scale = newScale;

    }

    private void OnDuzinaSliderValueChange(float value) {
        Label vrednostLabel = _duzinaKontrola.GetNode<Label>("TextContainer/Vrednost");
        vrednostLabel.Text = value.ToString();

        Vector3 newScale = _pripremak.Scale;
        newScale.y = value;
        _pripremak.Scale = newScale;
    }
    

    private void OnIzaberiDugmePressed() {
        EmitSignal(nameof(PripremakIzabran), _pripremak);
    }

}