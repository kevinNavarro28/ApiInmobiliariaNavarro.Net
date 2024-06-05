﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Laboratorio_3.Models;
using MailKit.Security;
using MimeKit;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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

	public class PropietariosController : ControllerBase//
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;


		public PropietariosController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
		{
			this.contexto = contexto;
			this.config = config;
			this.environment = environment;
		}


		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromForm] LoginView loginView)
		{
			try
			{	
                
				string hashed = HashPassword(loginView.Clave);
				var p = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
                Console.WriteLine(hashed);
				if (p == null || p.Clave != hashed)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}
            
				else
				{
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, p.Email),
						new Claim("FullName", p.Nombre + " " + p.Apellido),
                        new Claim("Password", p.Clave)

					};


					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		public async Task<ActionResult<Propietario>> Get()
		{
			try
			{
				var usuario = User.Identity.Name;
				return await contexto.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
				
				
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		

		[HttpPut]

		public async Task<IActionResult> Put ([FromBody] PropietarioUpdate entidad){
			Console.WriteLine(entidad);
			try{
				if(ModelState.IsValid){
				
					var propietario = await contexto.Propietarios.AsNoTracking().FirstOrDefaultAsync(x=> x.Email == entidad.Email);
					propietario.Dni = entidad.Dni;
					propietario.Apellido = entidad.Apellido;
					propietario.Nombre = entidad.Nombre;
					propietario.Telefono = entidad.Telefono;
					propietario.Email = entidad.Email;
                    contexto.Entry(propietario).Property(p => p.Clave).IsModified = false;
					
					contexto.Propietarios.Update(propietario);
					await contexto.SaveChangesAsync();
                    
					return Ok(entidad);


				}
				return BadRequest(ModelState);

			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}
    

    [HttpPost("email")]
[AllowAnonymous]
public async Task<IActionResult> GetByEmail([FromForm] Propietario model)
{
    try
    {
        var entidad = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == model.Email);
        if (entidad == null)
        {
            return NotFound("No se encontró un propietario con ese email.");
        }

        // Generate JWT token
        var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, entidad.Email),
            new Claim("FullName", entidad.Nombre + " " + entidad.Apellido),
            new Claim("Password", entidad.Clave)
        };

        var token = new JwtSecurityToken(
            issuer: config["TokenAuthentication:Issuer"],
            audience: config["TokenAuthentication:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: credenciales
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // Generate reset link
        var resetLink = GenerarUrlCompleta("Token", "Propietarios", tokenString, model.Email);

        await ReseteoClave(entidad.Email, resetLink);

        return Ok("Se ha enviado un enlace para restablecer la contraseña a su correo electrónico.");
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}


[HttpGet("token")]
[AllowAnonymous]
public async Task<IActionResult> Token([FromQuery] string access_token, [FromQuery] string email)
{
    try
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(access_token);

        var propietarioEmail = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        if (propietarioEmail == null || propietarioEmail != email)
        {
            return BadRequest("Token inválido o email no coincide.");
        }

        var propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == email);
        if (propietario == null)
        {
            return NotFound("Propietario no encontrado.");
        }

        // Generate new password
        Random rand = new Random(Environment.TickCount);
        string randomChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
        string nuevaClave = new string(Enumerable.Repeat(randomChars, 8).Select(s => s[rand.Next(s.Length)]).ToArray());

        propietario.Clave = HashPassword(nuevaClave);
        await contexto.SaveChangesAsync();

        await MandarClaveNueva(propietario.Email, nuevaClave);

        return Ok("Se ha generado una nueva contraseña y se ha enviado a su correo electrónico.");
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}



    private string HashPassword(string password)
    {
        
         string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password,
                            salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));
        return hashed;
    }

    private async Task ReseteoClave(string email, string resetLink)
    {
        var message = new MailMessage();
        message.From = new MailAddress("from@example.com");
        message.To.Add(new MailAddress(email));
        message.Subject = "Restablecimiento de contraseña";
        message.Body = $@"Por favor, restablece tu contraseña haciendo clic en el siguiente enlace: <a href=""{resetLink}"">{resetLink}</a>";
        message.IsBodyHtml = true;

        using var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential("45ebf36e05685c", "07bbeb52590c6a"),
            EnableSsl = true
        };
        await client.SendMailAsync(message);
    }

    private async Task MandarClaveNueva(string email, string newPassword)
    {
        var message = new MailMessage();
        message.From = new MailAddress("from@example.com");
        message.To.Add(new MailAddress(email));
        message.Subject = "Nueva contraseña";
        message.Body = $"Tu nueva contraseña es: {newPassword}";
        message.IsBodyHtml = true;

        using var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential("45ebf36e05685c", "07bbeb52590c6a"),
            EnableSsl = true
        };
        await client.SendMailAsync(message);
    }

    private string GenerarUrlCompleta(string action, string controller, string tokenString, string email)
    {
        var dominio = environment.IsDevelopment() ? "192.168.1.3:5000" : "www.misitio.com";
        var url = $"{Request.Scheme}://{dominio}/{controller}/{action}?access_token={tokenString}&email={HttpUtility.UrlEncode(email)}";
        return url;
    }

    



[HttpPut("cambiarClave")]
public async Task<IActionResult> CambiarClave([FromBody] CambiarClaveViewModel model)
{
    try
    {
        var email = User.Identity.Name;
        var propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == email);

        if (propietario == null)
        {
            return NotFound("Propietario no encontrado.");
        }

        // Actualizar la clave en la base de datos
        propietario.Clave = HashPassword(model.NuevaClave);
        await contexto.SaveChangesAsync();

        return Ok("La clave ha sido cambiada exitosamente.");
    }
    catch (Exception ex)
    {
        return BadRequest("Error al cambiar la clave: " + ex.Message);
    }
}

    



}

 }





