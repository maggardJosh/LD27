using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MuteMusicButton : FButton
{
    public MuteMusicButton(): base("music")
    {
        this.alpha = (FSoundManager.isMuted ? .3f : 1.0f);

        this.x = Futile.screen.halfWidth - this.sprite.width;
        this.y = -Futile.screen.halfHeight +this.sprite.height;
    }

    public override void HandleSingleTouchEnded(FTouch touch)
    {
        FSoundManager.isMuted = !FSoundManager.isMuted;
        this.alpha = (FSoundManager.isMuted ? .3f : 1.0f);
        base.HandleSingleTouchEnded(touch);
    }
}

