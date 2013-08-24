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

    private float lastShoot = 0;
    private float minShoot = .1f;

    private World world;

    public Player(bool isControllable = false)
        : base("player")
    {
        this.isControllable = isControllable;
        addAnimation(new FAnimation("idle", new int[] { 2 }, 300, true));
        addAnimation(new FAnimation("walk", new int[] { 1,2,3,2 }, 300, true));
        addAnimation(new FAnimation("shoot", new int[] { 0 }, 100, true));
        play("idle");
    }

    float xMove = 0;
    float yMove = 0;
    public override void Update()
    {
        

        float speed = 100;
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
            if (Input.GetMouseButton(0))
                state = State.SHOOTING;
            else
                state = State.IDLE;

            rotation = 0;
            rotation = this.GetLocalMousePosition().GetAngle() + 90;

        }
        else
        {
            if (RXRandom.Double() < .1)
            {
                xMove = (RXRandom.Float() * speed * 2 - speed) * UnityEngine.Time.deltaTime;
                yMove = (RXRandom.Float() * speed * 2 - speed) * UnityEngine.Time.deltaTime;

                rotation = new Vector2(xMove, yMove).GetAngle() + 90;
            }
            else if (RXRandom.Double() < .1)
            {
                xMove = 0;
                yMove = 0;
            }

            
        }

        x += xMove;
        y += yMove;



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

                if (lastShoot < minShoot)
                {
                  
                }                                           
                else                                        
                {                              
             
                    float bulletSpeed = 1000;
                    lastShoot = 0;
                    Bullet b = new Bullet(this.GetPosition(), new Vector2(Mathf.Cos((rotation-90) * C.PIOVER180) * bulletSpeed, -Mathf.Sin((rotation-90) * C.PIOVER180) * bulletSpeed));
                    b.rotation = this.rotation;
                    world.addBullet(b);

                }

                break;
        }

        base.Update();
    }


    internal void setWorld(World world)
    {
        this.world = world;
    }
}

