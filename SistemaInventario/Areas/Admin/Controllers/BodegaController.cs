using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    //Siempre indicar con que Area estamos trabajando...
    //[Atributo(Area)]

    [Area("Admin")]
    public class BodegaController : Controller
    {
        //1. Referenciamos a nuetra unidad de trabajo q ya es un servicio que esta en Program
        private readonly IUnidadTrabajo _unidadTrabajo;

        public BodegaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            //Inicializamos bodega
            Bodega bodega = new Bodega();
            //Consultamos si el ID es nulo crear una nueva bodega
            if (id==null)
            {
                //Crear nueva Bodega
                //Mandamos el estado de la bodega que este lleno
                bodega.Estado = true;
                //Retornamos la bodega Inicializada
                return View(bodega);
            }

            //Actualizamos Bodega
            bodega = await _unidadTrabajo.Bodega.obtener(id.GetValueOrDefault());
            if (bodega == null)
            {
                return NotFound();
            }
            return View(bodega);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if (bodega.Id ==0)
                {
                    await _unidadTrabajo.Bodega.Agregar(bodega);
                    TempData[DS.Exitosa] = "Bodega Creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Bodega.Actualizar(bodega);
                    TempData[DS.Exitosa] = "Bodega Actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al Grabar Bodega";
            return View(bodega);
        }



        //Metodo que retorne todas las vistas

        #region API
        [HttpGet]
        //El IActionResult no solo retorna 1 objeto o 1 vista sino objeto con formato Json 
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegaDb = await _unidadTrabajo.Bodega.obtener(id);
            if(bodegaDb == null)
            {
                return Json(new { succes = false, message = "Error al borrar la bodega" });
            }
            _unidadTrabajo.Bodega.Remover(bodegaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Bodega borrada exitosamente" });
        }


        [ActionName("ValidarNombre")]

        public async Task<IActionResult> ValidarNombre(string nombre, int id =0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Bodega.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }

        #endregion 


    }
}
