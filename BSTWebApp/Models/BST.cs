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

    public class BST<T> where T : IComparable
    {
        public Nodo<T> Raiz { get; private set; }

        public BST()
        {
            Raiz = null;
        }

        public void Insertar(T valor)
        {
            Raiz = InsertarRecursivo(Raiz, valor);
        }

        private Nodo<T> InsertarRecursivo(Nodo<T> nodo, T valor)
        {
            if (nodo == null)
            {
                return new Nodo<T>(valor);
            }

            if (valor.CompareTo(nodo.Valor) < 0)
            {
                nodo.Izquierdo = InsertarRecursivo(nodo.Izquierdo, valor);
            }
            else
            {
                nodo.Derecho = InsertarRecursivo(nodo.Derecho, valor);
            }

            return nodo;
        }

        public Nodo<T> Buscar(T valor)
        {
            return BuscarRecursivo(Raiz, valor);
        }

        private Nodo<T> BuscarRecursivo(Nodo<T> nodo, T valor)
        {
            if (nodo == null || nodo.Valor.CompareTo(valor) == 0)
            {
                return nodo;
            }

            if (valor.CompareTo(nodo.Valor) < 0)
            {
                return BuscarRecursivo(nodo.Izquierdo, valor);
            }
            else
            {
                return BuscarRecursivo(nodo.Derecho, valor);
            }
        }

        public void Eliminar(T valor)
        {
            Raiz = EliminarRecursivo(Raiz, valor);
        }

        private Nodo<T> EliminarRecursivo(Nodo<T> nodo, T valor)
        {
            if (nodo == null) return nodo;

            if (valor.CompareTo(nodo.Valor) < 0)
            {
                nodo.Izquierdo = EliminarRecursivo(nodo.Izquierdo, valor);
            }
            else if (valor.CompareTo(nodo.Valor) > 0)
            {
                nodo.Derecho = EliminarRecursivo(nodo.Derecho, valor);
            }
            else
            {
                if (nodo.Izquierdo == null)
                {
                    return nodo.Derecho;
                }
                else if (nodo.Derecho == null)
                {
                    return nodo.Izquierdo;
                }

                nodo.Valor = MinValor(nodo.Derecho);
                nodo.Derecho = EliminarRecursivo(nodo.Derecho, nodo.Valor);
            }

            return nodo;
        }

        private T MinValor(Nodo<T> nodo)
        {
            T minv = nodo.Valor;
            while (nodo.Izquierdo != null)
            {
                minv = nodo.Izquierdo.Valor;
                nodo = nodo.Izquierdo;
            }
            return minv;
        }

        public List<T> Inorden()
        {
            List<T> resultado = new List<T>();
            InordenRecursivo(Raiz, resultado);
            return resultado;
        }

        private void InordenRecursivo(Nodo<T> nodo, List<T> resultado)
        {
            if (nodo != null)
            {
                InordenRecursivo(nodo.Izquierdo, resultado);
                resultado.Add(nodo.Valor);
                InordenRecursivo(nodo.Derecho, resultado);
            }
        }

        public List<T> Preorden()
        {
            List<T> resultado = new List<T>();
            PreordenRecursivo(Raiz, resultado);
            return resultado;
        }

        private void PreordenRecursivo(Nodo<T> nodo, List<T> resultado)
        {
            if (nodo != null)
            {
                resultado.Add(nodo.Valor);
                PreordenRecursivo(nodo.Izquierdo, resultado);
                PreordenRecursivo(nodo.Derecho, resultado);
            }
        }

        public List<T> Postorden()
        {
            List<T> resultado = new List<T>();
            PostordenRecursivo(Raiz, resultado);
            return resultado;
        }

        private void PostordenRecursivo(Nodo<T> nodo, List<T> resultado)
        {
            if (nodo != null)
            {
                PostordenRecursivo(nodo.Izquierdo, resultado);
                PostordenRecursivo(nodo.Derecho, resultado);
                resultado.Add(nodo.Valor);
            }
        }

        public List<T> NivelOrden()
        {
            List<T> resultado = new List<T>();
            if (Raiz == null) return resultado;

            Queue<Nodo<T>> queue = new Queue<Nodo<T>>();
            queue.Enqueue(Raiz);
            while (queue.Count > 0)
            {
                Nodo<T> temp = queue.Dequeue();
                resultado.Add(temp.Valor);

                if (temp.Izquierdo != null)
                {
                    queue.Enqueue(temp.Izquierdo);
                }

                if (temp.Derecho != null)
                {
                    queue.Enqueue(temp.Derecho);
                }
            }
            return resultado;
        }

        public T Maximo()
        {
            return MaximoRecursivo(Raiz);
        }

        private T MaximoRecursivo(Nodo<T> nodo)
        {
            T maxv = nodo.Valor;
            while (nodo.Derecho != null)
            {
                maxv = nodo.Derecho.Valor;
                nodo = nodo.Derecho;
            }
            return maxv;
        }

        public T Minimo()
        {
            return MinValor(Raiz);
        }

        public void Balancear()
        {
            List<T> valores = Inorden();
            Raiz = BalancearRecursivo(valores, 0, valores.Count - 1);
        }

        private Nodo<T> BalancearRecursivo(List<T> valores, int inicio, int fin)
        {
            if (inicio > fin)
            {
                return null;
            }

            int medio = (inicio + fin) / 2;
            Nodo<T> nodo = new Nodo<T>(valores[medio]);

            nodo.Izquierdo = BalancearRecursivo(valores, inicio, medio - 1);
            nodo.Derecho = BalancearRecursivo(valores, medio + 1, fin);

            return nodo;
        }
    }
}
