using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BSTWebApp.Models;

namespace BSTWebApp.Controllers
{
    public class ArbolController : Controller
    {
        private static BST<Datos> arbol = new BST<Datos>();

        // Acción para insertar datos
        [HttpPost]
        public ActionResult Insertar(DateTime fecha, string matricula, string asistencia)
        {
            bool asistenciaBool = asistencia == "True"; // Convertir el valor de string a booleano
            Datos nuevoDato = new Datos
            {
                Fecha = fecha,
                Matricula = matricula,
                Asistencia = asistenciaBool
            };
            arbol.Insertar(nuevoDato);

            return RedirectToAction("Index", new { mensaje = "Se ha insertado correctamente." });
        }

        // Acción para buscar datos por matrícula
        [HttpPost]
        public ActionResult Buscar(string matricula)
        {
            Datos busqueda = new Datos { Matricula = matricula };
            Nodo<Datos> resultado = arbol.Buscar(busqueda);
            if (resultado != null)
            {
                ViewBag.DatosMatricula = resultado.Valor; // Establecer los datos encontrados
                ViewBag.Mensaje = $"Se encontró la matrícula {matricula}.";
            }
            else
            {
                ViewBag.DatosMatricula = null; // Limpiar los datos si no se encuentra la matrícula
                ViewBag.Mensaje = $"No se encontró la matrícula {matricula}.";
            }

            ViewBag.Records = ObtenerRecorridos(); // Actualizar los recorridos después de la búsqueda
            ViewBag.Minimo = arbol.Raiz != null ? arbol.Minimo().ToString() : "Árbol vacío";
            ViewBag.Maximo = arbol.Raiz != null ? arbol.Maximo().ToString() : "Árbol vacío";
            return View("Index");
        }

        // Acción para eliminar datos por matrícula
        [HttpPost]
        public ActionResult Eliminar(string matricula)
        {
            Datos datoAEliminar = new Datos { Matricula = matricula };

            // Verificar si la matrícula existe en el árbol
            var nodoEncontrado = arbol.Buscar(datoAEliminar);
            if (nodoEncontrado == null)
            {
                // Si no se encuentra la matrícula, retornar un mensaje de error
                return RedirectToAction("Index", new { mensaje = $"No se encontró la matrícula {matricula}." });
            }

            // Si se encuentra la matrícula, eliminarla
            arbol.Eliminar(datoAEliminar);

            return RedirectToAction("Index", new { mensaje = $"Se ha eliminado la matrícula {matricula}." });
        }

        // Método para obtener los recorridos del árbol
        private Dictionary<string, List<string>> ObtenerRecorridos()
        {
            var inOrden = arbol.InOrden();
            var preOrden = arbol.PreOrden();
            var postOrden = arbol.PostOrden();
            var porNiveles = arbol.RecorridoPorNiveles();

            return new Dictionary<string, List<string>>()
            {
                { "InOrden", inOrden.ConvertAll(d => d.ToString()) },
                { "PreOrden", preOrden.ConvertAll(d => d.ToString()) },
                { "PostOrden", postOrden.ConvertAll(d => d.ToString()) },
                { "Por Niveles", porNiveles.ConvertAll(d => d.ToString()) }
            };
        }

        // GET: Arbol/Index
        public ActionResult Index(string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.ResultadoBusqueda = null;
            ViewBag.Records = ObtenerRecorridos(); // Obtener los recorridos al cargar la página
            ViewBag.Minimo = arbol.Raiz != null ? arbol.Minimo().ToString() : "Árbol vacío";
            ViewBag.Maximo = arbol.Raiz != null ? arbol.Maximo().ToString() : "Árbol vacío";
            return View();
        }

        // Acción para obtener el valor mínimo del árbol
        [HttpPost]
        public ActionResult ObtenerMinimo()
        {
            ViewBag.MinMaxTitle = "MÍNIMO ";
            ViewBag.MinMaxValue = arbol.Raiz != null ? arbol.Minimo().ToString() : "Árbol vacío";
            ViewBag.Records = ObtenerRecorridos();
            return View("Index");
        }

        // Acción para obtener el valor máximo del árbol
        [HttpPost]
        public ActionResult ObtenerMaximo()
        {
            ViewBag.MinMaxTitle = "MÁXIMO ";
            ViewBag.MinMaxValue = arbol.Raiz != null ? arbol.Maximo().ToString() : "Árbol vacío";
            ViewBag.Records = ObtenerRecorridos();
            return View("Index");
        }

        // Acción para balancear el árbol
        [HttpPost]
        public ActionResult Balancear()
        {
            arbol.Balancear();
            return RedirectToAction("Index", new { mensaje = "Se ha balanceado el árbol correctamente." });
        }

        // Acción para obtener los datos del árbol para representación gráfica
        public ActionResult ObtenerArbol()
        {
            var arbolData = ObtenerArbol(arbol.Raiz);
            return Json(arbolData, JsonRequestBehavior.AllowGet);
        }

        // Método recursivo para obtener los datos del árbol para representación gráfica
        private object ObtenerArbol(Nodo<Datos> nodo)
        {
            if (nodo == null) return null;

            return new
            {
                valor = new
                {
                    Fecha = nodo.Valor.Fecha.ToString("o"),
                    Matricula = nodo.Valor.Matricula,
                    Asistencia = nodo.Valor.Asistencia ? "True" : "False"
                },
                izquierdo = ObtenerArbol(nodo.Izquierdo),
                derecho = ObtenerArbol(nodo.Derecho)
            };
        }
    }
}
