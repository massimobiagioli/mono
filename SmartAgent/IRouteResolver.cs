using System;
using System.Net;

namespace SmartAgent
{
	public interface IRouteResolver
	{
		RouteResolverResponse SendResponse(HttpListenerContext context);
	}
}
