using Godot;

[Tool]
public class CSGSim : CSGCombiner {

    /// </summary> Dodaje 2 CSGMesh node-a na koja će uticati CSG operacije. </summary>
    public void Setup(CSGMesh m1, CSGMesh m2, OperationEnum operation) {
        m2.Operation = operation;
        m1.AddChild(m2);
        
        AddChild(m1);
    }

    /// </summary> Vraća transformaciju i ArrayMesh koji su rezultat operacije između CSGMesh-ova. </summary>
    public Godot.Collections.Array GetResult() => GetMeshes();

}