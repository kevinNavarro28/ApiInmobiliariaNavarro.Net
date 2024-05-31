using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratorio_3.Models{

public class Contrato{

    [Key]
  [Display(Name= "Código")]
  [Required]  
   public int id {get;set;}

    [Required]
    public DateTime FechaInicio {get;set;}
    [Required]
    public DateTime FechaFin {get;set;}

    [Required]

    public double precio {get;set;}

    [Required]
    [Display(Name= "Inquilino")]     
    public int InquilinoId { get; set; }
    [Display(Name= "Inmueble")]     
    [Required]
    public int InmuebleId  { get; set; }
    public Inquilino? Inquilino { get; set; }
    public Inmueble? Inmueble {  get;   set; }







}


}