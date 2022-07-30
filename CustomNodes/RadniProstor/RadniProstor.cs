using Godot;
using System;
using System.Collections.Generic;

public class RadniProstor : Node {

    /// <summary> X osa radnog prostora. </summary>
    public static Vector3.Axis X = Vector3.Axis.Y;

    /// <summary> Y osa radnog prostora. </summary>
    public static Vector3.Axis Y = Vector3.Axis.Z;

    /// <summary> Z osa radnog prostora. </summary>
    public static Vector3.Axis Z = Vector3.Axis.X;


    /// <summary> Početak radnog prostora(mašinska nulta tačka). </summary>
    [Export] private NodePath _startNodePath;
    
    /// <summary> Kraj radnog prostora(referentna tačka). </summary>
    [Export] private NodePath _endNodePath;

    /// <summary> Node cija pozicija označava inicijalni origin radnog prostora. </summary>
    [Export] private NodePath _originPath;

    /// <summary> Origin spatial radnog prostora. </summary>
    public Spatial Origin {get; set;}

    /// <summary> Axis-alinged bounding box ovog radnog prostora. </summary>
    public AABB Aabb {get; set;}


    private Masina _masina;


    public override void _EnterTree() {
        base._EnterTree();

        _masina = GetParent<Masina>();

        Vector3 start, end;

        if (_startNodePath.IsEmpty()) {
            throw new ArgumentNullException("Start Node Path", $"Export parametar node-a { Name } nije podešen.");
        } else {
            start = GetNode<Spatial>(_startNodePath).Translation;
        }

        if (_endNodePath.IsEmpty()) {
            throw new ArgumentNullException("End Node Path", $"Export parametar node-a { Name } nije podešen.");
        } else {
            end = GetNode<Spatial>(_endNodePath).Translation;
        }

        if (_originPath.IsEmpty()) {
            throw new ArgumentNullException("Origin Path", $"Export parametar node-a { Name } nije podešen.");
        } else {
            Origin = GetNode<Spatial>(_originPath);
        }

        Aabb = new AABB(start, end-start);
    }

    /// <summary> Pretvara koordinate mašine u koordinate radnog prostora mašine. </summary>
    public Vector3 ConvertTo(Vector3 masinaCoord) {
        // koliko je coord udaljen od origin
        Vector3 coord = _masina.ToGlobal(masinaCoord) - Origin.Transform.origin;

        // preuredjujemo koordinate.
        return new Vector3(
            coord[(int)X],
            coord[(int)Y],
            coord[(int)Z]
        );
    }


    /// <summary> Pretvara koordinate radnog prostora mašine u koordinate mašine. </summary>
    public Vector3 ConvertFrom(Vector3 coord) {

        // redjamo koordinate nazad u prvobitni redosled.
        //! ovo nece raditi ako se implementacija iznad promenmi.
        Vector3 poredjaneCoord = new Vector3(
            coord[(int)Vector3.Axis.Z],
            coord[(int)Vector3.Axis.X],
            coord[(int)Vector3.Axis.Y]
        );

        Vector3 masinaCoord = Origin.Transform.origin + _masina.ToLocal(poredjaneCoord);
        return masinaCoord;
    }

}
