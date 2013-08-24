using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class World
{
    List<Player> playerList = new List<Player>();
    List<Bullet> bulletList = new List<Bullet>();

    FContainer playerLayer = new FContainer();

    public World()
    {
        
        Futile.stage.AddChild(playerLayer);
    }

    public void addPlayer(Player p)
    {
        playerLayer.AddChild(p);
        playerList.Add(p);
        p.setWorld(this);
    }

    public void Update()
    {
        for (int ind = 0; ind < bulletList.Count; ind++ )
        {
            Bullet b = bulletList[ind];
            b.Update();
            foreach (Player p in playerList)
            {
                if (b.checkCollision(p))
                {
                    bulletList.Remove(b);
                    ind--;
                }
            }
        }
    }

    internal void addBullet(Bullet b)
    {
        bulletList.Add(b);
        playerLayer.AddChild(b);
    }
}
