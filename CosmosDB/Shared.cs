using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

public class Shared
{
	public static CosmosClient Client { get; private set; }
	static Shared()
	{
		var endpoint = "https://exjobb2020.documents.azure.com:443/";
		var masterKey = "PWWJrJH9llc2YFY8WMUNZebljTjrkbdI1kiTESvBNm9VVpdNVJjbwPAzEOnwXrUVIm4Vthim4XvTEcMUcIdUaw==";
		Client = new CosmosClient(endpoint, masterKey);
	}
}
