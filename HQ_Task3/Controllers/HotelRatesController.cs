using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HQ_Task3.Model;
using Newtonsoft.Json;

namespace HQ_Task3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelRatesController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetHotelRatesList([FromQuery] int HotelId, [FromQuery] DateTime ArrivalDate)
        {
            // Import json
            // Get local json file path
            var hotelRatesjsonPath = Path.Combine(AppContext.BaseDirectory, "hotelsrates.json");

            // Read json file
            var hotelRates = System.IO.File.ReadAllText(hotelRatesjsonPath);

            // Deserialize to object for querying
            var hotelJsonList = JsonConvert.DeserializeObject<List<HQHotelRate>>(hotelRates);

            if (hotelJsonList != null && hotelJsonList.Any())
            {
                var hotelById = hotelJsonList.FirstOrDefault(x => x.Hotel.HotelID == HotelId);

                if (hotelById != null)
                {
                    var res = hotelById.HotelRates.Where(x => x.TargetDay.Date == ArrivalDate.Date).ToList();

                    if (res != null && res.Any())
                    {
                        return Ok(res);
                    }
                    else
                    {
                        return NotFound(new { HotelId, ArrivalDate });
                    }
                }
                else
                {
                    return NotFound(new { HotelId });
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
