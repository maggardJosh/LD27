using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PowerupClock : Clock
{
    Powerup powerupSprite;

    public PowerupClock()
        : base()
    {
        this.x = -Futile.screen.halfWidth + this.clockMargin + (width * tickScale) / 2;

        timeLabel.SetPosition(this.GetPosition());
        clockBackground.SetPosition(GetPosition());
        timeLabel.isVisible = false;
    }

    public void setPowerUpType(Powerup.PowerupType powerupType)
    {
        if (powerupSprite != null)
            powerupSprite.RemoveFromContainer();
        powerupSprite = new Powerup(powerupType);
        powerupSprite.SetPosition(this.GetPosition());
        this.container.AddChild(powerupSprite);
        if (powerupType != Powerup.PowerupType.NONE)
            this.percentage = 1.0f;
    }

    public override void Update()
    {
        base.Update();
        if (powerupSprite != null)
        {
            powerupSprite.scale = this.scale;
            if (this.percentage <= 0)
                powerupSprite.RemoveFromContainer();
        }
    }
}
