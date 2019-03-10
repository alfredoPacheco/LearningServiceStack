using ServiceStack;
using Chapter1.ServiceModel;

namespace Chapter1.ServiceInterface
{
    public class GreetingServices : Service
    {
        public object Get(Greeting request)
        {
            return new GreetingResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}
