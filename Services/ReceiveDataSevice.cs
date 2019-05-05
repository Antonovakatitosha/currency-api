using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using CurrencyApi.Models;
using CurrencyApi.Services;
using System.Globalization;

class TableHead
{
    public readonly string Name;
    public readonly double Unit;

    public TableHead(double unit, string name)
    {
        Name = name;
        Unit = unit;
    }
}

namespace CurrencyApi.Controllers
{
    public class ReceiveDataService
    {
        private readonly CurrencyService _currencyService;
        
        public ReceiveDataService(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<Boolean> FillDb(int year = 2018)
        {
            string dataTable = await GetExternalData(year);
            if(dataTable == null) return false;
            
            string[] tableStrings = dataTable.Split( '\n' );
            List<TableHead> tableHead = GetTableHead(tableStrings[0]);
            return TableSerialization(tableHead, tableStrings);
        }

        static async Task<string> GetExternalData(int year)
        {
            using (HttpClient client = new HttpClient())
            {
                try	
                {
                    String uri = $"https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year={year}";
                    string responseBody = await client.GetStringAsync(uri);
                    // Console.WriteLine(responseBody);
                    return responseBody;
                }  
                catch(HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");	
                    Console.WriteLine("Message :{0} ",e.Message);
                }
                return null;
            }
        }

        static List<TableHead> GetTableHead(string tableStrings)
        {
            List<TableHead> tableHead = new List<TableHead>();
            string[] headElems = tableStrings.Split( '|' );

            for (var i = 1; i < headElems.Length; i++)
            {
                int spaceСharacter = headElems[i].IndexOf(' ');
                string unit = headElems[i].Substring(0, spaceСharacter);
                string name = headElems[i].Substring(++spaceСharacter);
                tableHead.Add(new TableHead(Int32.Parse(unit), name));
            }
            return tableHead;
        }

        Boolean TableSerialization( List<TableHead> tableHead, string[] tableStrings)
        {
            for(var i = 1; i < tableStrings.Length; i++)
            {
                string[] rowElems = tableStrings[i].Split( '|' );
                if(rowElems.Length < tableHead.Count) break;

                DateTime date = DateTime.ParseExact(rowElems[0], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                if(_currencyService.Get(date)) break;

                List<Currency> currencies = new List<Currency>();
                for(var j = 0; j < tableHead.Count; j++)
                {
                    TableHead headElem = tableHead[j];
                    currencies.Add(new Currency(){
                        Date = date,
                        Name = headElem.Name,
                        Price = (decimal)double.Parse(rowElems[j+1]),
                        Units = headElem.Unit
                    });
                }
                _currencyService.Create(currencies);
            }
            return true;
        }
    }
}
