using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorKukiji.Server
{
	public class DB : ApiAuthorizationDbContext<IdentityUser>
	{
		public DB(DbContextOptions<DB> o, IOptions<OperationalStoreOptions> o2)
			: base(o, o2)
		{ }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<IdentityRole>().HasData(
				new IdentityRole { Id = Guid.NewGuid().ToString(), ConcurrencyStamp = "zxcvsdc", Name = "Admin", NormalizedName = "ADMIN" });
		}
	}
}
