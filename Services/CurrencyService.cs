using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CurrencyApi.Services
{
    public class CurrencyService
    {
        private readonly IMongoCollection<Currency> _currencies;

        public CurrencyService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("CurrencyDb"));
            var database = client.GetDatabase("CurrencyDb");
            _currencies = database.GetCollection<Currency>("Currencies");
        }

        public List<Currency> Get()
        {
            Console.WriteLine(_currencies.Find(currency => true).ToList().Count);
            return _currencies.Find(currency => true).ToList();
        }

        public Boolean Get(DateTime date)
        {
            Currency currensy = _currencies
                    .Find<Currency>(currency => currency.Date == date)
                    .FirstOrDefault();
            
            return currensy == null ? false : true;
        }
        
        public Currency Get(string name, DateTime date)
        {
            const byte MAX_ATTEMPTS = 7;
            byte attempts = 0;
            Currency currensy = null;

            while ( currensy == null && attempts++ <= MAX_ATTEMPTS)
            {
                currensy = _currencies
                    .Find<Currency>(currency => currency.Date == date && currency.Name == name)
                    .FirstOrDefault();
                date = date.AddDays(-1);
            }
            return currensy;
        }

        public List<Currency> Create(List<Currency> currencies)
        {
            _currencies.InsertMany(currencies);
            return currencies;
        }

        public void Remove()
        {
            _currencies.DeleteMany(currncy => true);
        }
    }
}