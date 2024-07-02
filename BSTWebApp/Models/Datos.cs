using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BSTWebApp.Models
{
    public class Datos : IComparable
    {
        public DateTime Fecha { get; set; }
        public string Matricula { get; set; }
        public bool Asistencia { get; set; }

        public int CompareTo(object obj)
        {
            Datos other = obj as Datos;
            return Matricula.CompareTo(other.Matricula);
        }

        public override string ToString()
        {
            return $"Fecha: {Fecha.ToString("dd/MM/yyyy")}, Matricula: {Matricula}, Asistencia: {Asistencia}";
        }
    }
}