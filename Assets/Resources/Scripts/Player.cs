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

    float bulletSpeed = 400;
    float speed = 100;
    private FSprite shadow;

    private float lastShoot = 0;
    private float minShoot = .1f;

    private World world;

    public Player(bool isControllable = false)
        : base("player")
    {
        this.isControllable = isControllable;
        addAnimation(new FAnimation("idle", new int[] { 2 }, 300, true));
        addAnimation(new FAnimation("walk", new int[] { 1, 2, 3, 2 }, 300, true));
        addAnimation(new FAnimation("shoot", new int[] { 0 }, 100, true));
        play("idle");

        shadow = new FSprite("player_0");
        shadow.color = new Color(0, 0, 0, .5f);
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

    float xMove = 0;
    float yMove = 0;
    public override void Update()
    {

        if (isControllable)
        {
            xMove = 0;
            yMove = 0;
            if (Input.GetKey(KeyCode.W))
                yMove = speed * UnityEngine.Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                yMove = -(speed * UnityEngine.Time.deltaTime);
            if (Input.GetKey(KeyCode.A))
                xMove = -(speed * UnityEngine.Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
                xMove = speed * UnityEngine.Time.deltaTime;
            if (Input.GetMouseButtonDown(0))
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



        if (lastShoot < minShoot)
        {
            lastShoot += UnityEngine.Time.deltaTime;
        }
        switch (state)
        {
            case State.IDLE:
                play("shoot");
                break;
            case State.SHOOTING:

                if (lastShoot >= minShoot)
                {
                    lastShoot = 0;
                    float rotationRadians = -(rotation + 90) * C.PIOVER180;
                    float xDisp = -5;
                    float yDisp = -5;
                    Bullet b = new Bullet(this.GetPosition() + new Vector2(Mathf.Cos(rotationRadians)*xDisp + Mathf.Sin(rotationRadians)*yDisp, -Mathf.Cos(rotationRadians)*yDisp + Mathf.Sin(rotationRadians)*xDisp), new Vector2(Mathf.Cos((rotation - 90) * C.PIOVER180) * bulletSpeed, -Mathf.Sin((rotation - 90) * C.PIOVER180) * bulletSpeed));
                    b.rotation = this.rotation;
                    b.setPlayer(this);
                    world.addBullet(b);
                }

                break;
        }

        shadow.rotation = this.rotation;
        shadow.SetPosition(this.GetPosition());
        shadow.y -= 3;

        base.Update();
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
        if (world.isWalkable((int)(x / world.Tilemap._tileWidth), (int)(-(y+yMove + height/4) / world.Tilemap._tileHeight)))
        {
            y += yMove;
        }
    }

    private void tryMoveDown()
    {
        if (world.isWalkable((int)(x / world.Tilemap._tileWidth), (int)(-(y + yMove -height/4) / world.Tilemap._tileHeight)))
        {
            y += yMove;
        }
    }

    private void tryMoveLeft()
    {
        if (world.isWalkable((int)((x+xMove - width/4) / world.Tilemap._tileWidth), (int)(-y / world.Tilemap._tileHeight)))
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
    }
}

