using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    //Siempre indicar con que Area estamos trabajando...
    //[Atributo(Area)]

    [Area("Admin")]
    public class CategoriaController : Controller
    {
        //1. Referenciamos a nuetra unidad de trabajo q ya es un servicio que esta en Program
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CategoriaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            //Inicializamos Categoria
            Categoria categoria = new Categoria();
            //Consultamos si el ID es nulo crear una nueva bodega
            if (id==null)
            {
                //Crear nueva Bodega
                //Mandamos el estado de la bodega que este lleno
                categoria.Estado = true;
                //Retornamos la bodega Inicializada
                return View(categoria);
            }

            //Actualizamos Categoria
            categoria = await _unidadTrabajo.Categoria.obtener(id.GetValueOrDefault());
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                if (categoria.Id ==0)
                {
                    await _unidadTrabajo.Categoria.Agregar(categoria);
                    TempData[DS.Exitosa] = "Categoria Creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Categoria.Actualizar(categoria);
                    TempData[DS.Exitosa] = "Categoria Actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al Grabar Categoria";
            return View(categoria);
        }



        //Metodo que retorne todas las vistas

        #region API
        [HttpGet]
        //El IActionResult no solo retorna 1 objeto o 1 vista sino objeto con formato Json 
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Categoria.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var categoriaDb = await _unidadTrabajo.Categoria.obtener(id);
            if(categoriaDb == null)
            {
                return Json(new { succes = false, message = "Error al borrar la categoria" });
            }
            _unidadTrabajo.Categoria.Remover(categoriaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoria borrada exitosamente" });
        }


        [ActionName("ValidarNombre")]

        public async Task<IActionResult> ValidarNombre(string nombre, int id =0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Categoria.ObtenerTodos();
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
