using BlazorKukiji.Shared;
using grpcAuth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorKukiji.Server.Controllers
{
	[Microsoft.AspNetCore.Mvc.Route("/api/[controller]/[action]")]
	[ApiController]
	public class UserInfoController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _um;
		private readonly SignInManager<IdentityUser> _sim;
		private readonly ILogger<UserInfoController> _log;

		public UserInfoController(UserManager<IdentityUser> um, SignInManager<IdentityUser> sim,
			ILogger<UserInfoController> log)
		{
			_um = um;
			_sim = sim;
			_log = log;
		}
		[HttpPost]
		public async Task<IActionResult> Registracija(RegistracijaMsg log)
		{
			_log.LogInformation("Dobio zahtev");
			var rez = await _um.CreateAsync(new IdentityUser
			{ UserName = log.Ime }, log.Pass);
			if (rez.Succeeded)
			{
				await _um.AddToRoleAsync(await _um.FindByNameAsync(log.Ime), "Admin");
				_log.LogInformation("Uspesno registrovan");
				return Ok();
			}
			
			_log.LogError("Greska pri registraciji!");
			return BadRequest("Greska pri reg");
			
		}


		[HttpGet]
		public async void Logout()
			=> await _sim.SignOutAsync();

		[HttpGet]
		public Korisnik Provera()
		{
			var k = new Korisnik
			{
				Ime = User.Identity.Name,
				Ulogovan = User.Identity.IsAuthenticated,
				Klejmovi = User.Claims.ToDictionary(k => k.Type, k => k.Value)
			};
			_log.LogInformation(User.Identity.IsAuthenticated.ToString());
			string rez = "-----------------------------" + Environment.NewLine;
			k.Klejmovi.ToList().ForEach(klejm => rez += $"{klejm.Key} -- {klejm.Value}" + Environment.NewLine);
			rez += "----------------------------";
			_log.LogInformation(rez);
			return k;
		}

		[HttpPost]
		public async Task<IActionResult> Login(RegistracijaMsg log)
		{
			_log.LogInformation("Dobio zahtev za login");
			var kor = await _um.FindByNameAsync(log.Ime);
			if (kor is null)
			{
				_log.LogError("Korisnik ne postoji");
				return BadRequest("Error something happen :/");
			}
			_log.LogInformation("Nasao korisnika");
			var rez = await _sim.PasswordSignInAsync(kor, log.Pass, false, false);
			_log.LogInformation("Login uspesan");
			if (rez.Succeeded)
				return Ok();
			else
				return BadRequest("Error something happen :/"); ;
		}
	}
}
