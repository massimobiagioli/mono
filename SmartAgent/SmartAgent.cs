using System;
using System.Net;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Xml;
using System.Text;

namespace SmartAgent
{
	class SmartAgent
	{
		private const string CONFIG_KEY_PATH_CONFIG = "PathConfig";
		private const string CONFIG_KEY_ROUTES_FILENAME = "RoutesFileName";
		private const string CONFIG_KEY_HOST = "Host";
		private const string ERR_HTTP_METHOD_NOT_SUPPORTED = "HTTP METHOD NOT SUPPORTED!";

		private string startUpPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		private string configPath = string.Empty;
		private HttpListener listener = new HttpListener();
		private Dictionary<string, RouteEntry> routes = new Dictionary<string, RouteEntry>();

		public SmartAgent() 
		{
			this.configPath = startUpPath + Path.DirectorySeparatorChar + ConfigurationManager.AppSettings[CONFIG_KEY_PATH_CONFIG] + Path.DirectorySeparatorChar;
		}

		public void Run() 
		{
			this.listener.Start();

			this.LoadRoutes();
			this.InitPrefixes();

			Thread t = new Thread(new ThreadStart(this.ClientListener));
			t.Start();

			while (true)
			{
				Console.ReadLine();
			}
		}

		private void LoadRoutes()
		{
			try
			{
				routes.Clear();                

				XmlDocument xml = new XmlDocument();
				xml.Load(configPath + ConfigurationManager.AppSettings[CONFIG_KEY_ROUTES_FILENAME]);
				XmlNodeList nodeList = xml.SelectNodes("routes/route");
				foreach (XmlNode node in nodeList)
				{
					routes.Add(node.Attributes["key"].Value, 
						new RouteEntry(node.Attributes["verb"].Value, node.Attributes["clazz"].Value));
				}                
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private void InitPrefixes()
		{
			listener.Prefixes.Clear();

			foreach (KeyValuePair<string, RouteEntry> entry in routes)
			{                
				listener.Prefixes.Add(ConfigurationManager.AppSettings[CONFIG_KEY_HOST] + "/" + entry.Key);                
			}
		}

		private void ClientListener()
		{
			while (true)
			{
				try
				{
					HttpListenerContext request = this.listener.GetContext();
					ThreadPool.QueueUserWorkItem(this.ProcessRequest, request);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		private void ProcessRequest(object listenerContext)
		{
			var context = (HttpListenerContext)listenerContext;

			try
			{    
				String url = this.ExtractUrl(context);

				if (!this.routes[url].Verb.Equals(context.Request.HttpMethod))
				{
					Console.WriteLine(ERR_HTTP_METHOD_NOT_SUPPORTED);
					this.HandleJsonpResponse(context, 404, ERR_HTTP_METHOD_NOT_SUPPORTED);
					return;
				}

				this.WriteRequestLog(url, context);
				this.HandleJsonpResponse(context, (int)HttpStatusCode.OK, this.DispatchRequest(url, context));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				this.HandleJsonpResponse(context, 500, e.Message);
			}
		}

		private string ExtractUrl(HttpListenerContext context)
		{
			String url = context.Request.RawUrl;
			if (url.Contains("?"))
			{
				url = url.Substring(1, url.IndexOf("?") - 1);
			}
			if (url.StartsWith("/"))
			{
				url = url.Substring(1, url.Length - 1);
			}

			return url;
		}

		private void HandleJsonpResponse(HttpListenerContext context, int statusCode, string toPrint)
		{
			string callback = context.Request.QueryString.Get("callback");

			toPrint = callback + "(" + toPrint + ")";

			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(toPrint);
			context.Response.ContentLength64 = bytes.Length;
			context.Response.StatusCode = statusCode;
			using (Stream s = context.Response.OutputStream)
			{
				s.Write(bytes, 0, bytes.Length);
			}
		}
		
		private string DispatchRequest(string url, HttpListenerContext context)
		{            
			IRouteResolver resolver = (IRouteResolver)Assembly.GetExecutingAssembly().CreateInstance(routes[url].Clazz);

			return resolver.SendJson(context);
		}

		private void WriteRequestLog(string url, HttpListenerContext context)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(String.Format("{0:dd-MM-yy hh:mm:ss}", DateTime.Now))
				.Append("\t")
				.Append(url)
				.Append("\t")
				.Append(context.Request.UserAgent);

			Console.WriteLine(sb.ToString());
		}

		public static void Main (string[] args)
		{
			SmartAgent agent = new SmartAgent();
			agent.Run();
		}
	}
}
