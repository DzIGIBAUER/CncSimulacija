using Godot;
using System;


/// <summary> Exception koji je bačen kada exportovan parametar node-a nije podešen. </summary>
public class ExportParametarNullException : Exception {

    /// <summary> Objekat čiji parametar nije podešen. </summary>
    public object OdgovoranObjekat;

    private static string _msg = "Export parametar Node-a {0} nije podešen."; 

    public ExportParametarNullException (object obj) : base (String.Format(_msg, obj.ToString())) {
        OdgovoranObjekat = obj;
    }

    public ExportParametarNullException (object obj, Exception innerException) : base (String.Format(_msg, obj.ToString()), innerException) {
        OdgovoranObjekat = obj;
    } 
}
