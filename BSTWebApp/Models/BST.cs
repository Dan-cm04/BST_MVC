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

        public List<T> InOrden()
        {
            List<T> result = new List<T>();
            InOrden(Raiz, result);
            return result;
        }

        private void InOrden(Nodo<T> nodo, List<T> result)
        {
            if (nodo == null) return;
            InOrden(nodo.Izquierdo, result);
            result.Add(nodo.Valor);
            InOrden(nodo.Derecho, result);
        }

        public List<T> PreOrden()
        {
            List<T> result = new List<T>();
            PreOrden(Raiz, result);
            return result;
        }

        private void PreOrden(Nodo<T> nodo, List<T> result)
        {
            if (nodo == null) return;
            result.Add(nodo.Valor);
            PreOrden(nodo.Izquierdo, result);
            PreOrden(nodo.Derecho, result);
        }

        public List<T> PostOrden()
        {
            List<T> result = new List<T>();
            PostOrden(Raiz, result);
            return result;
        }

        private void PostOrden(Nodo<T> nodo, List<T> result)
        {
            if (nodo == null) return;
            PostOrden(nodo.Izquierdo, result);
            PostOrden(nodo.Derecho, result);
            result.Add(nodo.Valor);
        }

        public List<T> RecorridoPorNiveles()
        {
            List<T> result = new List<T>();
            Queue<Nodo<T>> queue = new Queue<Nodo<T>>();
            if (Raiz != null) queue.Enqueue(Raiz);

            while (queue.Count > 0)
            {
                Nodo<T> current = queue.Dequeue();
                result.Add(current.Valor);

                if (current.Izquierdo != null) queue.Enqueue(current.Izquierdo);
                if (current.Derecho != null) queue.Enqueue(current.Derecho);
            }

            return result;
        }

        public T Maximo()
        {
            if (Raiz == null) throw new InvalidOperationException("El árbol está vacío.");
            Nodo<T> current = Raiz;
            while (current.Derecho != null)
            {
                current = current.Derecho;
            }
            return current.Valor;
        }

        public T Minimo()
        {
            if (Raiz == null) throw new InvalidOperationException("El árbol está vacío.");
            Nodo<T> current = Raiz;
            while (current.Izquierdo != null)
            {
                current = current.Izquierdo;
            }
            return current.Valor;
        }

        public void Balancear()
        {
            List<T> valores = InOrden();
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
