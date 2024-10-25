using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkModules
{
	public List<NetworkModule> Modules { get; set; }
}

[Serializable]
public class NetworkModule
{
	[field: SerializeField]
	public string macAddress { get; set; }

	[field: SerializeField]
	public List<ScannedNetwork> scannedNetworks { get; set; } = new List<ScannedNetwork>();
}