using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
     FAnimatedSprite sprite;
     FRadialWipeSprite clock;
	// Use this for initialization
	void Start () {

        FutileParams futileParams = new FutileParams(true, false, false, false);

        futileParams.AddResolutionLevel(240, 1.0f, 1.0f, "");

        futileParams.origin = new Vector2(0.5f, 0.5f);

        futileParams.backgroundColor = new Color(.2f, .2f, .2f);

        Futile.instance.Init(futileParams);

        Futile.atlasManager.LoadAtlas("Atlases/atlasOne");

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        sprite = new FAnimatedSprite("player");
        sprite.addAnimation(new FAnimation("idle", new int[] { 0, 1, 2, 3, 2, 1 }, 100, true));
        Futile.stage.AddChild(sprite);

        clock = new FRadialWipeSprite("player_2", true, 0, .5f);

        Futile.stage.AddChild(clock);



	}
	
	// Update is called once per frame
	void Update () {

        float speed = 1000;
        if (Input.GetKey(KeyCode.W))
            sprite.y += speed * UnityEngine.Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            sprite.y -= speed * UnityEngine.Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            sprite.x -= speed * UnityEngine.Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            sprite.x += speed * UnityEngine.Time.deltaTime;

        clock.percentage -= UnityEngine.Time.deltaTime * .1f;
        if (clock.percentage <= 0)
            clock.percentage = 1.0f;

	}
}
