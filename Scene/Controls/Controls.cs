using Godot;

public class Controls : HBoxContainer {
    

    private Global _Global;

    public override void _Ready() {

        _Global = (Global)GetNode("/root/Global");


        HSlider kretanjeSlider = (HSlider)GetNode("VBoxContainer/KontroleOsetljivosti/Kretanje/HSlider");
        kretanjeSlider.Connect("value_changed", this, "OnOsetljivostKretanjaPromenjena");

        HSlider misSlider = (HSlider)GetNode("VBoxContainer/KontroleOsetljivosti/Mis/HSlider");
        misSlider.Connect("value_changed", this, "OnMisOsetljivosPromenjena");
        
        base._Ready();
    }


    private void OnOsetljivostKretanjaPromenjena(float novaOsetljivost) {
        _Global.OstljivostKretanja = novaOsetljivost;
    }

    private void OnMisOsetljivosPromenjena(float novaOsetljivost) {
        _Global.OsetljivostMisa = novaOsetljivost;
    }


}