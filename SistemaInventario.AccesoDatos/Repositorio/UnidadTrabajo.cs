using SistemaInventario.AccesoDatos.data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    //nuestra clase va implementar a la Interfas de trabajo//
    public class UnidadTrabajo : IUnidadTrabajo
        //se implementa los metodos
    {
        private readonly ApplicationDbContext _db;
        public IBodegaRepositorio Bodega { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepositorio(_db);
        }


        public void Dispose()
        {
            _db.Dispose();  //para liberar memoria
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync(); //en forma global se usara y se puede invocar en cualquier momento
        }
    }
}
