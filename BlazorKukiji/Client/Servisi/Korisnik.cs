using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using grpcAuth;

namespace BlazorKukiji.Client.Servisi
{
	public class Korisnik
	{
		public string Ime { get; set; }
		public bool Ulogovan { get; set; }
		public Dictionary<string, string> Klejmovi { get; set; }

		public static implicit operator Korisnik(KorisnikInfoMsg km)
		{
			var k = new Korisnik { Ime = km.Ime, Ulogovan = km.Auth };

			XmlSerializer deSerijalizacija = new XmlSerializer(typeof(IEnumerable<System.Security.Claims.Claim>));

			var Kljucevi = deSerijalizacija.Deserialize
												   (new StreamReader
												   (new MemoryStream(Encoding.ASCII.GetBytes(km.KlejmoviXML))))
													  as IEnumerable<System.Security.Claims.Claim>;

			Dictionary<string, string> klejmovi = new Dictionary<string, string>();

			Kljucevi.Select(k => new { k.Type, k.Value })
				.ToList()
				.ForEach(k => klejmovi.Add(k.Type, k.Value));
			return k;
		}
	}
}
