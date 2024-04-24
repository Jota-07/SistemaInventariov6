using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Categoria
    {
        //key = Primary key
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre es Requerido")]
        [MaxLength(60, ErrorMessage ="El Nombre debe ser Maximo de 60 caracteres")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerido")]
        [MaxLength(100, ErrorMessage = "Decripcion debe ser Maximo de 100 caracteres")]
        public String Descripcion { get; set; }


        [Required(ErrorMessage = "Estado es Requerido")]
        public bool Estado { get; set; }


    }
}
