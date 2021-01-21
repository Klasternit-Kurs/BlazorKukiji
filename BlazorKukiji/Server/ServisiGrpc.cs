using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using grpcAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BlazorKukiji.Server
{
	public class Servisi : GrpcA.GrpcABase
	{
		UserManager<IdentityUser> _man;
		SignInManager<IdentityUser> _sign;
		ILogger<Servisi> _log;
		
		public Servisi(UserManager<IdentityUser> man, SignInManager<IdentityUser> sajn,
			ILogger<Servisi> log)
		{
			_man = man;
			_sign = sajn;
			_log = log;
		}
		
		public override async Task<RezultatMsg> Reg(RegistracijaMsg request, ServerCallContext context)
		{
			_log.LogInformation("Dobio zahtev");
			var rez = await _man.CreateAsync(new IdentityUser 
				{ UserName = request.Ime }, request.Pass);
			if (rez.Succeeded)
			{
				_log.LogInformation("Uspesno registrovan");
				return new RezultatMsg { Uspeh = true, Greska = "" };
			}
			else
			{
				_log.LogError("Greska pri registraciji!");
				return new RezultatMsg
				{
					Uspeh = false,
					Greska = rez.Errors.Select(e => e.Description)
					.Aggregate((aku, err) => aku += err + Environment.NewLine)
				};
			}
		}

		public override async Task<RezultatMsg> LogIn(RegistracijaMsg request, ServerCallContext context)
		{
			_log.LogInformation("Dobio zahtev za login");
			var kor = await _man.FindByNameAsync(request.Ime);
			if (kor is null)
			{
				_log.LogError("Korisnik ne postoji");
				return new RezultatMsg { Uspeh = false, Greska = "Nesto nije ok :/" };
			}
			_log.LogInformation("Nasao korisnika");
			var rez = await _sign.CheckPasswordSignInAsync(kor, request.Pass, false);
			_log.LogInformation("Login uspesan");
			if (rez.Succeeded)
				return new RezultatMsg { Uspeh = true, Greska = "" };
			else
				return new RezultatMsg { Uspeh = false, Greska = "Nesto nije ok :/" };
		}
	}
}
