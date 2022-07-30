using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class RadniProstor : Node {

    /// <summary> Početak radnog prostora(mašinska nulta tačka). </summary>
    [Export] private NodePath _startNodePath;
    
    /// <summary> Kraj radnog prostora(referentna tačka). </summary>
    [Export] private NodePath _endNodePath;

    /// <summary> Node čija pozicija označava inicijalni origin radnog prostora. </summary>
    [Export] private NodePath _originPath;


    /// <summary> Relacija radnog prostora mašine i koordinatnog sistema pogona. </summary>
    private Dictionary<Vector3.Axis, Vector3.Axis> _relacija = new Dictionary<Vector3.Axis, Vector3.Axis>(3) {
        [Vector3.Axis.Z] = Vector3.Axis.X,
        [Vector3.Axis.X] = Vector3.Axis.Y,
        [Vector3.Axis.Y] = Vector3.Axis.Z,
    };
    /// <summary> Obrnuta relacija radnog prostora mašine i koordinatnog sistema pogona. </summary>
    private Dictionary<Vector3.Axis, Vector3.Axis> _obrnutaRelacija; // namesteno u _EnterTree


    /// <summary> Origin spatial radnog prostora. </summary>
    public Spatial Origin {get; set;}

    /// <summary> Axis-alinged bounding box ovog radnog prostora. </summary>
    public AABB Aabb {get; set;}


    private Masina _masina;


    public override void _EnterTree() {
        base._EnterTree();

        _obrnutaRelacija = _relacija.ToDictionary((i) => i.Value, (i) => i.Key);

        _masina = GetParent<Masina>();

        Vector3 start, end;

        if (_startNodePath.IsEmpty()) {
            throw new ExportParametarNullException(_startNodePath);
        } else {
            start = GetNode<Spatial>(_startNodePath).Translation;
        }

        if (_endNodePath.IsEmpty()) {
            throw new ExportParametarNullException(_endNodePath);
        } else {
            end = GetNode<Spatial>(_endNodePath).Translation;
        }

        if (_originPath.IsEmpty()) {
            throw new ExportParametarNullException(_originPath);
        } else {
            Origin = GetNode<Spatial>(_originPath);
        }

        Vector3 size = end-start;
        size.z = 0.001f;
        GD.Print(size);
        Aabb = new AABB(start, size);
    }

    /// <summary> Pretvara koordinate mašine u koordinate radnog prostora mašine. </summary>
    public GCode.Point ConvertTo(Vector3 masinaCoord) {
        // koliko je coord udaljen od origin
        Vector3 coord = _masina.ToGlobal(masinaCoord) - Origin.Transform.origin;

        // preuredjujemo koordinate.
        return new GCode.Point(
            coord[(int)_relacija[Vector3.Axis.X]],
            coord[(int)_relacija[Vector3.Axis.Y]],
            coord[(int)_relacija[Vector3.Axis.Z]]
        );
    }


    /// <summary> Pretvara koordinate radnog prostora mašine u koordinate mašine. </summary>
    public Vector3 ConvertFrom(GCode.Point coord) {

        // redjamo koordinate nazad u prvobitni redosled.
        Vector3 poredjaneCoord = new Vector3(
            coord[(int)_obrnutaRelacija[Vector3.Axis.X]],
            coord[(int)_obrnutaRelacija[Vector3.Axis.Y]],
            coord[(int)_obrnutaRelacija[Vector3.Axis.Z]]
        );

        Vector3 masinaCoord = Origin.Transform.origin + _masina.ToLocal(poredjaneCoord);
        return masinaCoord;
    }

    /// <summary> Da li se dati point nalazi u radnom prostoru. </summary>
    /// prebacuje ga u vector3(preuredjuje ose), uzima xform(sta god to bilo lol) kako bi koordianta bila u odnosu na origin(gde krece aabb)
    //! Ovaj kod je vrlo opasan i moze izavati nulkearni karastih.
    public bool HasPoint(GCode.Point point) => Aabb.HasPoint( Origin.Transform.Xform(ConvertFrom(point)) );

}
