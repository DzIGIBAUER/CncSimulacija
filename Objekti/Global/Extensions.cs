using Godot; 
using Godot.Collections;

/// <summary>
/// Static class sa svim extension metodama.
/// </summary>
static class ExtensionMethods {

    /// <summary>
    /// Pretvara mesh u arrayMesh.
    /// </summary>
    /// <param name="mesh">Mesh koji pretvaramo.</param>
    /// <param name="primitiveType">Mesh primtitive type.</param>
    /// <returns>Konstruisan arrayMesh.</returns>
    public static ArrayMesh asArrayMesh(this Mesh mesh, Mesh.PrimitiveType primitiveType = Mesh.PrimitiveType.Triangles) {
        var arrMesh = new ArrayMesh();
        
        for (int i = 0; i < mesh.GetSurfaceCount(); i++) {
            var surfaceArrays = mesh.SurfaceGetArrays(i);
            arrMesh.AddSurfaceFromArrays(primitiveType, surfaceArrays);
        }

        return arrMesh;
    }

    /// <summary>
    /// Uzimajući face-a i 2 vertexa tog face-a nazad vraća index edge-a koji dele ta 2 vertexa, ili -1 ako nije pronađen.
    /// </summary>
    /// <param name="mdt">Mesh Data Tool sa informacijama o mesh-u.</param>
    /// <param name="faceIndex">Index face-a gde se nalaze vertexi.</param>
    /// <param name="vertexOneIndex">Index prvog vertexa.</param>
    /// <param name="vertexTwoIndex">Index drugog vertexa.</param>
    /// <returns>Indeg edge-a, ili -1 ako nije pronađen.</returns>
    public static int GetFaceEdgeBeetwen(this MeshDataTool mdt, int faceIndex, int vertexOneIndex, int vertexTwoIndex) {
        
        for (int i = 0; i < 3; i ++) {
            int edgeIndex = mdt.GetFaceEdge(faceIndex, i);

            int edgeVertexOneIndex = mdt.GetEdgeVertex(edgeIndex, 0);
            int edgeVertexTwoIndex = mdt.GetEdgeVertex(edgeIndex, 1);

            if (
                (edgeVertexOneIndex == vertexOneIndex || edgeVertexOneIndex == vertexTwoIndex) &&
                (edgeVertexTwoIndex == vertexOneIndex || edgeVertexTwoIndex == vertexTwoIndex)
            ) return edgeIndex;
        }

        return -1;
    }

    /// <summary>
    /// Duplira node i sve njegove osobine.
    /// </summary>
    /// <typeparam name="T">Tip node-a koji kloniramo.</typeparam>
    /// <param name="node">Node koji kloniramo.</param>
    /// <param name="parent">Koji node će biti parent kloniranog node-a.</param>
    /// <returns>Klonirani node.</returns>
    public static T Clone<T>(this Node node, Node parent) where T: Node {
        var clonedNode = node.Duplicate();
        parent.AddChild(clonedNode);

        foreach(Dictionary prop in node.GetPropertyList()) {
            string propName = prop["name"] as string;
            var propValue = node.Get(propName);

            clonedNode.Set(propName, propValue);
        }

        return clonedNode as T;
    }

}