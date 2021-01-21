using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorKukiji.Shared
{
	public class Korisnik
	{
		public string Ime { get; set; }
		public bool Ulogovan { get; set; }
		public Dictionary<string, string> Klejmovi { get; set; }
	}
}
