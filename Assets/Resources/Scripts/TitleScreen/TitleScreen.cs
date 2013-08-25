using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TitleScreen : FContainer
{
    private float onScreenCount = 0;
    private int startingLevel = 0;
    private FLabel clickToStart;
    private FLabel disclaimer;
    private MuteMusicButton mute;

    public TitleScreen()
        : base()
    {
        clickToStart = new FLabel("Large", "Click To Start");
        FSprite background = new FSprite("titleScreen");
        this.AddChild(background);
        this.AddChild(clickToStart);

        disclaimer = new FLabel("Large", "Made in 48 hours for the Ludum Dare Competition");
        disclaimer.y = -140;
        AddChild(disclaimer);

        clickToStart.alpha = 0;
        clickToStart.y = -Futile.screen.halfHeight / 2 ;

        mute = new MuteMusicButton();
        
        AddChild(mute);
    }

    public override void HandleAddedToStage()
    {
        Futile.instance.SignalUpdate += Update;
        Futile.stage.x = 0;
        Futile.stage.y = 0;
        
        base.HandleAddedToStage();
    }

    public override void HandleRemovedFromStage()
    {
        Futile.instance.SignalUpdate -= Update;
        base.HandleRemovedFromStage();
    }

    public void Update()
    {
        onScreenCount += UnityEngine.Time.deltaTime;
        if (onScreenCount > 1.0f)
            clickToStart.alpha += UnityEngine.Time.deltaTime * 1.0f;
        float xClickMargin = 100;
        float yClickMargin = 80;
        Vector2 stageMousePos = Futile.stage.GetLocalMousePosition();
        if (onScreenCount > 1.0f && Input.GetMouseButtonUp(0) && stageMousePos.x < xClickMargin && stageMousePos.x > -xClickMargin && stageMousePos.y < yClickMargin && stageMousePos.y > -yClickMargin)
        {
            this.RemoveFromContainer();
            World world = new World(startingLevel);
            Futile.instance.SignalUpdate += world.Update;
        }
    }
}

