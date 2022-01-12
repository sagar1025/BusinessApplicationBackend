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
        public const string SessionKeyName = "_companies";
        private readonly IMemoryCache memoryCache;

        public ApplicationController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IList<AppClass> Apps = new List<AppClass>();
            bool hasData = memoryCache.TryGetValue(SessionKeyName, out Apps);
            if(hasData)
            {
                return Ok(JsonSerializer.Serialize(Apps));
            }
            return Ok(null);
        }

        [HttpPost]
        public IActionResult Index(AppClass req)
        {
            if (ValidateRequest(req))
            {
                IList<AppClass> data = new List<AppClass>();
                bool sessionData = memoryCache.TryGetValue(SessionKeyName, out data);
                if (sessionData)
                {
                    try
                    {
                        if (data != null && data.Any())
                        {
                            data.Add(req);
                            memoryCache.Set(SessionKeyName, data);
                            return Ok(new { Success = true });
                        }
                    }
                    catch(Exception e) { return Ok(new { Success = false }); }
                }
                else
                {
                    var list = new List<AppClass>();
                    list.Add(req);
                    memoryCache.Set(SessionKeyName, list);
                    return Ok(new { Success = true });
                }

                return Ok(new { Success = false });
            }
            return BadRequest();
        }


        [NonAction]
        public bool ValidateRequest(AppClass req)
        {
            if (req != null)
            {
                return !string.IsNullOrEmpty(req.Email) && !string.IsNullOrEmpty(req.BusinessName)
                    && !string.IsNullOrEmpty(req.ZipCode) && !string.IsNullOrEmpty(req.Industry)
                    && (req.AnnualSales >= 50000 && req.AnnualSales <= 200000) && (req.AnnualPayroll >= 50000 && req.AnnualPayroll <= 200000) && (req.NumberOfEmployees > -1 && req.NumberOfEmployees <= 100000);
            }
            return false;
        }
    }




}
