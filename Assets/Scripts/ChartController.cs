using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartController : MonoBehaviour {

	public float chartWidth = 1000;
	public float chartHeight = 1500;
	public int divisionsX = 20;
	public int divisionsY = 10;
	public float updatesPerSecond = 10;
	float msSince = 0;
	float time = 0;
	private GameObject outline;
	private MarketData currData;
	private MainController mc;

	// Use this for initialization
	void Start () {
		mc = GameObject.Find ("MainController").GetComponent<MainController> ();
		outline = GameObject.Find ("outline");
		currData = mc.BitcoinData;
		print ("FIRST IS " + currData.priceAt(4));
	}

	void updateChart(){
		GameObject[] disposableGOs = GameObject.FindGameObjectsWithTag ("disposable");
		 //should improve this to just change the current objects instead of disposing every time
		foreach (GameObject go in disposableGOs){
			GameObject.Destroy (go); //get rid of old candlesticks
		}

		float startX = (chartWidth / 2 * -1) + outline.GetComponent<RectTransform>().position.x;
		float startY = (chartHeight / 2 * -1) + outline.GetComponent<RectTransform>().position.y;
		float min = Mathf.Infinity;
		float max = Mathf.NegativeInfinity;
		float[] nowData = new float[divisionsX];
		for (int i = 0; i < divisionsX; i++) {
			float p = currData.priceAt ((int)(i+time)); //get data of i + time
			if (p > max) max = p;
			if (p < min) min = p;
			nowData [i] = p;
		}

		float mid = (max - min) / 2.0f;

		for (int i = 1; i < divisionsX; i++) {
			float yPos = startY + (chartHeight/(max-min)) * (currData.priceAt((int)(i+time))-min);
			float xPos = startX + (chartWidth / divisionsX) * i;
			GameObject candlestick = GameObject.Instantiate (Resources.Load<GameObject>("candlestick"));
			candlestick.transform.SetParent(GameObject.Find("candlesticks").transform);
			candlestick.transform.position = new Vector3 (xPos, yPos);
		}


		for (int i = 1; i < divisionsY; i++) {
			float yPos = startY + i*(chartHeight/divisionsY);
			float xPos = startX + chartWidth/2.0f;
			GameObject ydiv = GameObject.Instantiate (Resources.Load<GameObject>("ydiv"));
			Vector3 scale = ydiv.transform.localScale;
			ydiv.transform.SetParent (GameObject.Find ("grid").transform);
			ydiv.transform.localScale = scale;
			ydiv.transform.position = new Vector3 (xPos, yPos);

		}


	}
		
	// Update is called once per frame
	void FixedUpdate () {
		currData = mc.BitcoinData;
		msSince = msSince + (Time.fixedDeltaTime*1000);
		if (msSince >= 1000 / updatesPerSecond) {
			time++;
			msSince = 0;
			updateChart ();
		}

	}
}
