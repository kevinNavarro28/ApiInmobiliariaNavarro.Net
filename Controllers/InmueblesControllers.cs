using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Laboratorio_3.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;



namespace Laboratorio_3.Controllers
{

	[Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

	public class InmueblesController : ControllerBase//
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;


		public InmueblesController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
		{
			this.contexto = contexto;
			this.config = config;
			this.environment = environment;
		}

    [HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var usuario = User.Identity.Name;
				return Ok(contexto.Inmuebles.Include(e => e.Propietario).Where(e => e.Propietario.Email == usuario));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      try
      {
        var usuario = User.Identity.Name;
        var inmueble=contexto.Inmuebles.Include(e => e.Propietario).Where(e => e.Propietario.Email == usuario).Single(e => e.Id == id);
        if(inmueble.Estado.Equals("Disponible")){
        inmueble.Disponible=true;
      } else
      {
        inmueble.Disponible=false;
      }

      return Ok(inmueble);
      }
    
      catch (Exception ex)
      {
        return BadRequest("Inmuebles no encontrado" +"\r\n"+ ex) ;
      }
    }



        [HttpPost("crear")]
        public async Task<IActionResult> Post([FromForm]Inmueble entidad)

            {

                try{

                    if(ModelState.IsValid){
                        var  usuario = User.Identity.Name;


                        var propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == usuario);

                        if(propietario == null){
                            return BadRequest("Propietario no encontrado");
                        }

                        entidad.PropietarioId = propietario.Id;

                        //Proceso de la Imagen

                        var ImagenUrl = await ProcesarImagen(entidad.ImagenFile);

                        if(!string.IsNullOrEmpty(ImagenUrl)){
                            entidad.ImagenUrl = ImagenUrl;
                        }    
                        await contexto.Inmuebles.AddAsync(entidad);
                        await contexto.SaveChangesAsync();
                        return CreatedAtAction(nameof(Get),new {id=entidad},entidad);

                    }

                    return BadRequest(ModelState);
                }


            catch(Exception ex){
                return BadRequest(ex.ToString());
            }
            }
            

        private async Task<string> ProcesarImagen(IFormFile imagenFile){


            if(imagenFile != null && imagenFile.Length>0){

                    var uploadsFolder = Path.Combine(environment.WebRootPath,"Uploads");

                    if(!Directory.Exists(uploadsFolder)){
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_"+Path.GetFileName(imagenFile.FileName);
                    var filePath = Path.Combine(uploadsFolder,uniqueFileName);
                    using (var filesStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagenFile.CopyToAsync(filesStream);
                    }
                    return "Uploads/"+uniqueFileName;


            }

            return null;

            
        }

[HttpPut("CambiarEstado/{id}")]
public IActionResult CambiarDisponibilidad(int id)
{
    var propietario = contexto.Propietarios.FirstOrDefault(x => x.Email == User.Identity.Name);  
    var inmuebleDB = contexto.Inmuebles.FirstOrDefault(x => x.Id == id && x.PropietarioId == propietario.Id);
    Console.WriteLine(inmuebleDB.ToString());
    if (inmuebleDB == null)
    {
        return NotFound("No se encontró el inmueble o no tienes permisos para modificarlo");
    }

    inmuebleDB.Disponible = !inmuebleDB.Disponible;
    
    try
    {
        contexto.SaveChanges();
        return Ok(inmuebleDB);
    }
    catch (Exception ex)
    {
        return BadRequest($"Error al cambiar la disponibilidad del inmueble: {ex.Message}");
    }
}





    }
}
