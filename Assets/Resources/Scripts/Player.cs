using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Player : FAnimatedSprite
{
    public enum State
    {
        IDLE, SHOOTING
    }

    private State state = State.IDLE;
    private bool isControllable;
    public int secondValue = 1;

    private FSprite playerBlip;

    private Powerup.PowerupType powerUpType = Powerup.PowerupType.NONE;

    public bool isControlled { get { return isControllable; } }

    float bulletSpeed = 400;
    float speed = 100;
    float controlSpeed = 150;
    private FSprite shadow;
    private FSprite hair;

    private float lastShoot = 0;
    private float minShoot = .4f;

    private World world;

    private PowerupClock powerupClock;

    public Player(bool isControllable = false)
        : base("player")
    {
        playerBlip = new FSprite("playerBlip");
        hair = new FSprite("hair");
        this.isControllable = isControllable;
        addAnimation(new FAnimation("idle", new int[] { 0 }, 100, true));
        addAnimation(new FAnimation("pistol", new int[] { 1 }, 100, true));
        addAnimation(new FAnimation("shotgun", new int[] { 2 }, 100, true));
        addAnimation(new FAnimation("machinegun", new int[] { 3 }, 100, true));


        powerupClock = new PowerupClock();
        if (isControllable)
        {
            playerBlip.color = Color.green;
            play("pistol");
            shadow = new FSprite("player_1");
            hair.color = Color.black;

            powerupClock.percentage = 0;
        }
        else
        {
            playerBlip.color = Color.red;
            play("idle");
            hair.color = new Color(RXRandom.Float() * .5f, RXRandom.Float() * .5f, RXRandom.Float() * .5f);
            float randomColorMult = .5f + RXRandom.Float() * .5f;
            this.color = new Color(randomColorMult, randomColorMult, randomColorMult);
            shadow = new FSprite("player_0");
        }

        shadow.color = new Color(0, 0, 0, .5f);


    }

    public override void HandleAddedToContainer(FContainer container)
    {
        container.AddChild(shadow);
        base.HandleAddedToContainer(container);
        container.AddChild(hair);
    }

    public override void HandleRemovedFromContainer()
    {
        playerBlip.RemoveFromContainer();
        shadow.RemoveFromContainer();
        base.HandleRemovedFromContainer();
        hair.RemoveFromContainer();
    }

    float xMove = 0;
    float yMove = 0;
    public override void Update()
    {

        if (isControllable)
        {

            xMove = 0;
            yMove = 0;
            if (Input.GetKey(KeyCode.W))
                yMove = controlSpeed * UnityEngine.Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                yMove = -(controlSpeed * UnityEngine.Time.deltaTime);
            if (Input.GetKey(KeyCode.A))
                xMove = -(controlSpeed * UnityEngine.Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
                xMove = controlSpeed * UnityEngine.Time.deltaTime;
            if (Input.GetMouseButton(0))
                state = State.SHOOTING;
            else
                state = State.IDLE;

            rotation = 0;
            rotation = this.GetLocalMousePosition().GetAngle() + 90;

        }
        else
        {
            if (RXRandom.Double() < .03)
            {
                xMove = (RXRandom.Float() * speed * 2 - speed) * UnityEngine.Time.deltaTime;
                yMove = (RXRandom.Float() * speed * 2 - speed) * UnityEngine.Time.deltaTime;

                rotation = new Vector2(xMove, yMove).GetAngle() + 90;
            }
            else if (RXRandom.Double() < .01)
            {
                xMove = 0;
                yMove = 0;
            }


        }

        tryMove();

        if (this.powerupClock.percentage <= 0 && this.powerUpType != Powerup.PowerupType.NONE)
        {
            collectPowerUp(Powerup.PowerupType.NONE);
        }

        if (lastShoot < minShoot)
        {
            lastShoot += UnityEngine.Time.deltaTime;
        }
        switch (state)
        {
            case State.IDLE:
                break;
            case State.SHOOTING:

                if (lastShoot >= minShoot || (powerUpType == Powerup.PowerupType.MACHINEGUN && lastShoot >= minShoot * .05f))
                {
                    lastShoot = 0;

                    foreach (Bullet b in shootBullet())
                        world.addBullet(b);
                }

                break;
        }
        if (isControllable)
            switch (this.powerUpType)
            {
                case Powerup.PowerupType.NONE:
                    play("pistol");
                    break;
                case Powerup.PowerupType.SHOTGUN:
                    play("shotgun");
                    break;
                case Powerup.PowerupType.MACHINEGUN:
                    play("machinegun");
                    break;
            }

        shadow.rotation = this.rotation;
        shadow.SetPosition(this.GetPosition());
        shadow.y -= 3;
        hair.SetPosition(this.GetPosition());
        hair.rotation = this.rotation;

        playerBlip.SetPosition(GetPosition() * Minimap.BLIP_POS_MULT);
        playerBlip.rotation = this.rotation;
        playerBlip.scale = this.scale*.5f;

        base.Update();
    }

    private List<Bullet> shootBullet()
    {
        List<Bullet> result = new List<Bullet>();
        switch (powerUpType)
        {
            case Powerup.PowerupType.NONE:
                FSoundManager.PlaySound("shoot 1", .3f);
                float rotationRadians = -(rotation + 90) * C.PIOVER180;
                float xDisp = -10;
                float yDisp = -8;
                for (int x = 0; x < 1; x++)
                {
                    float directionRotation = (rotation - 90 + RXRandom.Float()) * C.PIOVER180;
                    Bullet b = new Bullet(this.GetPosition() + new Vector2(Mathf.Cos(rotationRadians) * xDisp + Mathf.Sin(rotationRadians) * yDisp, -Mathf.Cos(rotationRadians) * yDisp + Mathf.Sin(rotationRadians) * xDisp), new Vector2(Mathf.Cos(directionRotation) * bulletSpeed, -Mathf.Sin(directionRotation) * bulletSpeed));
                    b.rotation = directionRotation * C.PIOVER180_INV + 90;
                    b.setPlayer(this);
                    result.Add(b);
                }
                break;
            case Powerup.PowerupType.SHOTGUN:
                FSoundManager.PlaySound("shoot", .2f);
                rotationRadians = -(rotation + 90) * C.PIOVER180;
                xDisp = -20;
                yDisp = -5;
                float randomAngle = 20;
                for (int x = 0; x < 5; x++)
                {
                    float directionRotation = (rotation - 90 + RXRandom.Float() * randomAngle * 2 - randomAngle) * C.PIOVER180;
                    Bullet b = new Bullet(this.GetPosition() + new Vector2(Mathf.Cos(rotationRadians) * xDisp + Mathf.Sin(rotationRadians) * yDisp, -Mathf.Cos(rotationRadians) * yDisp + Mathf.Sin(rotationRadians) * xDisp), new Vector2(Mathf.Cos(directionRotation) * bulletSpeed, -Mathf.Sin(directionRotation) * bulletSpeed));
                    b.rotation = directionRotation * C.PIOVER180_INV + 90;
                    b.setPlayer(this);
                    result.Add(b);
                }
                break;
            case Powerup.PowerupType.MACHINEGUN:
                FSoundManager.PlaySound("shoot", .1f);
                rotationRadians = -(rotation + 90) * C.PIOVER180;
                xDisp = -12;
                yDisp = -12;
                randomAngle = 6;
                for (int x = 0; x < 1; x++)
                {
                    float directionRotation = (rotation - 90 + RXRandom.Float() * randomAngle * 2 - randomAngle) * C.PIOVER180;
                    Bullet b = new Bullet(this.GetPosition() + new Vector2(Mathf.Cos(rotationRadians) * xDisp + Mathf.Sin(rotationRadians) * yDisp, -Mathf.Cos(rotationRadians) * yDisp + Mathf.Sin(rotationRadians) * xDisp), new Vector2(Mathf.Cos(directionRotation) * bulletSpeed, -Mathf.Sin(directionRotation) * bulletSpeed));
                    b.rotation = directionRotation * C.PIOVER180_INV + 90;
                    b.setPlayer(this);
                    result.Add(b);
                }
                break;
        }
        return result;
    }

    #region moveCode
    private void tryMove()
    {
        if (yMove > 0)
            tryMoveUp();
        else if (yMove < 0)
            tryMoveDown();

        if (xMove > 0)
            tryMoveRight();
        else if (xMove < 0)
            tryMoveLeft();
    }

    private void tryMoveUp()
    {
        if (world.isWalkable((int)(x / world.Tilemap._tileWidth), (int)(-(y + yMove + height / 4) / world.Tilemap._tileHeight)))
        {
            y += yMove;
        }
    }

    private void tryMoveDown()
    {
        if (world.isWalkable((int)(x / world.Tilemap._tileWidth), (int)(-(y + yMove - height / 4) / world.Tilemap._tileHeight)))
        {
            y += yMove;
        }
    }

    private void tryMoveLeft()
    {
        if (world.isWalkable((int)((x + xMove - width / 4) / world.Tilemap._tileWidth), (int)(-y / world.Tilemap._tileHeight)))
        {
            x += xMove;
        }
    }

    private void tryMoveRight()
    {
        if (world.isWalkable((int)((x + xMove + width / 4) / world.Tilemap._tileWidth), (int)(-y / world.Tilemap._tileHeight)))
        {
            x += xMove;
        }
    }
    #endregion

    internal void setWorld(World world)
    {
        this.world = world;
        if (this.isControllable)
            world.gui.AddChild(powerupClock);

        hair.MoveToFront();
    }

    internal void setScale(float newScale, bool setValue = true)
    {
        if (setValue)
            this.secondValue = (int)newScale;
        this.scale = newScale;
        this.shadow.scale = newScale;
        this.hair.scale = newScale;

    }

    internal void collectPowerUp(Powerup.PowerupType powerupType)
    {
        this.powerUpType = powerupType;
        this.powerupClock.setPowerUpType(powerupType);
    }

    internal void addToMiniMap(Minimap miniMap)
    {
        miniMap.addBlip(playerBlip);
    }
}

