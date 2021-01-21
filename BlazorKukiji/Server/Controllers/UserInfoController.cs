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
		[HttpGet]
		public Korisnik Provera()
		 => new Korisnik
			{
				Ime = User.Identity.Name,
				Ulogovan = User.Identity.IsAuthenticated,
				Klejmovi = User.Claims.ToDictionary(k => k.Type, k => k.Value)
			};

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
			var rez = await _sim.CheckPasswordSignInAsync(kor, log.Pass, false);
			_log.LogInformation("Login uspesan");
			if (rez.Succeeded)
				return Ok();
			else
				return BadRequest("Error something happen :/"); ;
		}
	}
}
