using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


/// <summary>
/// Klasa sa metodama potrebnim za razvlačenje mesh-a.
/// </summary>
public static class Extrude {

    /// <summary>
    /// MeshDataTool cache za svaki mesh sa kojim smo radili, ako nam bude trebao ponovno.
    /// </summary>
    private static ConditionalWeakTable<Mesh, MeshDataTool> _cache = new ConditionalWeakTable<Mesh, MeshDataTool>();

    public static Vector2[] ToConvexHull2D(this Mesh mesh, Node3D node, Vector3 pointA, Vector3 pointB) {
        var mdt = _cache.GetValue(mesh, GenerateMeshData);

        Vector3 pivot;

        var polyPoints = new List<Vector2>();
        for (int vertexIndex = 0; vertexIndex < mdt.GetVertexCount(); vertexIndex++) {
            Vector3 vertex = mdt.GetVertex(vertexIndex);

            pivot = Geometry3D.GetClosestPointToSegmentUncapped(vertex, pointA, pointB);

            Vector3 rotated = new Vector3(pivot.X, pivot.Y, pivot.Z) - (pivot - vertex);

            rotated = node.ToGlobal(rotated);

            if (rotated.X > pivot.X) {
                rotated.X = pivot.X;
            }

            polyPoints.Add(new Vector2(rotated.X, rotated.Y));
        }

        return Geometry2D.ConvexHull(polyPoints.ToArray());
    }

    public static ArrayMesh ExtrudeAroundConvexHull(this Vector2[] points, Node3D node, Vector3 pointA, Vector3 pointB) {
        Vector3 pivot;
        Vector3 dir;
        
        var quat = new Quaternion(pointA.DirectionTo(pointB), Mathf.Tau / 360);
        
        var st = new SurfaceTool();
        st.Begin(Mesh.PrimitiveType.Triangles);

        for (int i = 0; i < points.Length; i++) {
            int aIndex = i;
            int bIndex = (i < points.Length-1) ? i+1 : 0;
            
            Vector3 a = new Vector3(points[aIndex].X, points[aIndex].Y, 0);
            Vector3 b = new Vector3(points[bIndex].X, points[bIndex].Y, 0);

            a = node.ToLocal(a);
            b = node.ToLocal(b);

            //st.AddVertex(a);
            //st.AddVertex(b);

            for (int step = 0; step < 360; step++) {
                pivot = Geometry3D.GetClosestPointToSegmentUncapped(a, pointA, pointB);
                dir = a - pivot;
                dir = quat * dir;

                var ca = dir + pivot;
                
                pivot = Geometry3D.GetClosestPointToSegmentUncapped(b, pointA, pointB);
                dir = b - pivot;
                dir = quat * dir;

                var cb = dir + pivot;

                st.AddTriangleFan(new []{a, b, ca});
                st.AddTriangleFan(new []{b, cb, ca});
                a = ca;
                b = cb;
            }

        }

        return st.Commit();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    /// <returns></returns>
    public static List<Vector2> PointsToExtrude(Mesh mesh, Vector3 pointA, Vector3 pointB) {
        var mdt = _cache.GetValue(mesh, GenerateMeshData);

        Vector3 pivot;

        var polyPoints = new List<Vector2>();
        for (int vertexIndex = 0; vertexIndex < mdt.GetVertexCount(); vertexIndex++) {
            Vector3 vertex = mdt.GetVertex(vertexIndex);

            pivot = Geometry3D.GetClosestPointToSegmentUncapped(vertex, pointA, pointB);

            Vector3 rotated = new Vector3(pivot.X - (vertex - pivot).X, pivot.Y, pivot.Z);

            if (rotated.X > pivot.X) {
                GD.Print($"Vece je { rotated } od { pivot }");
                rotated.X = pivot.X;
            } else {
                GD.Print("Nije");
            }

            polyPoints.Add(new Vector2(rotated.X, rotated.Y));
        }

        return Geometry2D.ConvexHull(polyPoints.ToArray()).ToList();
    }

    public static ArrayMesh ExtrudedAround(this Mesh mesh, Vector3 pointA, Vector3 pointB) {
        List<Vector2> points = PointsToExtrude(mesh, pointA, pointB);
        var mdt = _cache.GetValue(mesh, GenerateMeshData);

        Vector3 pivot;
        Vector3 dir;
        var quat = new Quaternion(pointA.DirectionTo(pointB), Mathf.Tau / 360);

        var st = new SurfaceTool();
        st.Begin(Mesh.PrimitiveType.Triangles);

        int steps = 60;
        /*for (int i = 0; i < points.Count; i = i + steps+1) {
            int aIndex = i;
            int bIndex = (i < points.Count-1) ? i+1 : 0;

            float moveFactor = points[aIndex].DistanceTo(points[bIndex]);
            for (int k = 0; k < steps; k++) {
                points.Insert(bIndex, points[aIndex].MoveToward(points[bIndex], moveFactor));
            }
        }*/

        GD.Print(points.Count);

        for (int i = 0; i < points.Count; i++) {
            int aIndex = i;
            int bIndex = (i < points.Count-1) ? i+1 : 0;
            
            Vector3 a = new Vector3(points[aIndex].X, points[aIndex].Y, 0);
            Vector3 b = new Vector3(points[bIndex].X, points[bIndex].Y, 0);

            //st.AddVertex(a);
            //st.AddVertex(b);
   
            for (int step = 0; step < 360; step++) {
                pivot = Geometry3D.GetClosestPointToSegmentUncapped(a, pointA, pointB);
                dir = a - pivot;
                dir = quat * dir;

                var ca = dir + pivot;
                
                pivot = Geometry3D.GetClosestPointToSegmentUncapped(b, pointA, pointB);
                dir = b - pivot;
                dir = quat * dir;

                var cb = dir + pivot;

                st.AddTriangleFan(new []{a, ca, b});
                st.AddTriangleFan(new []{b, ca, cb});
                a = ca;
                b = cb;
            }

        }

        return st.Commit();
    }

    /// <summary>
    /// Razvlači mesh.
    /// </summary>
    /// <param name="mesh">Mesh koji razvlačimo.</param>
    /// <param name="to">Vektor do kog mesh treba da bude razvučen.</param>
    /// <returns>Razvučen mesh.</returns>
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

            for (int i = 0; i < 3; i++) {
                int vertexIndex = mdt.GetFaceVertex(faceIndex, i);
                Vector3 vertex = mdt.GetVertex(vertexIndex);

                int firstFaceThirdVertexIdx = FindClosestVertex(mdt, vertexIndex, toExtrude);

                /// Kreiramo prvi face
                {
                    Vector3 thirdVertex = mdt.GetVertex(firstFaceThirdVertexIdx);

                    var angle = vertex.SignedAngleTo(thirdVertex, to);

                    /// Redosled vertexa mora da bude u smeru kazaljke na satu
                    List<Vector3> verticies;


                    /// Ako smo već kreirali face sa ovim vertexom, onda ga offsetujemo sa 'to'
                    thirdVertex = (exclude.Contains(firstFaceThirdVertexIdx)) ? thirdVertex + to : thirdVertex;


                    /// Ako je ugao gledajući iz pravca izvlačenja manji, poređaj vertexe da budu u smeru kazaljke na satu
                    if (angle < 0) {
                        verticies = new List<Vector3> {vertex, thirdVertex, vertex + to};
                    }
                    else {
                        verticies = new List<Vector3> {vertex, vertex + to, thirdVertex};
                    }

                    foreach (Vector3 v in verticies) st.AddVertex(v);

                }


                /// Kreiramo drugi face
                {
                    int thirdVertexIndex = FindClosestVertex(mdt, vertexIndex, toExtrude, firstFaceThirdVertexIdx);
                    Vector3 thirdVertex = mdt.GetVertex(thirdVertexIndex);

                    var angle = vertex.SignedAngleTo(thirdVertex, to);

                    /// Redosled vertexa mora da bude u smeru kazaljke na satu
                    List<Vector3> verticies;


                    /// Ako smo već kreirali face sa ovim vertexom, onda ga offsetujemo sa 'to'
                    thirdVertex = (exclude.Contains(thirdVertexIndex)) ? thirdVertex + to : thirdVertex;

                    /// Ako je ugao gledajući iz pravca izvlačenja manji, poređaj vertexe da budu u smeru kazaljke na satu
                    if (angle < 0) {
                        verticies = new List<Vector3> {vertex, thirdVertex, vertex + to};
                    }
                    else {
                        verticies = new List<Vector3> {vertex, vertex + to, thirdVertex};
                    }

                    foreach (Vector3 v in verticies) st.AddVertex(v);

                }

                exclude.Add(vertexIndex);
            }
        }

        return st.Commit();
    }

    /// <summary>
    /// Pokušava da pronađe najbliži vertex u MeshDataTool objektu, u odnosu na prosleđeni.
    /// </summary>
    /// <param name="mdt">MeshDataTool objekat mesh-a.</param>
    /// <param name="originVertexIndex">Vertex u odnosu na koji gledamo.</param>
    /// <param name="toExtrude">Vertexi koji treba da budu relocirani tokom izvlačenja.</param>
    /// <param name="exclude">Vertexi koji će biti ignorisani tokom potrage.</param>
    /// <returns>Index vertexa u MeshDataTool, ili -1 ako vertex nije pronađen.</returns>
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

                int edge = mdt.GetFaceEdgeBeetwen(faceIndex, originVertexIndex, vertexIndex);
                if (edge == -1) continue;

                /// Edge mora da deli najvise 1 face koji nije u toExtrude
                int count = 0;
                foreach (int f in mdt.GetEdgeFaces(edge)) {
                    if (!toExtrude.Contains(f)) count += 1;

                    if (count > 1) break;
                }

                if (count != 1) continue;

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


    /// <summary>
    /// Uzima MeshDataTool mesh-a i određuje koji face-ovi treba da budu relocirani tokom razvlačenja.
    /// </summary>
    /// <param name="mdt">MeshDataTool mesh-a koji razvlačimo.</param>
    /// <param name="dir">Normalizovan vektor smera u kom razvlačimo.</param>
    /// <returns>Indexi face-ova.</returns>
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
                var result = Geometry3D.RayIntersectsTriangle(
                    center,
                    dir,
                    mdt.GetVertex( mdt.GetFaceVertex(i, 0) ),
                    mdt.GetVertex( mdt.GetFaceVertex(i, 1) ),
                    mdt.GetVertex( mdt.GetFaceVertex(i, 2) )
                );

                /// Ako ray udara u face
                if (result.Obj is Vector3 hitPoint) {

                    /// Proveravamo da li je ray udario u samu ivicu face-a
                    // Tako sto upoređujemo razliku mesta presecanja ray-a i rastojanja tog mesta i najbliže 
                    // tačke na ivici face-a (3 puta, za svaku ivicu) sa Vector3.Zero.
                    // Ako ne udara u ivicu onda udara negde "dublje" u face, pa neće proći dalje.
                    if(!(
                        hitPoint - Geometry3D.GetClosestPointToSegment(hitPoint, a, b) == Vector3.Zero ||
                        hitPoint - Geometry3D.GetClosestPointToSegment(hitPoint, b, c) == Vector3.Zero ||
                        hitPoint - Geometry3D.GetClosestPointToSegment(hitPoint, c, a) == Vector3.Zero
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

    /// <summary>
    /// Generiše MeshDataTool od datog mesh-a.
    /// </summary>
    /// <param name="mesh">Mesh čiji MeshDataTool generišemo.</param>
    /// <returns>Generisani MeshDataTool.</returns>
    private static MeshDataTool GenerateMeshData(Mesh mesh) {
        var arrMesh = mesh.asArrayMesh();

        var mdt = new MeshDataTool();
        mdt.CreateFromSurface(arrMesh, 0);

        return mdt;
    }

}
