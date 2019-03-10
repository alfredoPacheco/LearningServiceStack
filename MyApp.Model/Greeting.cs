using ServiceStack;

namespace MyApp.ServiceModel
{
    [Route("/hello")]
    [Route("/hello/{Name}")]
    public class Greeting : IReturn<GreetingResponse>
    {
        public string Name { get; set; }
    }

    public class GreetingResponse
    {
        public string Result { get; set; }
    }
}
