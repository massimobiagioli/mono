using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace SmartAgent
{
	public class SmartAgentHelper
	{

		public static string GetMyDocumentsPath() 
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}

		public static NameValueCollection ParseQueryString(HttpListenerContext context)
		{
			String body = new StreamReader(context.Request.InputStream).ReadToEnd();
			return HttpUtility.ParseQueryString(body);
		}

	}
}

