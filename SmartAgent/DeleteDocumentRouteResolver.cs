using System;
using System.Net;
using Newtonsoft.Json;

namespace SmartAgent
{
	public class DeleteDocumentRouteResolver : IRouteResolver
	{
		public DeleteDocumentRouteResolver()
		{
		}

		public string SendJson(HttpListenerContext context)
		{
			return JsonConvert.SerializeObject("ok");
		}
	}
}
