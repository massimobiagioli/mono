using System;
using System.Net;

namespace SmartAgent
{
	public interface IRouteResolver
	{
		string SendJson(HttpListenerContext context);
	}
}
