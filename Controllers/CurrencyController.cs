using System.Collections.Generic;
using CurrencyApi.Models;
using CurrencyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;
using System;
using System.Net.Http;

public class Response<T>
{
    public readonly T Data;
    public readonly bool Success;

    public Response(bool success = true, T data = default(T))
    {
        Success = success;
        Data = data;
    }
}

namespace CurrencyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        private readonly ReceiveDataService _receiveDataService;

        public CurrencyController(  CurrencyService currencyService, 
                                    ReceiveDataService receiveDataService)
        {
            _currencyService = currencyService;
            _receiveDataService = receiveDataService;
        }

        [HttpPost("fill/{year:length(4)}")]
        public async Task<Response<bool>> Fill(string year)
        {
            try
            {
                bool result = await _receiveDataService.FillDb(Int32.Parse(year));
                return new Response<bool>(true, result);
            }
            catch { return new Response<bool>(false); }
        }

        [HttpGet("all")]
        public Response<ActionResult<List<Currency>>> Get()
        {
            try
            {
                var result = _currencyService.Get();
                return new Response<ActionResult<List<Currency>>>(true, result);
            }
            catch { return new Response<ActionResult<List<Currency>>>(false); }
        }

        [HttpGet]
        [EnableCors]
        public Response<ActionResult<Currency>> Get([FromQuery]string name, [FromQuery]string date)
        {
            try
            {
                DateTime convertDate = DateTime.Parse(date);
                var result = _currencyService.Get(name, convertDate);
                return new Response<ActionResult<Currency>>(true, result);
            }
            catch { return new Response<ActionResult<Currency>>(false); }
        }

        [HttpDelete("all")]
        public Response<bool> Delete()
        {
            try
            {
                _currencyService.Remove();
                return new Response<bool>(true, true);
            }
            catch { return new Response<bool>(false); }
        }
    }
}