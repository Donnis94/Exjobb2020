using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

public class Shared
{
	public static CosmosClient Client { get; private set; }
	static Shared()
	{
		var endpoint = "{insert your Azure Endpoint here}";
		var masterKey = "{Insert your Azure primary key here}";
		Client = new CosmosClient(endpoint, masterKey);
	}
}
