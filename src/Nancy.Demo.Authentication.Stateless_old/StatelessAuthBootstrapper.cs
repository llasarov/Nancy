using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using TinyIoC;

namespace Nancy.Demo.Authentication.Stateless
{
    public class StatelessAuthBootstrapper : DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            // At request startup we modify the request pipelines to
            // include stateless authentication
            //
            // Configuring stateless authentication is simple. Just use the 
            // NancyContext to get the apiKey. Then, use the apiKey to get 
            // your user's identity.
            var configuration =
                new StatelessAuthenticationConfiguration(nancyContext =>
                    {
                        //for now, we will pull the apiKey from the querystring, 
                        //but you can pull it from any part of the NancyContext
                        var apiKey = (string) nancyContext.Request.Query.ApiKey.Value;

                        //get the user identity however you choose to (for now, using a static class/method)
                        return UserDatabase.GetUserFromApiKey(apiKey);
                    });

            //pipelines.AfterRequest.AddItemToEndOfPipeline(x=>
            //    {
            //        x.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //    });

            StatelessAuthentication.Enable(pipelines, configuration);
        }
    }
}