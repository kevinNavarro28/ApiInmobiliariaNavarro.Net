using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratorio_3.Models{
public class Inmueble
{
    [Key]
    [Display(Name= "Código")]
    
    public int Id { get ;set; }
    [Required]
	public string Direccion { get; set; }
	[Required]
	public int Ambientes { get; set; }
    [Required]
    public int Superficie{ get; set; }

	[Required]
    public string Tipo {get; set;}
	[Required]
    public string Uso {get; set;}
	[Required]
    public double Precio { get; set; }
    [Required]
    public bool Disponible {get; set;}
    
    [NotMapped]
    public string ? Estado {get; set;}


     [Display(Name = "Dueño")]
     public int PropietarioId { get; set; }
     
    [ForeignKey(nameof(PropietarioId))]
    public Propietario?  Propietario { get; set; }

    public string? ImagenUrl { get; set; }
    [NotMapped]
    public IFormFile? ImagenFile { get; set; }


}
	
}