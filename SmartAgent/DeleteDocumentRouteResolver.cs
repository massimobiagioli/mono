using System;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Specialized;
using System.Web;

namespace SmartAgent
{
	public class DeleteDocumentRouteResolver : IRouteResolver
	{
		private const string ERR_MISSING_PARAMETER_FILENAME = "Parametro fileName mancante";
		private const string KEY_FILENAME = "fileName";

		public DeleteDocumentRouteResolver()
		{
		}

		public RouteResolverResponse SendResponse(HttpListenerContext context)
		{
			RouteResolverResponse response = new RouteResolverResponse();
			String path = SmartAgentHelper.GetMyDocumentsPath();
			NameValueCollection nvc = SmartAgentHelper.ParseQueryString(context);
			string fileName = nvc.Get(KEY_FILENAME);

			if (String.IsNullOrEmpty(fileName)) 
			{
				response.Status = 404;
				response.Message = JsonConvert.SerializeObject(ERR_MISSING_PARAMETER_FILENAME);
				return response;
			} 

			try 
			{
				string fullPath = path + Path.DirectorySeparatorChar + fileName;
				File.Delete(fullPath);
				response.Status = (int)HttpStatusCode.OK;
				response.Message = JsonConvert.SerializeObject(fullPath);
			}
			catch (Exception e) {
				response.Status = 500;
				response.Message = JsonConvert.SerializeObject(e.Message);
			}

			return response;
		}
	}
}
