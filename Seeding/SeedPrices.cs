using Team1_FinalProject.DAL;
using Team1_FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team1_FinalProject.Seeding
{
	public static class SeedPrices
	{
		public static void AddPrice(AppDbContext db)
		{
		    //Create a new list for all of the Prices
		List<Price> AllPrices = new List<Price>();
		AllPrices.Add(new Price
		{
			PriceType = PType.WeekdayMorning,
			PriceValue = 5.00m,
		});
		
		AllPrices.Add(new Price
		{
			PriceType = PType.Tuesday,
			PriceValue = 8.00m,
		});
		
		AllPrices.Add(new Price
		{
			PriceType = PType.WeekdayAfternoon,
			PriceValue = 10.00m,
		});
				
		AllPrices.Add(new Price
		{
			PriceType = PType.Weekend,
			PriceValue = 12.00m,
		});
		
		AllPrices.Add(new Price
		{
			PriceType = PType.SeniorCitizen,
			PriceValue = -2.00m,
		});	
				
				//create some counters to help debug problems
		Int32 intPriceID = 0;

		//loop through the list to add or update the genre
		try
		{
			foreach (Price seedPrice in AllPrices)
			{
				//update the counters
				intPriceID = seedPrice.PriceID;
				//see if the genre is already in the database using the UniqueIdentifier
				Price dbPrice = db.Prices.FirstOrDefault(m => m.PriceID == seedPrice.PriceID);

					//if genre is null, genre is not in database
					if (dbPrice == null)
					{
						//Add the genre to the database
						db.Prices.Add(seedPrice);
						db.SaveChanges();
					}
					else //the genre is in the database - reset all fields except ID and Unique Identifier
					{
						dbPrice.PriceType = seedPrice.PriceType;
						dbPrice.PriceValue = seedPrice.PriceValue;
						db.SaveChanges();
					}
				}
			}
			catch (Exception ex) //throw error if there is a problem in the database
			{
				StringBuilder msg = new StringBuilder();
				msg.Append(" (PriceID: ");
				msg.Append(intPriceID);
				msg.Append(")");
				throw new Exception(msg.ToString(), ex);
			}
		}
	}
}
