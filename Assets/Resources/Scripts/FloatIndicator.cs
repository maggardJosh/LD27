using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FloatIndicator : FLabel
{
    float count = 0;
    float aliveTime = 1.0f;
    float upSpeed = -10.0f;

    private FLabel shadow;

    public FloatIndicator(string text, Vector2 startPos) : base("Small", text)
    {
        this.SetPosition(startPos);
        shadow = new FLabel("Small", text);
        shadow.color = new Color(0, 0, 0, .5f);
    }
    public override void HandleAddedToContainer(FContainer container)
    {
        container.AddChild(shadow);
        base.HandleAddedToContainer(container);

    }
    public override void HandleAddedToStage()
    {
        Futile.instance.SignalUpdate += Update;
        base.HandleAddedToStage();
    }

    public override void HandleRemovedFromStage()
    {
        shadow.RemoveFromContainer();
        Futile.instance.SignalUpdate -= Update;
        base.HandleRemovedFromStage();
    }

    public void Update()
    {
        this.alpha = 1 - (float)(count/aliveTime);
        this.y -= upSpeed * UnityEngine.Time.deltaTime;
        count += UnityEngine.Time.deltaTime;
        if (count >= aliveTime)
            this.RemoveFromContainer();
        shadow.SetPosition(GetPosition() + new Vector2(1.0f, -1.0f));
        shadow.alpha = this.alpha;
    }

    
}
