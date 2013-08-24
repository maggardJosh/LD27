using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class World
{
    List<Player> playerList = new List<Player>();
    List<Bullet> bulletList = new List<Bullet>();
    List<FNode> spawnPoints = new List<FNode>();

    FTmxMap tmxMap = new FTmxMap();
    FTilemap tilemap;

    FContainer playerLayer = new FContainer();

    public World()
    {

        Futile.stage.AddChild(playerLayer);

        tmxMap.LoadTMX("Maps/mapOne");
        tilemap = (FTilemap)(tmxMap.getLayerNamed("Tilemap"));
        FTilemap objectLayer = (FTilemap)(tmxMap.getLayerNamed("Objects"));

        for (int xInd = 0; xInd < objectLayer.widthInTiles; xInd++)
            for (int yInd = 0; yInd < objectLayer.heightInTiles; yInd++)
            {
                switch (objectLayer.getFrameNum(xInd, yInd))
                {
                    case 0:

                        break;
                    case 10:
                        FNode newSpawn = new FNode();
                        newSpawn.x = xInd * tilemap._tileWidth + tilemap._tileWidth / 2;
                        newSpawn.y = -yInd * tilemap._tileHeight + tilemap._tileHeight / 2;
                        spawnPoints.Add(newSpawn);
                        break;
                }
            }



        playerLayer.AddChild(tmxMap);
    }

    public void addPlayer(Player p)
    {
        p.SetPosition(spawnPoints[RXRandom.Int(spawnPoints.Count)].GetPosition());
        playerLayer.AddChild(p);
        playerList.Add(p);
        p.setWorld(this);
    }

    public void Update()
    {
        for (int ind = 0; ind < bulletList.Count; ind++)
        {
            Bullet b = bulletList[ind];
            b.Update();
            for (int playerInd = 0; playerInd < playerList.Count; playerInd++)
            {
                Player p = playerList[playerInd];
                if (b.checkCollision(p))
                {

                    b.RemoveFromContainer();
                    p.RemoveFromContainer();
                    bulletList.Remove(b);
                    playerList.Remove(p);
                    ind--;
                    playerInd--;
                }
                else
                    if (tilemap.getFrameNum((int)(b.x / tilemap._tileWidth), (int)(-b.y / tilemap._tileHeight)) == 1)
                    {
                        b.RemoveFromContainer();
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

    internal void setGUI(FCamObject gui)
    {
        tilemap.clipNode = gui;
    }
}
