using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bullet : FSprite
{
    Vector2 move;
    public Bullet(Vector2 startPos, Vector2 direction) : base("bullet")
    {
        this.x = startPos.x;
        this.y = startPos.y;

        this.move = direction;
    }

    public void Update()
    {
        this.x += move.x * UnityEngine.Time.deltaTime;
        this.y += move.y * UnityEngine.Time.deltaTime;
    }

    internal bool checkCollision(Player p)
    {
        if ((p.GetPosition() - this.GetPosition()).sqrMagnitude < (width / 2 * width / 2))
        {
            this.RemoveFromContainer();
            return true;
        }
        return false;
    }
}

