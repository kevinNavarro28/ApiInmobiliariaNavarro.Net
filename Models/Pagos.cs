using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace Laboratorio_3.Models{

public class Pago{

    [Display(Name= "Código")]     
  public int Id { get; set; }

    [Display(Name= "Numero de pago")]     
    public int NroPago { get; set; }

   [Required,Display(Name="Fecha pago")]
    [DataType(DataType.Date)]    
    public DateTime FechaPago { get; set; }
   [Required]
    public double Importe {get;set;}

    [Display(Name= "Contrato")]     
     [Required]
   public int ContratoId { get; set; }
     
    public Contrato? Contrato  { get; set; }




}}