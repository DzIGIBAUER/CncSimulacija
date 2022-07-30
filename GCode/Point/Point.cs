using Godot;
using System;



namespace GCode {

    /// <summary> Opisuje Vector3 u koordinatnom sistemu radnog prostora. </summary>
    public struct Point {

        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Point(float x, float y, float z) {
            X = x; Y = y; Z = z;
        }

        public Point(Vector3 v) {
            X = v.x; Y = v.y; Z = v.z;
        }

        /// <summary> Prebacuje ovaj Point u Vector3, zadržavajući redosled članova.</summary>
        public Vector3 AsVector() => new Vector3(X, Y, Z);

        public float this[int i] {
            get {
                switch(i) {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public override string ToString() => $"Point({ X }, { Y }, { Z })";
    }
}
