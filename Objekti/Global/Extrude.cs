using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;



public static class Extrude {

    private static ConditionalWeakTable<Mesh, MeshDataTool> _cache = new ConditionalWeakTable<Mesh, MeshDataTool>();


    public static ArrayMesh Extruded(this Mesh mesh, Vector3 to) {
        Vector3 dir = to.Normalized();

        var mdt = _cache.GetValue(mesh, GenerateMeshData);

        HashSet<int> toExtrude = FacesToExtrude(mdt, dir);

        var st = new SurfaceTool();
        st.Begin(Mesh.PrimitiveType.Triangles);

        /// Za svaki face koji treba da pomerimo
        foreach (int faceIndex in toExtrude) {
            // pomeramo svaki vertex na novu poziciju
            for (int i = 0; i < 3; i++) {
                int vertexIndex = mdt.GetFaceVertex(faceIndex, i);
                Vector3 vertex = mdt.GetVertex(vertexIndex);

                st.AddVertex(vertex + to);
            }

        }

        List<int> exclude = new List<int>();

        /// Za svaki face koji treba da ostane tamo gde jeste.
        for (int faceIndex = 0; faceIndex < mdt.GetFaceCount(); faceIndex++) {
            if (toExtrude.Contains(faceIndex)) continue;

            for (int i = 0; i < 3; i++) {
                int vertexIndex = mdt.GetFaceVertex(faceIndex, i);
                Vector3 vertex = mdt.GetVertex(vertexIndex);

                st.AddVertex(vertex);
            }

            /*for (int i = 0; i < 3; i++) {
                int vertexIndex = mdt.GetFaceVertex(faceIndex, i);
                Vector3 vertex = mdt.GetVertex(vertexIndex);

                st.AddVertex(vertex);
                st.AddVertex(vertex + to);
                int firstFaceThirdVertexIdx = FindClosestVertex(mdt, vertexIndex, toExtrude);
                Vector3 firstFaceThirdVertex = mdt.GetVertex( firstFaceThirdVertexIdx );

                firstFaceThirdVertex = (exclude.Contains(firstFaceThirdVertexIdx)) ? firstFaceThirdVertex + to : firstFaceThirdVertex;

                st.AddVertex(firstFaceThirdVertex);
                
                int secondFaceThirdVertexIdx = FindClosestVertex(mdt, vertexIndex, toExtrude, firstFaceThirdVertexIdx);
                Vector3 secondFaceThirdVertex = mdt.GetVertex(secondFaceThirdVertexIdx);

                secondFaceThirdVertex = (exclude.Contains(secondFaceThirdVertexIdx)) ? secondFaceThirdVertex + to : secondFaceThirdVertex;

                st.AddVertex(secondFaceThirdVertex);
                st.AddVertex(vertex + to);
                st.AddVertex(vertex);

                exclude.Add(vertexIndex);
            }*/
        }

        return st.Commit();
    }

    private static int FindClosestVertex(MeshDataTool mdt, int originVertexIndex, HashSet<int> toExtrude, params int[] exclude) {

        Vector3 originVertex = mdt.GetVertex(originVertexIndex);

        int closestVertex = -1;
        float? minDistance = null;

        /// Uzimamo sve face-ove koji dele origin vertex
        int[] faces = mdt.GetVertexFaces(originVertexIndex);


        /// Prolazimo kroz svaki face koji nije u toExtrude
        foreach (int faceIndex in faces) {
            if (toExtrude.Contains(faceIndex)) continue;

            /// Prolazimo kroz svaki vertex
            for (int i = 0; i < 3; i++) {
                int vertexIndex = mdt.GetFaceVertex(faceIndex, i);
                if (vertexIndex == originVertexIndex || exclude.Contains(vertexIndex)) continue;

                //int[] edges = mdt.GetVertexEdges(vertexIndex);

                int edge = mdt.GetFaceEdgeBeetwen(faceIndex, originVertexIndex, vertexIndex);
                if (edge == -1) continue;

                /// Edge sme da deli najvise 1 face koji nije u toExtrude
                //! MORA BAREM JEDNA UADHUAHIAF
                int count = 0;
                foreach (int f in mdt.GetEdgeFaces(edge)) {
                    if (!toExtrude.Contains(f)) count += 1;

                    if (count > 1) break;
                }

                Vector3 vertex = mdt.GetVertex(vertexIndex);
                float distance = originVertex.DistanceSquaredTo(vertex);


                /// Ako je minDistance null, onda je ovo prvi put
                if (!minDistance.HasValue) {
                    minDistance = distance;
                    closestVertex = vertexIndex;
                }
                else {
                    minDistance = Math.Min((float)minDistance, distance);
                    /// Ako je nova distanca manja od stare menjamo najblizi vertex
                    if (distance < (float)minDistance) {
                        closestVertex = vertexIndex;
                    }
                }
            }
        }

        return closestVertex;
    }


    //! Neki face-ovi koje ova metoda vraća izgledaju kao da su pogodni za izmenu ali ustvari nisu.
    private static HashSet<int> FacesToExtrude(MeshDataTool mdt, Vector3 dir) {
        HashSet<int> toExtrude = new HashSet<int>();

        /// Za svaki face mesh-a
        for (int faceIndex = 0; faceIndex < mdt.GetFaceCount(); faceIndex++) {

            /// Uzimamo vetrexe i računamo centar face-a
            var a = mdt.GetVertex( mdt.GetFaceVertex(faceIndex, 0) );
            var b = mdt.GetVertex( mdt.GetFaceVertex(faceIndex, 1) );
            var c = mdt.GetVertex( mdt.GetFaceVertex(faceIndex, 2) );
            var center = (a+b+c)/3;

            /// Ponovo prolazimo kroz svaki face mesh-a
            for (int i = 0; i < mdt.GetFaceCount(); i++) {
                if (i == faceIndex) continue; // preskačemo trenutni face

                /// Da li ray iz centra face-a u smeru izduživanja udara u face koji sada proveravamo
                var result = Geometry.RayIntersectsTriangle(
                    center,
                    dir,
                    mdt.GetVertex( mdt.GetFaceVertex(i, 0) ),
                    mdt.GetVertex( mdt.GetFaceVertex(i, 1) ),
                    mdt.GetVertex( mdt.GetFaceVertex(i, 2) )
                );

                /// Ako ray udara u face
                if (result is Vector3 hitPoint) {

                    /// Proveravamo da li je ray udario u samu ivicu face-a
                    // Tako sto upoređujemo razliku mesta presecanja ray-a i rastojanja tog mesta i najbliže 
                    // tačke na ivici face-a (3 puta, za svaku ivicu) sa Vector3.Zero.
                    // Ako ne udara u ivicu onda udara negde "dublje" u face, pa neće proći dalje.
                    if(!(
                        hitPoint - Geometry.GetClosestPointToSegment(hitPoint, a, b) == Vector3.Zero ||
                        hitPoint - Geometry.GetClosestPointToSegment(hitPoint, b, c) == Vector3.Zero ||
                        hitPoint - Geometry.GetClosestPointToSegment(hitPoint, c, a) == Vector3.Zero
                    )) {
                        break;
                    }
                }
                
                /// Ako smo stigli do ovde a zadnji je prolaz, center nije udario ni u jedan drugi face
                if (i == mdt.GetFaceCount()-1) {
                    toExtrude.Add(faceIndex);
                }
            }
        }

        return toExtrude;
    }

    private static MeshDataTool GenerateMeshData(Mesh mesh) {
        var arrMesh = mesh.asArrayMesh();

        var mdt = new MeshDataTool();
        mdt.CreateFromSurface(arrMesh, 0);

        return mdt;
    }

}
