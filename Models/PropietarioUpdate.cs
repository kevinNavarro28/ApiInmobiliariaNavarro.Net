using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Laboratorio_3.Models
{
    public class PropietarioUpdate
{
    [Required]
    public string? Dni { get; set; }
    [Required]
    public string? Apellido { get; set; }
    [Required]
    public string? Nombre { get; set; }
    [Required]
    public string? Telefono { get; set; }
    [Required, EmailAddress]
    public string? Email { get; set; }
}

}