using System;
using System.Web.Mvc;
using BSTWebApp.Models;

namespace BSTWebApp.Controllers
{
    public class ArbolController : Controller
    {
        private static BST<Datos> arbol = new BST<Datos>();


        public class Datos : IComparable
        {
            public DateTime Fecha { get; set; }
            public string Matricula { get; set; }
            public bool Asistencia { get; set; }

            public int CompareTo(object obj)
            {
                Datos other = obj as Datos;
                return Fecha.CompareTo(other.Fecha);
            }

            public override string ToString()
            {
                return $"Fecha: {Fecha}, Matricula: {Matricula}, Asistencia: {Asistencia}";
            }
        }



        // GET: Arbol/Index
        public ActionResult Index()
        {
            ViewBag.Mensaje = null; // Limpiar mensajes anteriores al cargar la vista
            ViewBag.Resultado = null;
            return View();
        }

        // Acción para insertar datos
        [HttpPost]
        public ActionResult Insertar(DateTime fecha, string matricula, bool asistencia)
        {
            Datos nuevoDato = new Datos
            {
                Fecha = fecha,
                Matricula = matricula,
                Asistencia = asistencia
            };
            arbol.Insertar(nuevoDato);

            ViewBag.Mensaje = "Se ha insertado correctamente.";
            return RedirectToAction("Index");
        }

        // Acción para buscar datos
        [HttpPost]
        public ActionResult Buscar(DateTime fecha)
        {
            Datos busqueda = new Datos { Fecha = fecha };
            Nodo<Datos> resultado = arbol.Buscar(busqueda);
            ViewBag.Resultado = resultado != null ? resultado.Valor.ToString() : "No encontrado";
            return View("Index");
        }

        // Acción para eliminar datos
        [HttpPost]
        public ActionResult Eliminar(DateTime fecha)
        {
            Datos datoAEliminar = new Datos { Fecha = fecha };
            arbol.Eliminar(datoAEliminar);

            ViewBag.Mensaje = "Se ha eliminado correctamente.";
            return RedirectToAction("Index");
        }

        // Acción para balancear el árbol
        [HttpPost]
        public ActionResult Balancear()
        {
            arbol.Balancear();

            ViewBag.Mensaje = "Se ha balanceado el árbol correctamente.";
            return RedirectToAction("Index");
        }

        // Acción para obtener el árbol como JSON (para la representación gráfica)
        public JsonResult ObtenerArbol()
        {
            var datos = ObtenerDatosDelNodo(arbol.Raiz);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        // Método recursivo para obtener los datos del árbol para representación gráfica
        private object ObtenerDatosDelNodo(Nodo<Datos> nodo)
        {
            if (nodo == null) return null;

            return new
            {
                valor = nodo.Valor,
                izquierdo = ObtenerDatosDelNodo(nodo.Izquierdo),
                derecho = ObtenerDatosDelNodo(nodo.Derecho)
            };
        }
    }
}
