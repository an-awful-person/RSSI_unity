using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScannedNetwork
{
	[field: SerializeField]
	public string scanningModuleSSID { get; set; }

	[field: SerializeField]
	public string scannedModuleName { get; set; }

	[field: SerializeField]
	public string scannedModuleSSID { get; set; }

	[field: SerializeField]
	public int RSSI { get; set; }

	[field: SerializeField]
	public DateTime DateTime { get; } = DateTime.Now;
}