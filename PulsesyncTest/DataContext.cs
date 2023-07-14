using PulsesyncTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PulsesyncTest
{
	public class DataContext : DbContext
	{

		public DbSet<Fruit> Fruits { get; set; }
		public DbSet<Vegetable> Vegetables { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Fruit>().ToTable("Fruits");
			modelBuilder.Entity<Vegetable>().ToTable("Vegetables");
		}
	}
}