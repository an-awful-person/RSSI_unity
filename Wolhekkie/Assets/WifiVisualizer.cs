using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WifiVisualizer : MonoBehaviour
{
	[SerializeField]
	private ApiCommunicator communicator;

	[SerializeField]
	private NetworkModule[] foundModules;

	[SerializeField]
	private GameObject[] modulePositions;

	[ContextMenu("TestRefresh")]
	private void RefreshData()
	{
		communicator.GetWifiData(SetData);
	}

	private void SetData(NetworkModule[] modules)
	{
		foundModules = modules;
		Debug.Log("Modules has been set");
	}

	[ContextMenu("Try Triangulation")]
	private void TriangulateResults()
	{
		foreach (var res in foundModules[0].scannedNetworks)
		{
			var accordingNetworks = new List<ScannedNetwork>();
			for (int i = 0; i < foundModules.Length; i++)
			{
				var found = foundModules[i].scannedNetworks.FirstOrDefault(module => module.scannedModuleSSID == res.scannedModuleSSID);
				if (found != null)
				{
					accordingNetworks.Add(found);
				}
			}
			if (accordingNetworks.Count >= 3)
			{
				Vector3 center = Vector3.zero;
				foreach (var mp in modulePositions)
				{
					var pos = mp.transform.position;
					center += pos;
				}

				center = center / modulePositions.Length;

				float maxStrength = 0;
				foreach (var nw in accordingNetworks)
				{
					var positive = Mathf.Abs(nw.RSSI);
					if (positive > maxStrength)
					{
						maxStrength = positive;
					}
				}

				List<Vector3> plottingPoints = new List<Vector3>();
				for (int i = 0; i < accordingNetworks.Count; ++i)
				{
					var t = Mathf.Abs(accordingNetworks[i].RSSI) / maxStrength;
					plottingPoints.Add(Vector3.Lerp(center, modulePositions[i].transform.position, t));
				}
				var location = Vector3.zero;
				foreach (var p in plottingPoints)
				{
					location += p;
				}
				location /= plottingPoints.Count;

				var placement = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				placement.transform.position = location;
			}
		}
	}
}