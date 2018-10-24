using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToTwitter;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sorteio.API.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sorteio.API.Controllers
{
    [Route("api/sorteio")]
    [EnableCors("AllowSpecificOrigin")]
    public class TwitterController : Controller
    {

        //private readonly IAuthorizer _authorizer;
        private readonly IHostingEnvironment _hostingEnvironment;
        
        public TwitterController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
 
        [HttpGet]
        public Response<Twitter> BuscarTweets()
        {
            var response = new Response<Twitter>();

            try
            {
                var auth = new MvcAuthorizer
                {
                    CredentialStore = new InMemoryCredentialStore
                    {
                        ConsumerKey = "###############",
                        ConsumerSecret = "##############",
                        OAuthToken = OAuthKeys.TwitterAccessToken,
                        OAuthTokenSecret = OAuthKeys.TwitterAccessTokenSecret,
                    }
                };
                auth.AuthorizeAsync();

                var ctx = new TwitterContext(auth);

                List<Search> searchResponse =
                 (from search in ctx.Search
                  where search.Type == SearchType.Search &&
                        search.Query == "#vexpro"
                  select search)
                  .ToList();

                var rnd = new Random();
                var tweet = searchResponse[rnd.Next(searchResponse.Count)];

                if (tweet != null)
                {
                    response.Result = new Twitter
                    {
                        status = tweet.Statuses.ToString()
                    };
                    return response;
                };
                
            }
            catch (Exception)
            {

                response.Add("Ocorreu um problema ");
                return response;
            }
        return response;
        }
                      
    }

}

    


