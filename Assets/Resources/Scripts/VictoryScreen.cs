using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LevelOverScreen : FContainer
{
    FSprite transparentBackground;
    FLabel levelCompleteLabel;


    public bool readyToStart = false;

    public LevelOverScreen(bool won, bool lastLevel = false)
        : base()
    {
        transparentBackground = new FSprite(Futile.whiteElement);
        transparentBackground.color = Color.black;
        transparentBackground.alpha = 0;
        transparentBackground.width = Futile.screen.width;
        transparentBackground.height = Futile.screen.height;
        if (won)
            levelCompleteLabel = new FLabel("Large", "Level Complete!\n\nClick to Continue");
        else
            levelCompleteLabel = new FLabel("Large", "You Ran Out Of Time!\n\nClick to Retry");

        if (lastLevel)
            levelCompleteLabel.text = "Level Complete!\n\nThat was the last level!!!\n\nThanks for Playing\n\n\n                             Jif";
        levelCompleteLabel.alpha = 0;

        AddChild(transparentBackground);
        AddChild(levelCompleteLabel);
    }

    public override void HandleAddedToStage()
    {
        Futile.instance.SignalUpdate += Update;
        base.HandleAddedToStage();
    }

    public override void HandleRemovedFromStage()
    {
        Futile.instance.SignalUpdate -= Update;
        base.HandleRemovedFromStage();
    }

    private void Update()
    {
        if (levelCompleteLabel.alpha < 1.0f)
            levelCompleteLabel.alpha += UnityEngine.Time.deltaTime * .3f;

        if (transparentBackground.alpha < .7f)
            transparentBackground.alpha += UnityEngine.Time.deltaTime * .3f;



        if (levelCompleteLabel.alpha >= .4f && Input.GetMouseButtonUp(0))
        {
            readyToStart = true;

        }
    }
}
