using System;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace SmartAgent
{
	public class ListAllRouteResolver : IRouteResolver
	{
		private const string DOC_FILTER = "*.html";

		public ListAllRouteResolver()
		{
		}

		public RouteResolverResponse SendResponse(HttpListenerContext context)
		{
			String path = SmartAgentHelper.GetMyDocumentsPath();
			List<string> filez = new List<string>(Directory.EnumerateFiles(path, DOC_FILTER));

			return new RouteResolverResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(filez));
		}
	}
}
