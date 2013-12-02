using System;
using System.Net;
using Newtonsoft.Json;

namespace SmartAgent
{
	public class SaveDocumentRouteResolver : IRouteResolver
	{
		public SaveDocumentRouteResolver()
		{
		}

		public RouteResolverResponse SendResponse(HttpListenerContext context)
		{
			return new RouteResolverResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject("ok"));
		}
	}
}
