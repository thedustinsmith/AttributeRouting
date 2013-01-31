using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RoutePrefix("plain")]
    public class PlainController : BaseApiController
    {
        // GET /api/plain
        [GET("")]
        public IEnumerable<string> All()
        {
            return new [] { "value1", "value2" };
        }

        // GET /api/plain/5
        [GET("{id}")]
        public string Single(int id)
        {
            return "value";
        }

        // POST /api/plain
        [POST("")]
        public HttpResponseMessage Create()
        {
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        // PUT /api/plain/5
        [PUT("{id}")]
        public HttpResponseMessage Update(int id)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Updated " + id)
            };

            return response;
        }

        // DELETE /api/plain/5
        [DELETE("{id}")]
        public HttpResponseMessage Destroy(int id)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Deleted " + id)
            };

            return response;
        }
    }
}