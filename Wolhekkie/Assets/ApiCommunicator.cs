using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiCommunicator : MonoBehaviour
{
	public delegate void ModulesCallback(NetworkModule[] results);

	private string localAddress = "192.168.1.13";
	private string port = "5000";
	private string getWifiNetworkScansCall = "NetworksScan/network_modules";
	private string getBluetoothNetworkScansCall = "NetworksScan/GetBluetoothNetworkScans";

	private string wifiNetworkScansResult;
	private string bluetoothNetworkScansResult;

	public string getApiUrl()
	{
		return "http://" + localAddress + ":" + port + "/api/";
	}

	public string getWifiNetworkScansUrl()
	{
		return getApiUrl() + getWifiNetworkScansCall;
	}

	public string getBluetoothNetworkScansUrl()
	{
		return getApiUrl() + getBluetoothNetworkScansCall;
	}

	[ContextMenu("Test Bluetooth Result")]
	public void GetBluetoothData()
	{
		StartCoroutine(GetBluetoothResult());
	}

	private IEnumerator GetBluetoothResult()
	{
		Debug.Log("Trying to get result from :" + getBluetoothNetworkScansUrl());
		yield return null;
		using (UnityWebRequest request = UnityWebRequest.Get(getBluetoothNetworkScansUrl()))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError("Error getting bluetooth data: " + request.error);
			}
			else
			{
				// Handle the data received
				string responseData = request.downloadHandler.text;
				Debug.Log("Bluetooth result: " + responseData);
				bluetoothNetworkScansResult = responseData;

				// Optional: Parse JSON if needed
				// YourDataType data = JsonUtility.FromJson<YourDataType>(responseData);
			}
		}
	}

	[ContextMenu("Test Wifi Result")]
	public void GetWifiData(ModulesCallback callback = null)
	{
		StartCoroutine(GetWifiResult(callback));
	}

	private IEnumerator GetWifiResult(ModulesCallback callback = null)
	{
		Debug.Log("Trying to get result from :" + getWifiNetworkScansUrl());
		yield return null;
		using (UnityWebRequest request = UnityWebRequest.Get(getWifiNetworkScansUrl()))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError("Error getting wifi data: " + request.error);
			}
			else
			{
				// Handle the data received
				string responseData = request.downloadHandler.text;
				Debug.Log("Wifi result: " + responseData);
				wifiNetworkScansResult = responseData;
				var result = Newtonsoft.Json.JsonConvert.DeserializeObject<NetworkModule[]>(responseData);
				Debug.Log("done");
				if (callback != null)
				{
					callback(result);
				}
			}
		}
	}
}