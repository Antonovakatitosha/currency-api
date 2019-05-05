using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CurrencyApi.Controllers;
using CurrencyApi.Services;
using System.Configuration;

namespace CurrencyApi
{
    public class Program
    {
         public static void Main(string[] args)
        {
            // IConfiguration config = ConfigurationManager.AppSettings;
            // CurrencyService currencyService = new CurrencyService(config);
            // ReceiveDataController receiveDataController = new ReceiveDataController(currencyService);
            // receiveDataController.FillDb();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        // static async void FillDb()
        // {
        //     using (HttpClient client = new HttpClient())
        //     {
        //         try	
        //         {
        //             String uri = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year=2018";
        //             string responseBody = await client.GetStringAsync(uri);
        //             Console.WriteLine(responseBody);
        //         }  
        //         catch(HttpRequestException e)
        //         {
        //             Console.WriteLine("\nException Caught!");	
        //             Console.WriteLine("Message :{0} ",e.Message);
        //         }
        //     }
        // }
    }
}
