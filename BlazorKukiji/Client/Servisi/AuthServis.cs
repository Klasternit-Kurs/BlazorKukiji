using grpcAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorKukiji.Client.Servisi
{
	public class AuthServis : IAuthServis
	{
		GrpcA.GrpcAClient _kli;
		
		public AuthServis(GrpcA.GrpcAClient kli)
		{
			_kli = kli;
		}

		public Task<RezultatMsg> Login(RegistracijaMsg l)
		{
			throw new NotImplementedException();
		}

		public Task LogOut()
		{
			throw new NotImplementedException();
		}

		public async Task<RezultatMsg> Registracija(RegistracijaMsg r)
			=> await _kli.RegAsync(r);
		

		public Task<Korisnik> ProveraKorisnika()
		{
			throw new NotImplementedException();
		}
	}
}
