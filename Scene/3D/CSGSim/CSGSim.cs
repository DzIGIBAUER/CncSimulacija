using Godot;
using System.Threading.Tasks;

public partial class CSGSim : CsgCombiner3D {

    [Signal]
	public delegate void ResultReadyEventHandler();

    public async Task<Godot.Collections.Array> ResultingMesh(CsgMesh3D pripremak, CsgMesh3D alat, Vector3 to) {
        var pripremakDup = (CsgMesh3D)pripremak.Duplicate();
        //var pripremakDup = pripremak.Clone<CsgMesh3D>(this);
        var alatDup = (CsgMesh3D)alat.Duplicate();
        //var alatDup = alat.Clone<CsgMesh3D>(this);


        var pointA = alat.ToLocal(pripremak.GlobalTransform.Origin);
        var pointB = alat.ToLocal(pripremak.GlobalTransform.Origin + Vector3.Right);

        alatDup.Mesh = alat.Mesh
            .Extruded(to)
            .ExtrudedAround(new Vector3(0, 0, 0), new Vector3(0, -1, 0));


        GD.Print(ResourceSaver.Save(alatDup.Mesh, "E:\\ProjectsGodot\\CncSimulacija\\alobre.tres"));
        GD.Print(ResourceSaver.Save(pripremakDup.Mesh, "E:\\ProjectsGodot\\CncSimulacija\\pripbre.tres"));

        alatDup.Mesh = GD.Load<Mesh>("E:\\ProjectsGodot\\CncSimulacija\\alobre.tres");
        pripremakDup.Mesh = GD.Load<Mesh>("E:\\ProjectsGodot\\CncSimulacija\\pripbre.tres");

        AddChild(pripremakDup);
        pripremakDup.AddChild(alatDup);

        pripremakDup.GlobalTransform = pripremak.GlobalTransform;
        alatDup.GlobalTransform = alat.GlobalTransform;
        //alatDup.Operation = OperationEnum.Subtraction;

        Call("_update_shape");

        //await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        return GetMeshes();
    }

    /// </summary> Dodaje 2 CsgMesh3D node-a na koja su zatim klonirana i koja će uticati CSG operacije. </summary>
    public void Setup(CsgMesh3D m1, CsgMesh3D m2, OperationEnum operation) {

        var m1GlobalTransform = m1.GlobalTransform;
        var m2GlobalTransform = m2.GlobalTransform;
        
        var m1Dup = (CsgMesh3D)m1.Duplicate();
        var m2Dup = (CsgMesh3D)m2.Duplicate();

        m1Dup.GlobalTransform = m1GlobalTransform;
        m2Dup.GlobalTransform = m2GlobalTransform;

        AddChild(m1Dup);
        AddChild(m2Dup);

        //m2Dup.Operation = operation;
    }

    /// </summary> Vraća transformaciju i ArrayMesh koji su rezultat operacije između CSGMesh3D-ova. </summary>
    public Godot.Collections.Array GetData() => GetMeshes();

}