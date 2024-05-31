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

	public class  PagosController : ControllerBase//
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;


		public PagosController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
		{
			this.contexto = contexto;
			this.config = config;
			this.environment = environment;
		}




        [HttpGet("traerpagos")]
        public IActionResult TraerPagos()
        {
            var propietarioEmail = User.Identity.Name;
            var misPagos = contexto.Pagos
            .Where(p => p.Contrato.Inmueble.Propietario.Email == propietarioEmail)
            .Include(p => p.Contrato)
            .ThenInclude(c => c.Inmueble)
            .Include(p => p.Contrato)
            .ThenInclude(c => c.Inquilino)
            .ToList();

            return Ok(misPagos);
        }
        
        }
    
    }