using ServiceStack;
using MyApp.ServiceModel;

namespace MyApp.ServiceInterface
{
    public class GreetingServices : Service
    {
        public object Get(Greeting request)
        {
            return new GreetingResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}
