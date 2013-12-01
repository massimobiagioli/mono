using System;
using System.Net;
using Newtonsoft.Json;

namespace SmartAgent
{
	public class GetDocumentRouteResolver : IRouteResolver
	{
		public GetDocumentRouteResolver()
		{
		}

		public string SendJson(HttpListenerContext context)
		{
			return JsonConvert.SerializeObject("ok");
		}
	}
}
