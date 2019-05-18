using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BiankaKorban_DiplomaProject.Services
{
	public class HamperManager
	{
		private MyDbContext _context;
		private DbSet<Hamper> _dbHamper;

		public HamperManager()
		{
			_context = new MyDbContext();
			_dbHamper = _context.Set<Hamper>();
		}

		public Hamper addProduct(int hamperId, int productId)
		{
			Hamper dbHamper = _dbHamper.Where(c => c.HamperId == hamperId)
											.Include(c => c.Products).FirstOrDefault();
			Product dbProduct = _context.TblProduct.Where(s => s.ProductId == productId).FirstOrDefault();

			dbHamper.Products.Add(new HamperProduct { product = dbProduct });

			_context.SaveChanges();

			return dbHamper;
		}

		public void RemoveProduct(int hamperId, int productId)
		{
			var hamper = _dbHamper.Where(c => c.HamperId == hamperId)
											.Include(c => c.Products).FirstOrDefault();

			HamperProduct dbProduct = _context.HamperProduct.Where(s => s.ProductId == productId).FirstOrDefault();

			hamper.Products.Remove(dbProduct);

			_context.SaveChanges();


		}

	

	}
}
