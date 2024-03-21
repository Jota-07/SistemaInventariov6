using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    //IDisposable : Proporciona un mecanismo para liberar recursos no administrados.//
    //..permite deshacerse de cualquier recurso del sistema y liberará objetos que no se estennconsumiendo//
    public interface IUnidadTrabajo : IDisposable
    {
        //envolver los repositorios que tenemos
        IBodegaRepositorio Bodega { get;  }

        //metodo asincrono guardar
        Task Guardar();

    }
}
