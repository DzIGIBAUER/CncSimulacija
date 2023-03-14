using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RadniProstor : Node {

    /// <summary> Početak radnog prostora(mašinska nulta tačka). </summary>
    [Export] private Node3D _startNode;
    
    /// <summary> Kraj radnog prostora(referentna tačka). </summary>
    [Export] private Node3D _endNode;

    /// <summary> Node čija pozicija označava inicijalni origin radnog prostora. </summary>
    [Export] public Node3D Origin { get; set; }


    /// <summary> Relacija radnog prostora mašine i koordinatnog sistema pogona. </summary>
    private Dictionary<Vector3.Axis, Vector3.Axis> _relacija = new Dictionary<Vector3.Axis, Vector3.Axis>(3) {
        [Vector3.Axis.Z] = Vector3.Axis.X,
        [Vector3.Axis.X] = Vector3.Axis.Y,
        [Vector3.Axis.Y] = Vector3.Axis.Z,
    };
    /// <summary> Obrnuta relacija radnog prostora mašine i koordinatnog sistema pogona. </summary>
    private Dictionary<Vector3.Axis, Vector3.Axis> _obrnutaRelacija; // namesteno u _EnterTree


    /// <summary> Axis-alinged bounding box ovog radnog prostora. </summary>
    public Aabb Aabb {get; set;}


    private Masina _masina;


    public override void _EnterTree() {
        base._EnterTree();

        _obrnutaRelacija = _relacija.ToDictionary((i) => i.Value, (i) => i.Key);

        _masina = GetParent<Masina>();

        Aabb = new Aabb(_startNode.Position, _endNode.Position-_startNode.Position);
    }

    /// <summary> Pretvara koordinate mašine u koordinate radnog prostora mašine. </summary>
    public GCode.Point ConvertTo(Vector3 masinaCoord) {
        // koliko je coord udaljen od origin
        Vector3 coord = _masina.ToGlobal(masinaCoord) - Origin.Transform.Origin;

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

        Vector3 masinaCoord = Origin.Transform.Origin + _masina.ToLocal(poredjaneCoord);
        return masinaCoord;
    }

}
