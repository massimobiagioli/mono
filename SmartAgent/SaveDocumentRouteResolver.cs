using System;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using System.Collections.Specialized;

namespace SmartAgent
{
	public class SaveDocumentRouteResolver : IRouteResolver
	{
		private const string ERR_MISSING_PARAMETER_FILENAME = "Parametro fileName mancante";
		private const string ERR_MISSING_PARAMETER_CONTENT = "Parametro content mancante";
		private const string KEY_FILENAME = "fileName";
		private const string KEY_CONTENT = "content";

		public SaveDocumentRouteResolver()
		{
		}

		public RouteResolverResponse SendResponse(HttpListenerContext context)
		{
			RouteResolverResponse response = new RouteResolverResponse();
			String path = SmartAgentHelper.GetMyDocumentsPath();
			NameValueCollection nvc = SmartAgentHelper.ParseQueryString(context);
			string fileName = nvc.Get(KEY_FILENAME);
			string content = nvc.Get(KEY_CONTENT);

			if (String.IsNullOrEmpty(fileName)) 
			{
				response.Status = 404;
				response.Message = JsonConvert.SerializeObject(ERR_MISSING_PARAMETER_FILENAME);
				return response;
			} 

			if (String.IsNullOrEmpty(content)) 
			{
				response.Status = 404;
				response.Message = JsonConvert.SerializeObject(ERR_MISSING_PARAMETER_CONTENT);
				return response;
			} 

			try 
			{
				byte[] decoded = Convert.FromBase64String(content);
				string fullPath = path + Path.DirectorySeparatorChar + fileName;
				File.WriteAllBytes(fullPath, decoded);
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
