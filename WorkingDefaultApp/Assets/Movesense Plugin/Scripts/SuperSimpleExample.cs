using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SuperSimpleExample : MonoBehaviour {


	[SerializeField] private Text text;
	private void Awake() {
		ScanController.Event += OnScanControllerCallbackEvent;
		MovesenseController.Event += OnMovesenseControllerCallbackEvent;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(Connect("0C:8C:DC:36:08:F8"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnScanControllerCallbackEvent(object sender, ScanController.EventArgs e) {
		//Debug.Log("OnScanControllerCallbackEvent, Type: " + e.Type + ", invoked by: " + e.InvokeMethod);
		switch (e.Type) {
			case ScanController.EventType.NEW_DEVICE:
				Debug.Log("OnScanControllerCallbackEvent, NEW_DEVICE with MacID: "+e.MacID+", connecting...");
				StartCoroutine(Connect(e.MacID));

			break;
		}
	}

	void OnMovesenseControllerCallbackEvent(object sender, MovesenseController.EventArgs e) {
		//Debug.Log("OnMovesenseControllerCallbackEvent, Type: " + e.Type + ", invoked by: " + e.InvokeMethod);
		switch (e.Type) {
			case MovesenseController.EventType.CONNECTING:
				for (int i = 0; i < e.OriginalEventArgs.Count; i++) {
					var ce = (ConnectCallback.EventArgs) e.OriginalEventArgs[i];

					Debug.Log("OnMovesenseControllerCallbackEvent, CONNECTING " + ce.MacID);
				}
			break;
			case MovesenseController.EventType.CONNECTED:
				for (int i = 0; i < e.OriginalEventArgs.Count; i++) {
					var ce = (ConnectCallback.EventArgs) e.OriginalEventArgs[i];

					Debug.Log("OnMovesenseControllerCallbackEvent, CONNECTED " + ce.MacID + ", subscribing linearAcceleration");
					
					MovesenseController.Subscribe(ce.Serial, SubscriptionPath.LinearAcceleration, SampleRate.slowest);
				}
			break;
			case MovesenseController.EventType.NOTIFICATION:
				for (int i = 0; i < e.OriginalEventArgs.Count; i++) {
					var ne = (NotificationCallback.EventArgs) e.OriginalEventArgs[i];
					
					Debug.Log("OnMovesenseControllerCallbackEvent, NOTIFICATION for " + ne.Serial + ", SubscriptionPath: " + ne.Subscriptionpath + ", Data: " + ne.Data);
					text.text = ne.Data.ToString();

				}
			break;
		}
	}

	IEnumerator StartScanning() {
		if (ScanController.IsInitialized) {
			yield return 0;
			ScanController.StartScan();
		} else {
			yield return new WaitForSeconds(0.1F); // wait for ScanController to be initialized
			ScanController.StartScan();
		}
	}

	IEnumerator Connect(string macID) {

		

		if (MovesenseController.isInitialized) {
			yield return 0;
			MovesenseController.Connect(macID);
		} else {
			yield return new WaitForSeconds(0.1F); // wait for MovesenseController to be initialized
			MovesenseController.Connect(macID);
		}
	}
}
