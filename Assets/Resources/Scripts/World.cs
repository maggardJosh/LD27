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

    LevelOverScreen endScreen = null;

    float startNumPlayers;

    public Minimap miniMap;

    private FTmxMap tmxMap = new FTmxMap();
    private FTilemap tilemap;
    private FContainer playerLayer = new FContainer();

    public FTilemap Tilemap { get { return tilemap; } }

    public FCamObject gui;

    private Clock clock;
    private EnemyClock enemyClock;

    float beginTime = 1.0f;             //Give the player 1 second for free at the beginning
    float beginCount = 0;

    private int[] enemiesOnLevel = new int[] { 30, 30, 40, 40, 40, 60, 40, 60, 60 };
    private string[] beginningMessages = new string[] {"Kill all enemies before time runs Out\n\nWASD - Move\n\nMouse - Shoot",
                                                      "Tip: Sometimes bigger enemies drop powerups... Shh...",
                                                      "You gain back time with every enemy you kill...",
                                                      "Remember you don't have to kill every enemy right away...",
                                                        "No more tips... Good luck",
                                                        "", 
                                                        "",
                                                        "One more level...",
                                                        "Last Level!..."};
    private FLabel beginningLabel;
    private FLabel beginningLabelShadow;

    public World(int level)
    {
        
        string beginningMessage = "";
        if (beginningMessages.Length > level)
            beginningMessage = beginningMessages[level];
        beginningLabel = new FLabel("Large", beginningMessage);
        beginningLabel.alpha = 1.0f;
        beginningLabel.y = -70;

        beginningLabelShadow = new FLabel("Large", beginningMessage);
        beginningLabelShadow.color = Color.black;
        beginningLabelShadow.SetPosition(beginningLabel.GetPosition());
        
        beginningLabelShadow.x += 1;
        beginningLabelShadow.y += -1;
        

        this.currentLevelNum = level;
        string levelName = "Maps/map" + level;
        this.startNumPlayers = enemiesOnLevel[level];

        clock = new Clock();
        clock.enableSound();
        enemyClock = new EnemyClock();

        gui = new FCamObject();
        gui.AddChild(clock);
        gui.AddChild(enemyClock);

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
        this.miniMap = new Minimap(this);
        playerLayer.AddChild(tmxMap);
        tilemap.clipNode = gui;

        Player player = new Player(true);
        gui.follow(player);
        addPlayer(player);
        player.setScale(2.0f, true);

        miniMap.setFollow(player);

        for (int ind = 0; ind < startNumPlayers; ind++)
        {
            Player p = new Player();
            addPlayer(p);
        }

        Futile.stage.AddChild(gui);

        gui.AddChild(new MuteMusicButton());
        gui.AddChild(miniMap);
        gui.AddChild(beginningLabelShadow);
        gui.AddChild(beginningLabel);
    }

    public bool isWalkable(int tileX, int tileY)
    {
        if (tileX < 0 || tileY < 0 || tileX >= tilemap.widthInTiles || tileY >= tilemap.heightInTiles)
            return false;        //For minimap drawing
        return tilemap.getFrameNum(tileX, tileY) != 1;
    }

    public void addPlayer(Player p)
    {
        p.addToMiniMap(miniMap);
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
        if (beginCount < beginTime)
        {
            beginCount += UnityEngine.Time.deltaTime;
            clock.percentage = 1.0f;
        }
        else if (beginningLabel.alpha > 0)
        {
            beginningLabel.alpha -= .3f * UnityEngine.Time.deltaTime;
            beginningLabelShadow.alpha = beginningLabel.alpha;
        }
        enemyClock.percentage = (playerList.Count - 1) / startNumPlayers;
        if (playerList.Count == 1)
        {
            clock.disableClock();
            if (endScreen == null)
            {
                FSoundManager.PlaySound("win");
                endScreen = new LevelOverScreen(true, currentLevelNum+1 >= enemiesOnLevel.Length);
                gui.AddChild(endScreen);

            }
            else
            {

                if (endScreen.readyToStart)
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
            }

        }
        else if (clock.percentage <= 0)
        {
            if (endScreen == null)
            {
                FSoundManager.PlaySound("lose");
                endScreen = new LevelOverScreen(false);
                gui.AddChild(endScreen);
            }
            else
            {
                endScreen.MoveToFront();
                if (endScreen.readyToStart)
                {
                    Futile.instance.SignalUpdate -= Update;
                    Futile.stage.RemoveAllChildren();

                    World newWorld = new World(this.currentLevelNum);
                    Futile.instance.SignalUpdate += newWorld.Update;
                }
            }
        }
        for (int ind = 0; ind < powerups.Count; ind++)
        {
            Powerup powerup = powerups[ind];
            foreach (Player p in playerList)
            {
                if (p.isControlled)
                    if (powerup.checkCollision(p))
                    {
                        FSoundManager.PlaySound("powerup");
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
                if (clock.percentage > 0 && b.checkCollision(p))
                {
                    p.setScale(p.scale - 1.0f, false);
                    if (p.scale <= 0)
                    {
                        FSoundManager.PlaySound("dead", .3f);
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
                            if (powerupChance < .4f)
                                powerup = new Powerup(Powerup.PowerupType.MACHINEGUN);
                            else if (powerupChance < .8f)
                                powerup = new Powerup(Powerup.PowerupType.SHOTGUN);
                            if (powerup != null)
                            {
                                powerups.Add(powerup);
                                powerup.SetPosition(p.GetPosition());
                                playerLayer.AddChild(powerup);
                            }
                        }
                    }
                    else
                    {
                        FSoundManager.PlaySound("hit", .3f);

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
