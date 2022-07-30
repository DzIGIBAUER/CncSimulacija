using Godot;
using System;


/// <summary> Container Node za opciju koji emituje signale kada je opcija izmenjena. Exporti moraju da budu namešteni. </summary>
public class OpcijaContainer : GridContainer {

    /// <summary> Signal koji je emitovan kada je vrednost opcije promenjena. </summary>
    [Signal]
    delegate void OpcijaPromenjena(OpcijaEvent vrednost);

    [Export]
    private NodePath _interaktivnaKontrola;
    /// <summary> Control Node koji emituje signal kada ga korisnik izmeni. </summary>
    public Control InteraktivnaKontrola;
    
    [Export]
    private NodePath _vrednostLabel;
    /// <summary> Label ciji tekst ce se promeniti kada se vrednost interaktivne kontrole izmeni. (Opcionalno) </summary>
    public Label VrednostLabel;

    /// <summary> Naziv signala koji interaktiva kontrola emituje kada je njegova vrednost izmenjena. </summary>
    [Export]
    public string NazivSignala;

    /// <summary> Koji od parametara signala treba da bude tretiran kao vrednost opcije. </summary>
    [Export]
    public int SignalParametar;

    /// <summary> Node koji dobija signale promene vrednosti opcije. </summary>
    private Node _signalHandler;


    public override void _Ready() {
        base._Ready();

        GDScript script = GD.Load<GDScript>("res://CustomNodes/OpcijaContainer/signal_handler.gd");
        _signalHandler = (Node)script.New();
        AddChild(_signalHandler);
        

        if (_interaktivnaKontrola != null) {
            InteraktivnaKontrola = GetNode<Control>(_interaktivnaKontrola);
        } else {
            throw new ExportParametarNullException(_interaktivnaKontrola);
        }
        
        if (_vrednostLabel != null) {
            VrednostLabel = GetNode<Label>(_vrednostLabel);
        } // nema else, ovaj parametar je opcionalan.

        if (InteraktivnaKontrola.HasSignal(NazivSignala)) {
            InteraktivnaKontrola.Connect(NazivSignala, _signalHandler, "on_interaktivna_kontrola_value_change");
        
        } else {
            throw new ArgumentException($"Signal { NazivSignala } nije pronađen kao definisan signal node-a { Name }.", "Naziv Signala");
        }

    }

}
