using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    World world;

    // Use this for initialization
    void Start()
    {

        FutileParams futileParams = new FutileParams(true, false, false, false);
        futileParams.AddResolutionLevel(480, 1.0f, 1.0f, "");
        futileParams.origin = new Vector2(0.5f, 0.5f);
        futileParams.backgroundColor = new Color(.2f, .2f, .2f);
        Futile.instance.Init(futileParams);
        Futile.atlasManager.LoadAtlas("Atlases/atlasOne");
        Futile.atlasManager.LoadAtlas("Atlases/Fonts");
        Futile.atlasManager.LoadFont("Small", "Small Font", "Atlases/Small Font", 0, 0);

        TitleScreen titleScreen = new TitleScreen();
        Futile.stage.AddChild(titleScreen);
        
    }

    // Update is called once per frame
    void Update()
    {

       
    }
}
