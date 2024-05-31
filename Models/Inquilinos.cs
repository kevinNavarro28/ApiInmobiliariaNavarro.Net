using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratorio_3.Models{
    
    public class Inquilino{

        [Required]
        public int Id { get; set; }

         [Required]

        public int Dni { get; set; }

        [Required]


        public string Apellido { get; set; }

        [Required]

        public string Nombre { get; set; }

        [Required]
        

        public string Direccion{ get; set; }

        [Required]

       
        public string Telefono { get; set; }

        [Required]

         public string Email { get; set; }

        [Required]


        public string NombreGarante { get; set; }

        [Required]

    
        public string TelefonoGarante { get;set;}        

        

    }
}