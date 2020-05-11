using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace elasticsearch
{
    public class Shared
    {
		public static ElasticClient Client { get; private set; }
		private const string ElasticUri = "https://elastic:tGuN5QwaqyrTt79lFvaPIrA5@9d7877f17a72445ca79a84a5cea45f3f.eastus2.azure.elastic-cloud.com:9243";

		static Shared(){
			var uri = new Uri(ElasticUri);
			var settings = new ConnectionSettings(uri);
			var client = new ElasticClient(settings);
			Client = client;
		}

	}
}
