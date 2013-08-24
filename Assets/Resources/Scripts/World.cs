using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class World
{
    private List<Player> playerList = new List<Player>();
    private List<Bullet> bulletList = new List<Bullet>();
    private List<FNode> spawnPoints = new List<FNode>();
    private List<Powerup> powerups = new List<Powerup>();

    FNode playerSpawn;
    int currentLevelNum = 0;

    float startNumPlayers;

    private FTmxMap tmxMap = new FTmxMap();
    private FTilemap tilemap;
    private FContainer playerLayer = new FContainer();

    public FTilemap Tilemap { get { return tilemap; } }

    public FCamObject gui;

    private Clock clock;
    private EnemyClock enemyClock;

    private int[] enemiesOnLevel = new int[] { 30, 30, 40, 40, 1 };

    public World(int level)
    {
        
        this.currentLevelNum = level;
        string levelName = "Maps/map" + level ;
        this.startNumPlayers = enemiesOnLevel[level];

        clock = new Clock();
        enemyClock = new EnemyClock();

        FCamObject gui = new FCamObject();
        gui.AddChild(clock);
        gui.AddChild(enemyClock);

        this.gui = gui;

        setClock(clock);

        Futile.stage.AddChild(playerLayer);

        tmxMap.LoadTMX(levelName);
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
                        newSpawn.y = -yInd * tilemap._tileHeight - tilemap._tileHeight / 2;
                        spawnPoints.Add(newSpawn);
                        break;
                    case 11:
                        playerSpawn = new FNode();
                        playerSpawn.x = xInd * tilemap._tileWidth + tilemap._tileWidth / 2;
                        playerSpawn.y = -yInd * tilemap._tileHeight - tilemap._tileHeight / 2;
                        break;
                }
            }

        playerLayer.AddChild(tmxMap);
        tilemap.clipNode = gui;
        Player player = new Player(true);
        gui.follow(player);
        addPlayer(player);
        player.setScale(2.0f, true);

        for (int ind = 0; ind < startNumPlayers; ind++)
        {
            Player p = new Player();
            addPlayer(p);
        }

        Futile.stage.AddChild(gui);
    }

    public bool isWalkable(int tileX, int tileY)
    {
        return tilemap.getFrameNum(tileX, tileY) != 1;
    }

    public void addPlayer(Player p)
    {
        if (p.isControlled)
            p.SetPosition(playerSpawn.GetPosition());
        else
            p.SetPosition(spawnPoints[RXRandom.Int(spawnPoints.Count)].GetPosition());
        float scaleChance = RXRandom.Float();
        if (scaleChance < .15f)
            p.setScale(3.0f);
        else if (scaleChance < .4f)
            p.setScale(2.0f);
        playerLayer.AddChild(p);
        playerList.Add(p);
        p.setWorld(this);
    }

    public void Update()
    {
        
        enemyClock.percentage = (playerList.Count - 1) / startNumPlayers;
        if (playerList.Count == 1)
        {
            Futile.instance.SignalUpdate -= Update;
            Futile.stage.RemoveAllChildren();
            if (this.currentLevelNum + 1 < enemiesOnLevel.Length)
            {

                World newWorld = new World(++this.currentLevelNum);
                Futile.instance.SignalUpdate += newWorld.Update;
            }
            else
            {
                TitleScreen titleScreen = new TitleScreen();
                Futile.stage.AddChild(titleScreen);
            }
        }
        for (int ind = 0; ind < powerups.Count; ind++)
        {
            Powerup powerup = powerups[ind];
            foreach (Player p in playerList)
            {
                if(p.isControlled)
                if (powerup.checkCollision(p))
                {
                    p.collectPowerUp(powerup.PType);
                    powerup.RemoveFromContainer();
                    powerups.Remove(powerup);
                    ind--;
                }
            }
        }
        for (int ind = 0; ind < bulletList.Count; ind++)
        {
            Bullet b = bulletList[ind];
            b.Update();
            for (int playerInd = 0; playerInd < playerList.Count; playerInd++)
            {
                Player p = playerList[playerInd];
                if (b.checkCollision(p))
                {
                    p.setScale(p.scale - 1.0f, false);
                    if (p.scale <= 0)
                    {
                        p.RemoveFromContainer();
                        playerList.Remove(p);
                        playerInd--;
                        FloatIndicator floatInd = new FloatIndicator("+00:00:0" + p.secondValue, p.GetPosition());
                        playerLayer.AddChild(floatInd);
                        clock.percentage += p.secondValue / 10.0f;      //Add the seconds to the clock
                        if (p.secondValue == 3)
                        {
                            float powerupChance = RXRandom.Float();
                            Powerup powerup = null;
                            if(powerupChance < .4f)
                                powerup = new Powerup(Powerup.PowerupType.MACHINEGUN);
                            else if(powerupChance < .8f)
                                powerup = new Powerup(Powerup.PowerupType.SHOTGUN);
                            if (powerup != null)
                            {
                                powerups.Add(powerup);
                                powerup.SetPosition(p.GetPosition());
                                playerLayer.AddChild(powerup);
                            }
                        }
                    }
                    b.RemoveFromContainer();
                    bulletList.Remove(b);
                    ind--;
                    break;
                }
                else
                    if (tilemap.getFrameNum((int)(b.x / tilemap._tileWidth), (int)(-b.y / tilemap._tileHeight)) == 1)
                    {
                        b.RemoveFromContainer();
                        bulletList.Remove(b);
                        ind--;
                        break;
                    }
            }
        }
    }

    internal void addBullet(Bullet b)
    {
        bulletList.Add(b);
        playerLayer.AddChild(b);
    }

    

    

    internal void setClock(Clock clock)
    {
        this.clock = clock;
    }
}
