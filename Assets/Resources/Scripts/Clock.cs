using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Clock : FRadialWipeSprite
{
    private float clockMargin = 5;
    public Clock()
        : base("clock", true, 0, 1.0f)
    {

        x = Futile.screen.halfWidth - width / 2 - clockMargin;
        y = Futile.screen.halfHeight - height / 2 - clockMargin;
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


    public void Update()
    {
        percentage -= UnityEngine.Time.deltaTime * .1f;
        if (percentage <= 0)
            percentage = 1.0f;
    }
}
