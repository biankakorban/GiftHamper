
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.Services
{
    public class MyDbContext : IdentityDbContext
    {

        public DbSet<Customer> TblCustomer { get; set; }
        public DbSet<Address> TblAddress { get; set; }
        public DbSet<Category> TblCategory { get; set; }
        public DbSet<Admin> TblAdmin { get; set; }
        public DbSet<Hamper> TblHamper { get; set; }
		public DbSet<HamperProduct> HamperProduct { get; set; }
		public DbSet<Order> TblOrder { get; set; }
        public DbSet<OrderLine> TblOrderLine { get; set; }
        public DbSet<Product> TblProduct { get; set; }
        public DbSet<Review> TblReview { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlServer(@"Server='xxx'; Database='xxx'; Trusted_Connection=True;");
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			//Composite Primary key
			builder.Entity<HamperProduct>()
					.HasKey(t => new { t.HamperId, t.ProductId });

			//now creating many to many relationship 
			builder.Entity<HamperProduct>()
					.HasOne(p => p.hamper)
					.WithMany(x => x.Products)
					.HasForeignKey(y => y.HamperId);

			builder.Entity<HamperProduct>()
					.HasOne(s => s.product)
					.WithMany(c => c.Hampers)
					.HasForeignKey(z => z.ProductId);
		}

	}
}
