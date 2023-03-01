using Godot;
using System;
using GCode;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Funkcije : Reference {

    private Masina _masina;

    public Funkcije(Masina masina) {
        _masina = masina;
    }


    private void LosParametar(ProgramskaRecenica recenica, KeyValuePair<Adresa, string> rec) {
        throw new GCodeException($"Parametar { rec.Key } vrednosti { rec.Value } nije validan.");
    }

    private async Task SpremiKretanjeAlata(ProgramskaRecenica recenica, Point target) {


        var csgMesh = _masina.Alat.GetNode<CSGMesh>("CSGMesh");
        
        _masina.CSGSim.Setup(_masina.SteznaGlava.Pripremak, csgMesh, CSGShape.OperationEnum.Subtraction);
        
        // neka CSGSim ima signal
        await ToSignal(_masina.GetTree().CreateTimer(1), "timeout");
        var results = _masina.CSGSim.GetResult();
        
        var resultMesh = (ArrayMesh)results[1];

        var to = _masina.RadniProstor.ConvertFrom(target) - _masina.Alat.Translation;
        to = to / csgMesh.Scale;
        var quat = new Quat(-_masina.Alat.Rotation);
        to = quat.Xform(to);

        var extrudedMesh = csgMesh.Mesh
            .Extruded(to)
            .ExtrudedAround(Vector3.Zero, Vector3.Axis.Y, 360);

        recenica.Data.ExtrudedMesh = extrudedMesh;
        recenica.Data.ResultPart = resultMesh;
        GD.Print("SKA");
    }

    public async void M03(ProgramskaRecenica recenica, bool prepare = false) {
        if (prepare) return;

        var paramS = recenica.Param(Adresa.TYPE.SPEED, new ParamValue(1));

        _masina.SteznaGlava.BrzinaVretena = paramS.Value.Number;

    }

    public async Task G01(ProgramskaRecenica recenica, bool prepare = false) {
        var paramX = recenica.Param(Adresa.TYPE.X, new ParamValue(0));
        var paramZ = recenica.Param(Adresa.TYPE.Z, new ParamValue(0));
        var paramF = recenica.Param(Adresa.TYPE.FEED_RATE, new ParamValue(0));

        var target = new Point(paramX.Value.Number / 100, 0, paramZ.Value.Number / 100);

        /*if (!_masina.RadniProstor.HasPoint(target)) {
            throw new GCodeException($"Cilja tačka { target } je izvan radnog prostora.", recenica);
        }*/

        GD.Print(prepare);
        if (prepare) {

            await SpremiKretanjeAlata(recenica, target);

            return;
        }

        
        _masina.PomeriAlat(target, paramF.Value.Number);
        GD.Print("pomeren");
        await ToSignal(_masina.Alat, "TargetReached");
        GD.Print("ALAT STIGAO");
        GD.Print(recenica.Data.ResultPart);
    }


}