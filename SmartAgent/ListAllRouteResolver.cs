using System;
using System.Net;
using Newtonsoft.Json;

namespace SmartAgent
{
	public class ListAllRouteResolver : IRouteResolver
	{
		public ListAllRouteResolver()
		{
		}

		public string SendJson(HttpListenerContext context)
		{
			return JsonConvert.SerializeObject("ok");
		}
	}
}
