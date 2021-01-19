using grpcAuth;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorKukiji.Client.Servisi
{
	public class AuthProvajder : AuthenticationStateProvider
	{
		private IAuthServis _api;
		private Korisnik _kor;

		public AuthProvajder(IAuthServis api)
		{
			_api = api;
		}

		public async override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var id = new ClaimsIdentity(); //sadrzi sve klejmove za korisnika (tj sadrzace ako ih ima :) )
			Korisnik korInfo = await DohvatiKorisnika();

			if (korInfo.Ulogovan)
			{
				var klejmovi = new[] { new Claim(ClaimTypes.Name, _kor.Ime) }
					.Concat(_kor.Klejmovi.Select(k => new Claim(k.Key, k.Value)));

				id = new ClaimsIdentity(klejmovi, "Server authentication");
			}
			return new AuthenticationState(new ClaimsPrincipal(id));
		}

		private async Task<Korisnik> DohvatiKorisnika()
			=> _kor is not null && _kor.Ulogovan ? _kor : await _api.ProveraKorisnika();

		public async Task<RezultatMsg> Registracija(RegistracijaMsg k)
		{
			var rez = await _api.Registracija(k);
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
			return rez;
		}

		public async Task<RezultatMsg> Login(RegistracijaMsg k)
		{
			var rez = await _api.Login(k);
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
			return rez;
		}

		public async Task Logut()
		{		
			await _api.LogOut();
			_kor = null;
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

	}
}
