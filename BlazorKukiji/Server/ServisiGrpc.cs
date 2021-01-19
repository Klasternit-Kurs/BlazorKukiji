using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using grpcAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Xml.Serialization;
using System.IO;

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
			var kor = await _man.FindByNameAsync(request.Ime);
			if (kor is null)
				return new RezultatMsg { Uspeh = false, Greska = "Nesto nije ok :/" };

			var rez = await _sign.CheckPasswordSignInAsync(kor, request.Pass, false);
			if (rez.Succeeded)
				return new RezultatMsg { Uspeh = true, Greska = "" };
			else
				return new RezultatMsg { Uspeh = false, Greska = "Nesto nije ok :/" };
		}

		public override async Task<KorisnikInfoMsg> Provera(NullMsg request, ServerCallContext context)
		{
			_log.LogInformation("Proveravam korisnika");
			if (_sign.Context.User.Identity.IsAuthenticated) //da li je ulogovan ili ne :)
			{
				_log.LogInformation("Nije ulogovan, zavrsavam");
				return new KorisnikInfoMsg { Auth = false, Ime = string.Empty, KlejmoviXML = string.Empty };
			}
			else
			{
				_log.LogInformation("Ulogovan, lovim klejmove...");
				var klejmovi = _sign.Context.User.Claims;
				XmlSerializer xs = new XmlSerializer(klejmovi.GetType());
				var mem = new MemoryStream();

				xs.Serialize(mem, klejmovi);        //serijalizuje objekat u stream
				var sr = new StreamReader(mem);     //stream reader nam olaksava citanje iz streama
				string kred = sr.ReadToEnd();       //ucitavamo kompletan stream kao string
				_log.LogInformation($"Klejmovi su: {kred}");
				return new KorisnikInfoMsg
				{
					Auth = true,
					Ime = _sign.Context.User.Identity.Name,
					KlejmoviXML = kred
				};
			}
		}
	}
}
