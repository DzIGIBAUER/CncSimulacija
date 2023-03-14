using System;


namespace GCode {

    /// <summary> Exception koji je bačen kada dođe do greške sa programskom rečenicom G koda. </summary>
    public partial class GCodeException : Exception {

        /// <summary> Programska rečenica koja je zaslužna za ovu grešku. </summary>
        public object ProgramskaRecenica;

        public GCodeException () {}

        public GCodeException (string message) : base(message) {}

        public GCodeException (string message, Exception innerException) : base (message, innerException) {}
    
        public GCodeException (string message, ProgramskaRecenica recenica, Exception innerException) : base (message, innerException) {
            ProgramskaRecenica = recenica;
        } 
        
        public GCodeException (string message, ProgramskaRecenica recenica) : base (message) {
            ProgramskaRecenica = recenica;
        } 
    }

}