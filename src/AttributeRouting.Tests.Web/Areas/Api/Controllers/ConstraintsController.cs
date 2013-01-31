using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RoutePrefix("Constraints")]
    public class ConstraintsController : BaseApiController
    {
        [GET("Int/{x:int}")]
        public string Int(int x)
        {
            return "Int " + x;
        }

        [GET("IntOptional/{x:int?}")]
        public string IntOptional(int? x)
        {
            return "IntOptional " + x.GetValueOrDefault(-1);
        }

        [GET("IntDefault/{x:int=123}")]
        public string IntDefault(int x)
        {
            return "IntDefault " + x;
        }

        [GET("Query?{x:int}")]
        public string Query(int x)
        {
            return "Query " + x;
        }
    }
}
