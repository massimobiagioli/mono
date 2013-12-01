using System;

namespace SmartAgent
{
	public class RouteEntry
	{
		private string clazz;
		private string verb;

		public RouteEntry()
		{
			this.verb = string.Empty;
			this.clazz = string.Empty;
		}

		public RouteEntry(string verb, string clazz)
		{
			this.verb = verb;
			this.clazz = clazz;
		}

		public string Clazz {
			get {
				return clazz;
			}
			set {
				clazz = value;
			}
		}

		public string Verb {
			get {
				return verb;
			}
			set {
				verb = value;
			}
		}

	}
}

