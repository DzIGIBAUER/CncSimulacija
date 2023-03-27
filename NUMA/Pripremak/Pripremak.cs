using Godot;
using System.Collections.Generic;
using System;

public partial class Pripremak : Node3D {

	public MeshInstance3D OtpadMeshInstance { set; get; }
	
	private Masina _masina;

	private MeshDataTool _otpadMeshMdt;

	public override void _Ready() {
		base._Ready();

		_masina = GetParent().GetParent<Masina>();

        OtpadMeshInstance = new MeshInstance3D();

        AddChild(OtpadMeshInstance);

    }

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

    }

}
