using System;

namespace SmartAgent
{
	public class RouteResolverResponse
	{

		private int status;
		private string message;

		public RouteResolverResponse()
		{
		}

		public RouteResolverResponse(int status, string json) 
		{
			this.status = status;
			this.message = json;
		}
		
		public int Status {
			get {
				return status;
			}
			set {
				status = value;
			}
		}

		public string Message {
			get {
				return message;
			}
			set {
				message = value;
			}
		}
	}
}

