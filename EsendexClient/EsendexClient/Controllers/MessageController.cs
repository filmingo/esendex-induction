﻿using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;
using EsendexApi;
using EsendexApi.Clients;
using EsendexApi.Structures;
using EsendexClient.Models;
using EsendexClient.Settings;

namespace EsendexClient.Controllers
{
    public class MessageController : ApiController, IRequiresSessionState
    {
        private HttpSessionState Session { get { return HttpContext.Current.Session; } }
        private EsendexCredentials Credentials { get { return Session["credentials"] as EsendexCredentials; } }

        public async Task<IHttpActionResult> Post([FromBody] OutboundMessage message)
        {
            var restFactory = new RestFactory(AppSettings.EsendexEndpoint, Credentials.Username, Credentials.Password);
            var accountDetailses = (await new AccountClient(restFactory).GetAccounts());
            var accountRef = accountDetailses.Single().Reference;
            var submitResponse = await new MessageDispatcherClient(restFactory).SendMessage(accountRef, message);

            return Ok(submitResponse);
        }
    }
}