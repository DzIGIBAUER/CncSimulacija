using Godot;


public class CSGSim : CSGCombiner {

    /// </summary> Dodaje 2 CSGMesh node-a na koja su zatim klonirana i koja će uticati CSG operacije. </summary>
    public void Setup(CSGMesh m1, CSGMesh m2, OperationEnum operation) {

        if (m1.GetParent() != null) {
            m1 = m1.Clone<CSGMesh>(this);
        }
        else {
            AddChild(m1);   
        }

        if (m2.GetParent() != null) {
            m2 = m2.Clone<CSGMesh>(this);
        }
        else {
            AddChild(m2);
        }
        
        m2.Operation = operation;
    }

    /// </summary> Vraća transformaciju i ArrayMesh koji su rezultat operacije između CSGMesh-ova. </summary>
    public Godot.Collections.Array GetResult() => GetMeshes();

}