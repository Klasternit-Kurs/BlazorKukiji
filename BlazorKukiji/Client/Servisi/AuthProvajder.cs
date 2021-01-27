using BlazorKukiji.Shared;
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
			var id = new ClaimsIdentity();
			_kor = await DohvatiKorisnika();

			if (_kor.Ulogovan)
			{
				var klejmovi = new[] { new Claim(ClaimTypes.Name, _kor.Ime) }
					.Concat(_kor.Klejmovi.Select(k => new Claim(k.Key, k.Value)));
				id = new ClaimsIdentity(klejmovi, "Server authentication");
			}
			return new AuthenticationState(new ClaimsPrincipal(id));
		}

		private async Task<Korisnik> DohvatiKorisnika()
			=> _kor is not null && _kor.Ulogovan ? _kor : await _api.ProveraKorisnika();

		public async Task Registracija(RegistracijaMsg reg)
		{
			await _api.Registracija(reg);
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		public async Task Login(RegistracijaMsg reg)
		{
			await _api.Login(reg);

			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		public async Task Logout()
		{
			await _api.LogOut();
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}
	}
}
