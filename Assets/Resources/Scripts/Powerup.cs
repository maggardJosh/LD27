using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Powerup : FAnimatedSprite
{
    public enum PowerupType
    {
        NONE,
        SHOTGUN
    }

    PowerupType type;

    public PowerupType PType { get { return type; } }
    public Powerup(PowerupType type) : base ("powerup")
    {
        this.type = type;
        addAnimation(new FAnimation("shotgun", new int[] { 0 }, 100, true));
        switch(type)
        {
            case PowerupType.SHOTGUN:
                play("shotgun");
                break;
            case PowerupType.NONE:
                //What are we doing here? THis should never happen
                break;
            default:
                throw new NotImplementedException("Powerup " + type + " not implemented");
        }
        

    }

    public bool checkCollision(Player p)
    {
        if ((p.GetPosition() - this.GetPosition()).sqrMagnitude < (p.width / 2 * p.width / 2))
        {
            return true;
        }
        return false;
    }
}
