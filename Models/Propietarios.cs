using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Laboratorio_3.Models
{
	public class Propietario
	{
		[Key]
		[Display(Name = "Código")]
		public int Id { get; set; }
		[Required]
		public string? Dni { get; set; }
		[Required]
		public string? Apellido { get; set; }
		[Required]
		public string? Nombre { get; set; }
		[Required]
		[Display(Name = "Teléfono")]
		public string? Telefono { get; set; }
		[Required, EmailAddress]
		public string? Email { get; set; }
		[DataType(DataType.Password)]
		[JsonIgnore]
		[Required]
		public string Clave { get; set; }

		public override string ToString()
		{
			//return $"{Apellido}, {Nombre}";
			return $"{Nombre} {Apellido}";
		}
	}
}