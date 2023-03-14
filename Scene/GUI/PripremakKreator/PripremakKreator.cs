using Godot;

/// <summary> GUI za kreiranje pripremka(biranje dužine, prečnika...). </summary>
public partial class PripremakKreator : Control {


    /// <summary> Signal koji se emituje kada je pripremak kreiran. </summary>
    [Signal]
    public delegate void PripremakIzabranEventHandler(CsgMesh3D pripremak);

    /// <summary> Pripremak kojim renutno manipulišemo. </summary>
    private CsgMesh3D _pripremak;

    /// <summary> Container node koju u sebi ima slider i label. </summaru>
    private VBoxContainer _precnikKontrola;
    
    /// <summary> Container node koju u sebi ima slider i label. </summaru>
    private VBoxContainer _duzinaKontrola;

    public override void _Ready() {
        base._Ready();

        _pripremak = GetNode<CsgMesh3D>("SubViewportContainer/SubViewport/Node3D/Pripremak");
        _precnikKontrola = GetNode<VBoxContainer>("HBoxContainer/Kontrole/Precnik");
        _duzinaKontrola = GetNode<VBoxContainer>("HBoxContainer/Kontrole/Duzina");        

        _precnikKontrola.GetNode("HSlider").Connect("value_changed",new Callable(this,"OnPrecnikSliderValueChange"));
        _duzinaKontrola.GetNode("HSlider").Connect("value_changed",new Callable(this,"OnDuzinaSliderValueChange"));

        Button izaberiDugme = GetNode<Button>("HBoxContainer/Kontrole/Izaberi/Button");
        izaberiDugme.Connect("pressed",new Callable(this,"OnIzaberiDugmePressed"));
    }

    private void OnPrecnikSliderValueChange(float value) {
        Label vrednostLabel = _precnikKontrola.GetNode<Label>("TextContainer/Vrednost");
        vrednostLabel.Text = value.ToString();
        
        Vector3 newScale = _pripremak.Scale;
        newScale.X = value;
        newScale.Z = value;
        _pripremak.Scale = newScale;

    }

    private void OnDuzinaSliderValueChange(float value) {
        Label vrednostLabel = _duzinaKontrola.GetNode<Label>("TextContainer/Vrednost");
        vrednostLabel.Text = value.ToString();

        Vector3 newScale = _pripremak.Scale;
        newScale.Y = value;
        _pripremak.Scale = newScale;
    }
    

    private void OnIzaberiDugmePressed() {
        EmitSignal(nameof(PripremakIzabran), _pripremak);
    }

}