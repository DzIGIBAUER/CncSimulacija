using Godot;
using System;
using GCode;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public partial class Funkcije : RefCounted {

    private Masina _masina;

    private Mesh _pripremakMesh;

    public Funkcije(Masina masina) {
        _masina = masina;
    }


    private void LosParametar(ProgramskaRecenica recenica, KeyValuePair<Adresa, string> rec) {
        throw new GCodeException($"Parametar {rec.Key} vrednosti {rec.Value} nije validan.");
    }

    private void SpremiKretanjeAlata(ProgramskaRecenica recenica, Point target) {
        var pripremakCsg = _masina.SteznaGlava.Pripremak.GetNode<CsgMesh3D>("CsgMesh3D");
        if (_pripremakMesh == null) {
            _pripremakMesh = pripremakCsg.Mesh;
        }

        var alatCsg = _masina.Alat.GetNode<CsgMesh3D>("CsgMesh3D");
        var to = _masina.ToGlobal(_masina.RadniProstor.ConvertFrom(target));
        to = alatCsg.ToLocal(to);
        to = to + (alatCsg.Position / alatCsg.Scale);
        

        var extrudedAlat = alatCsg.Mesh.Extruded(to);
        var alatPolygon = extrudedAlat.ToConvexHull2D(alatCsg, Vector3.Zero, Vector3.Down);

        var pripremakPolygon = _pripremakMesh.ToConvexHull2D(
            pripremakCsg,
            Vector3.Zero,
            Vector3.Down
        );

        _masina.GetNode<Polygon2D>("/root/test/Polygon2D").Polygon = alatPolygon;
        _masina.GetNode<Polygon2D>("/root/test/Polygon2D2").Polygon = pripremakPolygon;

        for (int i = 0; i < pripremakPolygon.Length; i++) {
            if (pripremakPolygon[i].Y < pripremakCsg.GlobalPosition.Y) {
                pripremakPolygon[i].Y = pripremakCsg.GlobalPosition.Y;
            }
        }

        var resultPolygon = Geometry2D.ClipPolygons(
            pripremakPolygon,
            alatPolygon
        )[0];

        var otpad = Geometry2D.IntersectPolygons(
            pripremakPolygon,
            alatPolygon
        )[0];

        recenica.Data.ResultPart = resultPolygon.ExtrudeAroundConvexHull(
            pripremakCsg,
            Vector3.Zero,
            Vector3.Down
        );

        recenica.Data.ResultOtpad = otpad.ExtrudeAroundConvexHull(
            _masina.SteznaGlava.Pripremak.OtpadMeshInstance,
            Vector3.Zero,
            Vector3.Down
        );

        _masina.Alat.Position = _masina.ToGlobal(_masina.RadniProstor.ConvertFrom(target));

        _pripremakMesh = recenica.Data.ResultPart;
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
            
            SpremiKretanjeAlata(recenica, target);

            return;
        }

        //_masina.SteznaGlava.Pripremak.OtpadMeshInstance.Mesh = recenica.Data.ResultOtpad;

        _masina.SteznaGlava.Pripremak.GetNode<CsgMesh3D>("CsgMesh3D").Mesh = recenica.Data.ResultPart;
        
        _masina.PomeriAlat(target, paramF.Value.Number / 100f);

        await ToSignal(_masina.Alat, nameof(_masina.Alat.TargetReached));

    }


}