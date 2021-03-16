using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TG_Task3.Model;
using Newtonsoft.Json;
using System.Net.Mime;

namespace TG_Task3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class HotelRatesController : ControllerBase
    {
        /// <summary>
        /// Returns the filtered list of Hotel rates by HotelId and Arrival Date
        /// </summary>
        /// <param name="HotelId">e.g. 7294</param>
        /// <param name="ArrivalDate">e.g 2016-03-15. Time part is not used</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetHotelRatesList([FromQuery] int HotelId, [FromQuery] DateTime ArrivalDate)
        {
            // Import json
            // Get local json file path
            var hotelRatesjsonPath = Path.Combine(AppContext.BaseDirectory, "hotelsrates.json");

            // Read json file
            var hotelRates = System.IO.File.ReadAllText(hotelRatesjsonPath);

            // Deserialize to object for querying
            var hotelJsonList = JsonConvert.DeserializeObject<List<TGHotelRate>>(hotelRates);

            if (hotelJsonList != null && hotelJsonList.Any())
            {
                var hotelById = hotelJsonList.FirstOrDefault(x => x.Hotel.HotelID == HotelId);

                if (hotelById != null)
                {
                    // Comparing only the date part of the DateTime Object 
                    var filteredHotelRates = hotelById.HotelRates.Where(x => x.TargetDay.Date == ArrivalDate.Date).ToList();

                    if (filteredHotelRates != null && filteredHotelRates.Any())
                    {
                        return Ok(new TGHotelRate
                        {
                            Hotel = hotelById.Hotel,
                            HotelRates = filteredHotelRates
                        });
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
