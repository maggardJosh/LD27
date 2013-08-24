using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Clock : FRadialWipeSprite
{
    private float clockMargin = 5;
    private float normalScale = 2.0f;
    private float tickScale = 5.0f;
    public Clock()
        : base("clock", true, 0, 1.0f)
    {

        x = Futile.screen.halfWidth - width * tickScale / 2 - clockMargin;
        y = Futile.screen.halfHeight - height * tickScale / 2 - clockMargin;
        this.scale = normalScale;
        
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

    int lastSecond = 10;

    public void Update()
    {
        percentage -= UnityEngine.Time.deltaTime * .1f;
        if (Mathf.FloorToInt(percentage * 10) != lastSecond)
        {
            lastSecond = Mathf.FloorToInt(percentage * 10);
            this.scale = tickScale;
        }
        else
        {
            if (this.scale >= normalScale)
                this.scale -= UnityEngine.Time.deltaTime * (tickScale - normalScale);
        }
        if (percentage < .3f)
            this.color = Color.red;
        else
            this.color = Color.white;
        if (percentage <= 0)
            percentage = 1.0f;
    }
}
