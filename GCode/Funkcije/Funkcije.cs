using Godot;
using System;
using GCode;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Funkcije : RefCounted {

    private Masina _masina;

    public Funkcije(Masina masina) {
        _masina = masina;
    }


    private void LosParametar(ProgramskaRecenica recenica, KeyValuePair<Adresa, string> rec) {
        throw new GCodeException($"Parametar {rec.Key} vrednosti {rec.Value} nije validan.");
    }

    private async Task SpremiKretanjeAlata(ProgramskaRecenica recenica, Point target) {
        // razvlacimo alat
        var alatCsgMesh = _masina.Alat.GetNode<CsgMesh3D>("CSGMesh3D");
        
        var to = _masina.RadniProstor.ConvertFrom(target) - _masina.Alat.Position;
        to = to / alatCsgMesh.Scale;
        var quat = new Quaternion(_masina.Alat.Basis.Inverse());
        to = quat * to;

        var result = await _masina.CSGSim.ResultingMesh(_masina.SteznaGlava.Pripremak, alatCsgMesh, to);

        recenica.Data.ResultPartGlobalTransform = (Transform3D)result[0];
        recenica.Data.ResultPart = (ArrayMesh)result[1];
    }


    public void M03(ProgramskaRecenica recenica, bool prepare) {
        var paramS = recenica.Param(Adresa.TYPE.SPEED, new ParamValue(1));

        if (prepare) return;

        _masina.SteznaGlava.BrzinaVretena = paramS.Value.Number;

    }

    public void M05(ProgramskaRecenica recenica, bool prepare) {
        if (prepare) return;

        _masina.SteznaGlava.BrzinaVretena = 0;
    }
    
    public async Task G01(ProgramskaRecenica recenica, bool prepare) {
        var paramX = recenica.Param(Adresa.TYPE.X, new ParamValue(0));
        var paramZ = recenica.Param(Adresa.TYPE.Z, new ParamValue(0));
        var paramF = recenica.Param(Adresa.TYPE.FEED_RATE, new ParamValue(0));

        var target = new Point(paramX.Value.Number / 100f, 0, paramZ.Value.Number / 100f);
        
        if (prepare) {
            
            await SpremiKretanjeAlata(recenica, target);

            return;
        }

        _masina.PomeriAlat(target, paramF.Value.Number / 100f);

        await ToSignal(_masina.Alat, nameof(_masina.Alat.TargetReached));

        _masina.SteznaGlava.Pripremak.Mesh = recenica.Data.ResultPart;
        _masina.SteznaGlava.Pripremak.GlobalTransform = recenica.Data.ResultPartGlobalTransform;

    }


}