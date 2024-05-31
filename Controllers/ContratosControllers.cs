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

	public class ContratosController : ControllerBase//
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;


		public ContratosController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
		{
			this.contexto = contexto;
			this.config = config;
			this.environment = environment;
		}
        [HttpGet("{id}")]
        
        public Contrato ObtenerContrato(int id){
            
            var propietario = contexto.Propietarios.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            var contrato = contexto.Contratos.Where(x => x.id == id ).FirstOrDefault();
            var inmueble = contexto.Inmuebles.Where(x => x.Id == contrato.InmuebleId).FirstOrDefault();
            var Inquilino = contexto.Inquilinos.Where(x => x.Id==contrato.InquilinoId).FirstOrDefault();
            if(contrato.InquilinoId == Inquilino.Id && contrato.InmuebleId == inmueble.Id && inmueble.PropietarioId == propietario.Id && propietario.Email == User.Identity.Name && User.Identity.IsAuthenticated){
                return contrato;
            }
            return null;
         }



           
            [HttpGet("traercontratos")]

            public IActionResult TraerContatos()
            {
                var propietarioEmail = User.Identity.Name;
                var misContratos = contexto.Contratos
                .Where(x => x.Inmueble.Propietario.Email == propietarioEmail)
                .Include(x => x.Inquilino)
                .Include(x => x.Inmueble.Propietario)
                
                .ToList();

             return Ok(misContratos);
            }

    
    
    
    }
    
    }
