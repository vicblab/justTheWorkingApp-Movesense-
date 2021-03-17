using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


//---------------------------------------------------------------------------------
// Our own example for connection with the sensors, hopefully more user friendly :D
//---------------------------------------------------------------------------------


public class TUASexample : MonoBehaviour
{


	// Movesense sensor's MAC addresses, maybe get them through the Movesense Showcase App

	[SerializeField] public List<string> knownAddresses = new List<string>();

	

	[SerializeField] private Text dataDisplay;


	// We add our custom events to the Scan and sensor so whenever something invokes them our events are invoked as well
	private void Awake()
	{
		ScanController.Event += OnScanControllerCallbackEvent;
		MovesenseController.Event += OnMovesenseControllerCallbackEvent;

		//just for starting, my sensor:
		knownAddresses.Add("0C:8C:DC:36:0D:A0");
	}

	// Use this for initialization, here we already started scanning
	void Start()
	{
		StartCoroutine(StartScanning());
	}

	// Update is called once per frame
	void Update()
	{

	}


	// This event occurs when some new device is found, so we only connect to devices which MAC address is on our knownAddresses list

	void OnScanControllerCallbackEvent(object sender, ScanController.EventArgs e)
	{
		//Debug.Log("OnScanControllerCallbackEvent, Type: " + e.Type + ", invoked by: " + e.InvokeMethod);
		switch (e.Type)
		{
			case ScanController.EventType.NEW_DEVICE:
				Debug.Log("OnScanControllerCallbackEvent, NEW_DEVICE with MacID: " + e.MacID + ", connecting...");

				dataDisplay.text = "Not ours";
				dataDisplay.text += e.MacID.ToString();

				//if (knownAddresses.Contains(e.MacID.ToString()))
				//{
					StartCoroutine(Connect(e.MacID));
					
				//}
				break;
		}
	}


	// Here is where the magic begins... Once the device is connected, we subscribe to its linear acceleration, and we retrieve its data from ne.Data
	// all possible subscriptions are:
	/*SubscriptionPath.LinearAcceleration 
	SubscriptionPath.AngularVelocity 
	SubscriptionPath.MagneticField 
	SubscriptionPath.HeartRate 
	SubscriptionPath.Temperature 
*/
	void OnMovesenseControllerCallbackEvent(object sender, MovesenseController.EventArgs e)
	{
		//Debug.Log("OnMovesenseControllerCallbackEvent, Type: " + e.Type + ", invoked by: " + e.InvokeMethod);
		switch (e.Type)
		{
			case MovesenseController.EventType.CONNECTING:
				for (int i = 0; i < e.OriginalEventArgs.Count; i++)
				{
					var ce = (ConnectCallback.EventArgs)e.OriginalEventArgs[i];
					dataDisplay.text = "Yeah, connecting";
					Debug.Log("OnMovesenseControllerCallbackEvent, CONNECTING " + ce.MacID);
				}
				break;
			case MovesenseController.EventType.CONNECTED:
				for (int i = 0; i < e.OriginalEventArgs.Count; i++)
				{
					var ce = (ConnectCallback.EventArgs)e.OriginalEventArgs[i];

					Debug.Log("OnMovesenseControllerCallbackEvent, CONNECTED " + ce.MacID + ", subscribing linearAcceleration");

					MovesenseController.Subscribe(ce.Serial, SubscriptionPath.LinearAcceleration, SampleRate.slowest);
					dataDisplay.text = "Yeah, connected";
				}
				break;
			case MovesenseController.EventType.NOTIFICATION:
				for (int i = 0; i < e.OriginalEventArgs.Count; i++)
				{
					var ne = (NotificationCallback.EventArgs)e.OriginalEventArgs[i];

					Debug.Log("OnMovesenseControllerCallbackEvent, NOTIFICATION for " + ne.Serial + ", SubscriptionPath: " + ne.Subscriptionpath + ", Data: " + ne.Data);

					// here we display the data
					dataDisplay.text = ne.Data.ToString();
				}
				break;
		}
	}

	IEnumerator StartScanning()
	{
		if (ScanController.IsInitialized)
		{
			yield return 0;
			ScanController.StartScan();
		}
		else
		{
			yield return new WaitForSeconds(0.1F); // wait for ScanController to be initialized
			ScanController.StartScan();
		}
	}

	IEnumerator Connect(string macID)
	{
		if (MovesenseController.isInitialized)
		{

			//yield return 0;
			dataDisplay.text = "Sensor Initialized";
			MovesenseController.Connect(macID);
		}
		else
		{

			dataDisplay.text = "Sensor not Initialized";
			yield return new WaitForSeconds(1F); // wait for MovesenseController to be initialized
			dataDisplay.text = "time waited";
			MovesenseController.Connect(macID);
		}
	}
}
