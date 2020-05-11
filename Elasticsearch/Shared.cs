using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace elasticsearch
{
    public class Shared
    {
		public static ElasticClient Client { get; private set; }
		private const string ElasticUri = "https://{username}:{password}{elasticsearch endpoint}";

		static Shared(){
			var uri = new Uri(ElasticUri);
			var settings = new ConnectionSettings(uri);
			var client = new ElasticClient(settings);
			Client = client;
		}

	}
}
