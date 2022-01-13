using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using System.Data;
using Microsoft.Extensions.Caching.Memory;
using BusinessApplicationBackend.Models;

namespace BusinessApplicationBackend.Controllers
{
    [Route("/applications")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        public const string CacheKeyName = "_companies";
        private readonly IMemoryCache memoryCache;

        public ApplicationController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IList<AppClass> Apps = new List<AppClass>();
            bool hasData = memoryCache.TryGetValue(CacheKeyName, out Apps);

            return hasData ? Ok(JsonSerializer.Serialize(Apps)) : Ok(null);
        }

        [HttpPost]
        public IActionResult Index(AppClass req)
        {
            if (ValidateRequest(req))
            {
                IList<AppClass> data = new List<AppClass>();
                bool sessionData = memoryCache.TryGetValue(CacheKeyName, out data);
                if (sessionData)
                {
                    try
                    {
                        if (data != null && data.Any())
                        {
                            data.Add(req);
                            memoryCache.Set(CacheKeyName, data);
                            return Ok(new { Success = true });
                        }
                    }
                    catch(Exception e) { return Ok(new { Success = false }); }
                }
                else
                {
                    var list = new List<AppClass>();
                    list.Add(req);
                    memoryCache.Set(CacheKeyName, list);
                    return Ok(new { Success = true });
                }

                return Ok(new { Success = false });
            }
            return BadRequest(new { Success = false, Message = "Please check the values provided" });
        }


        [NonAction]
        public bool ValidateRequest(AppClass req)
        {
            if (req != null)
            {
                return !string.IsNullOrWhiteSpace(req.Email) && !string.IsNullOrWhiteSpace(req.BusinessName)
                    && !string.IsNullOrWhiteSpace(req.ZipCode) && !string.IsNullOrWhiteSpace(req.Industry)
                    && (req.AnnualSales >= 50000 && req.AnnualSales <= 200000) && (req.AnnualPayroll >= 50000 && req.AnnualPayroll <= 200000) && (req.NumberOfEmployees > -1 && req.NumberOfEmployees <= 100000);
            }
            return false;
        }
    }

}
