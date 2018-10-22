using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using SmartHotel.Services.BookingProcesor.Models;
using System.Threading.Tasks;
using SmartHotel.Services.BookingProcesor.Services;
using SmartHotel.Services.BookingProcesor.Exceptions;

namespace SmartHotel.Services.BookingProcesor
{
    public static class ProcessBooking
    {
        [FunctionName("ProcessBooking")]
        public async static Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "HttpTriggerCSharp/name/{name}")]HttpRequestMessage req, string name, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var bookingProcesor = new Services.BookingProcesor();
            var bookingRequest = await req.Content.ReadAsAsync<BookingRequest>();

            try
            {
                await bookingProcesor.ProcessAsync(bookingRequest);
                return req.CreateResponse(HttpStatusCode.OK, "Booking created");
            }
            catch (BookingException ex) when (ex.Code == BookingExceptionCode.HotelNotFound)
            {
                return req.CreateErrorResponse(HttpStatusCode.NotFound,ex.Message);
            }
            catch (BookingException ex) when (
                    ex.Code == BookingExceptionCode.GuestsRoomsMissmatch
                    || ex.Code == BookingExceptionCode.InvalidPrice
                )
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
