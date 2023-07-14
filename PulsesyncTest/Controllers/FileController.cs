using PulsesyncTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PulsesyncTest.Controllers
{
	// FileController.cs
	public class FileController : ApiController
	{
		[System.Web.Http.HttpPost]
		public IHttpActionResult UploadFile()
		{
			var httpRequest = HttpContext.Current.Request;
			var file = httpRequest.Files.Count > 0 ? httpRequest.Files[0] : null;

			if (file == null || file.ContentLength == 0)
				return BadRequest("No file uploaded.");

			var lines = new List<string>();
			using (var reader = new StreamReader(file.InputStream))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					lines.Add(line);
				}
			}

			var fruits = new List<Fruit>();
			var vegetables = new List<Vegetable>();

			foreach (var line in lines)
			{
				var parts = line.Split(',');

				if (parts.Length == 2)
				{
					var name = parts[0].Trim();
					var type = parts[1].Trim();

					if (type.Equals("Fruit", StringComparison.OrdinalIgnoreCase))
					{
						fruits.Add(new Fruit { Name = name, Type = type });
					}
					else if (type.Equals("Vegetable", StringComparison.OrdinalIgnoreCase))
					{
						vegetables.Add(new Vegetable { Name = name, Type = type });
					}
				}
			}

			using (var context = new DataContext())
			{
				context.Fruits.AddRange(fruits);
				context.Vegetables.AddRange(vegetables);
				context.SaveChanges();
			}

			return Ok("File uploaded and data saved successfully.");
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/FruitsAndVegetables")]
		public IHttpActionResult GetFruitsAndVegetables()
		{
			using (var context = new DataContext())
			{
				var fruitsAndVegetables = context.Fruits
					.Select(f => new { Name = f.Name, Type = f.Type })
					.Union(context.Vegetables.Select(v => new { Name = v.Name, Type = v.Type }))
					.ToList();

				return Ok(fruitsAndVegetables);
			}
		}

	}

}