using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using grpcAuth;

namespace BlazorKukiji.Client.Servisi
{
	public interface IAuthServis
	{
		Task<RezultatMsg> Registracija(RegistracijaMsg r);
		Task<RezultatMsg> Login(RegistracijaMsg l);
		Task LogOut();
		Task<Korisnik> ProveraKorisnika();
	}
}
