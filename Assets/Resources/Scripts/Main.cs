using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    Player player;
    
     FRadialWipeSprite clock;
     float clockMargin = 5;
     FContainer playerLayer;
	// Use this for initialization
	void Start () {

        FutileParams futileParams = new FutileParams(true, false, false, false);

        futileParams.AddResolutionLevel(240, 1.0f, 1.0f, "");

        futileParams.origin = new Vector2(0.5f, 0.5f);

        futileParams.backgroundColor = new Color(.2f, .2f, .2f);

        Futile.instance.Init(futileParams);

        Futile.atlasManager.LoadAtlas("Atlases/atlasOne");

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        playerLayer = new FContainer();
        player = new Player();
        Futile.instance.SignalUpdate += player.Update;

        playerLayer.AddChild(player);

        Futile.stage.AddChild(playerLayer);

        clock = new FRadialWipeSprite("clock", true, 0, 1.0f);
        clock.x = Futile.screen.halfWidth - clock.width/2 - clockMargin;
        clock.y = Futile.screen.halfHeight - clock.height/2 - clockMargin;


        FCamObject gui = new FCamObject();
        gui.AddChild(clock);
        gui.follow(player);

        Futile.stage.AddChild(gui);

	}
	
	// Update is called once per frame
	void Update () {



        clock.percentage -= UnityEngine.Time.deltaTime * .1f;
        if (clock.percentage <= 0)
            clock.percentage = 1.0f;

	}
}
