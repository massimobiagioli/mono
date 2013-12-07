using System;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace SmartAgent
{
	public class GetDocumentRouteResolver : IRouteResolver
	{
		private const string ERR_MISSING_PARAMETER_FILENAME = "Parametro fileName mancante";
		private const string KEY_FILENAME = "fileName";

		public GetDocumentRouteResolver()
		{
		}

		public RouteResolverResponse SendResponse(HttpListenerContext context)
		{
			RouteResolverResponse response = new RouteResolverResponse();
			String path = SmartAgentHelper.GetMyDocumentsPath();
			string fileName = context.Request.QueryString.Get(KEY_FILENAME);

			if (null == fileName) 
			{
				response.Status = 404;
				response.Message = JsonConvert.SerializeObject(ERR_MISSING_PARAMETER_FILENAME);
				return response;
			} 

			try
			{
				string fullPath = path + Path.DirectorySeparatorChar + fileName;
				string encoded = Convert.ToBase64String(File.ReadAllBytes(fullPath));
				response.Status = (int)HttpStatusCode.OK;
				response.Message = JsonConvert.SerializeObject(encoded);
			}
			catch (Exception e) 
			{
				response.Status = 500;
				response.Message = JsonConvert.SerializeObject(e.Message);
			}

			return response;
		}
	}
}
