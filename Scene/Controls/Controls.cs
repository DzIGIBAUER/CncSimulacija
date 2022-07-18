using Godot;

public class Controls : HBoxContainer {

    private Podesavanja PodesavanjaNode;

    public override void _Ready() {
        PodesavanjaNode = (Podesavanja)GetNode("/root/Podesavanja");

        HSlider kretanjeSlider = (HSlider)GetNode("VBoxContainer/KontroleOsetljivosti/Kretanje/HSlider");
        kretanjeSlider.Connect("value_changed", this, "OnOsetljivostKretanjaPromenjena");

        HSlider misSlider = (HSlider)GetNode("VBoxContainer/KontroleOsetljivosti/Mis/HSlider");
        misSlider.Connect("value_changed", this, "OnMisOsetljivosPromenjena");

        base._Ready();
    }

    private void OnOsetljivostKretanjaPromenjena(float novaOsetljivost) {
        PodesavanjaNode.OsetljivostKretanja = novaOsetljivost;
    }

    private void OnMisOsetljivosPromenjena(float novaOsetljivost) {
        PodesavanjaNode.OsetljivostMisa = novaOsetljivost;
    }

}