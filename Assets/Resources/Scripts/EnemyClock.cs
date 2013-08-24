using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EnemyClock : Clock
{
    FSprite playerSprite;

    public EnemyClock()
        : base()
    {
        this.x = 0;
        
        timeLabel.SetPosition(this.GetPosition());
        clockBackground.SetPosition(GetPosition());
        timeLabel.isVisible = false;

        playerSprite = new FSprite("player_0");
        playerSprite.SetPosition(this.GetPosition());

        label.x = this.x;
        label.text = "Enemies Left";

    }

    public override void HandleAddedToContainer(FContainer container)
    {
        
        base.HandleAddedToContainer(container);
        container.AddChild(playerSprite);
    }

    public override void Update()
    {
        playerSprite.MoveToFront();
        base.Update();
    }
}
