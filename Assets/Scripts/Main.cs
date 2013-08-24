using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {

        FutileParams futileParams = new FutileParams(true, false, false, false);

        futileParams.AddResolutionLevel(960, 1.0f, 1.0f, "");

        futileParams.origin = new Vector2(0.5f, 0.5f);

        futileParams.backgroundColor = new Color(.2f, .2f, .2f);

        Futile.instance.Init(futileParams);

        Futile.atlasManager.LoadAtlas("Atlases/atlasOne");

        Screen.sleepTimeout = SleepTimeout.NeverSleep;


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
