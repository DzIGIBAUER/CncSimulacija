using Godot;
using System;

public partial class scene : Node3D {

	[Export] CsgCombiner3D Combiner;
	[Export] MeshInstance3D MeshNode;
	[Export] CsgPolygon3D Poly;
	[Export] CsgMesh3D AA;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {

		AA.Mesh = AA.Mesh.Extruded(new Vector3(0, 2, 0)).ExtrudedAround(new Vector3(0, -1, 0), new Vector3(0, 1, 0));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}
}
