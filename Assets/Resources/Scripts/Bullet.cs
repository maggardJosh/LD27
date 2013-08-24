using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bullet : FSprite
{
    Vector2 move;
    Player owner;

    FSprite shadow;

    public Bullet(Vector2 startPos, Vector2 direction) : base("bullet")
    {
        this.x = startPos.x;
        this.y = startPos.y;

        shadow = new FSprite("bullet");
        shadow.color = new Color(0, 0, 0, .8f);
        shadow.scale = 2;

        this.move = direction;
    }

    public override void HandleAddedToContainer(FContainer container)
    {
        container.AddChild(shadow);
        base.HandleAddedToContainer(container);
    }

    public override void HandleRemovedFromContainer()
    {
        shadow.RemoveFromContainer();
        base.HandleRemovedFromContainer();
    }

    public void setPlayer(Player p)
    {
        this.owner = p;
    }

    public void Update()
    {
        this.x += move.x * UnityEngine.Time.deltaTime;
        this.y += move.y * UnityEngine.Time.deltaTime;
        shadow.x = this.x;
        shadow.y = this.y - 3;
        shadow.rotation = this.rotation;
    }

    internal bool checkCollision(Player p)
    {
        if (this.owner == p)
            return false;
        if ((p.GetPosition() - this.GetPosition()).sqrMagnitude < (p.width / 2 * p.width / 2))
        {
            return true;
        }
        return false;
    }
}

