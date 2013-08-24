using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    World world;
    Clock clock;

    // Use this for initialization
    void Start()
    {

        FutileParams futileParams = new FutileParams(true, false, false, false);
        futileParams.AddResolutionLevel(480, 1.0f, 1.0f, "");
        futileParams.origin = new Vector2(0.5f, 0.5f);
        futileParams.backgroundColor = new Color(.2f, .2f, .2f);
        Futile.instance.Init(futileParams);
        Futile.atlasManager.LoadAtlas("Atlases/atlasOne");

        world = new World();

        clock = new Clock();
        Player player = new Player(true);
        world.addPlayer(player);


        FCamObject gui = new FCamObject();
        gui.AddChild(clock);
        gui.follow(player);

        Futile.stage.AddChild(gui);
        float randSize = 200;
        for (int ind = 0; ind < 20; ind++)
        {
            Player p = new Player();
            p.x = RXRandom.Float() * randSize * 2 - randSize;
            p.y = RXRandom.Float() * randSize * 2 - randSize;
            world.addPlayer(p);
            
        }
        Futile.instance.SignalUpdate += world.Update;

    }

    // Update is called once per frame
    void Update()
    {

       
    }
}
