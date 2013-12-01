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

		public string SendJson(HttpListenerContext context)
		{
			return JsonConvert.SerializeObject("ok");
		}
	}
}
