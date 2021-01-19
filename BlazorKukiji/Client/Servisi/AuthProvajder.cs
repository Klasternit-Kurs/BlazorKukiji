﻿using Microsoft.AspNetCore.Components.Authorization;
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
			var korInfo = await DohvatiKorisnika();

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
		}
	}
}