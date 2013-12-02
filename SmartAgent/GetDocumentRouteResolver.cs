using System;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace SmartAgent
{
	public class GetDocumentRouteResolver : IRouteResolver
	{
		private const string ERR_MISSING_PARAMETER_FILENAME = "Parametro fileName mancante";

		public GetDocumentRouteResolver()
		{
		}

		public RouteResolverResponse SendResponse(HttpListenerContext context)
		{
			RouteResolverResponse response = new RouteResolverResponse();
			String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			string fileName = context.Request.QueryString.Get("fileName");

			if (null == fileName) 
			{
				response.Status = 404;
				response.Message = JsonConvert.SerializeObject(ERR_MISSING_PARAMETER_FILENAME);
			} 
			else 
			{
				string encoded = Convert.ToBase64String(File.ReadAllBytes(path + Path.DirectorySeparatorChar + fileName));
				response.Status = (int)HttpStatusCode.OK;
				response.Message = JsonConvert.SerializeObject(encoded);
			}

			return response;
		}
	}
}
