using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BSTWebApp.Models
{
    public class Nodo<T> where T : IComparable
    {
        public T Valor { get; set; }
        public Nodo<T> Izquierdo { get; set; }
        public Nodo<T> Derecho { get; set; }

        public Nodo(T valor)
        {
            Valor = valor;
            Izquierdo = null;
            Derecho = null;
        }
    }
}