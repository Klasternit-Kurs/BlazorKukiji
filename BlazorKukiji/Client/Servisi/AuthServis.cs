using BlazorKukiji.Shared;
using grpcAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorKukiji.Client.Servisi
{
	public class AuthServis : IAuthServis
	{
		GrpcA.GrpcAClient _kli;
		HttpClient _hc;
		
		public AuthServis(GrpcA.GrpcAClient kli, HttpClient hc)
		{
			_kli = kli;
			_hc = hc;
		}

		public async Task Login(RegistracijaMsg l)
		{
			var rez = await _hc.PostAsJsonAsync("api/userinfo/login", l);
			rez.EnsureSuccessStatusCode();
		}

		public Task LogOut()
		{
			throw new NotImplementedException();
		}

		public async Task<RezultatMsg> Registracija(RegistracijaMsg r)
			=> await _kli.RegAsync(r);


		public async Task<Korisnik> ProveraKorisnika()
		{
			Console.WriteLine("Treba da pozovem proveru");
			var k =	await _hc.GetFromJsonAsync<Korisnik>("api/userinfo/provera");
			Console.WriteLine("--------------------------");
			k.Klejmovi.ToList().ForEach(k => Console.WriteLine($"{k.Key} --- {k.Value}"));
			Console.WriteLine("--------------------------");
			return k;
		}
	}
}
