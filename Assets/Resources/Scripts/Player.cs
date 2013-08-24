using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Player : FAnimatedSprite
{
    public Player() : base("player")
    {
        addAnimation(new FAnimation("idle", new int[] { 0, 1, 2, 3, 2, 1 }, 100, true));
        play("idle");
    }

    public override void Update()
    {
        float speed = 100;
        if (Input.GetKey(KeyCode.W))
            y += speed * UnityEngine.Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            y -= speed * UnityEngine.Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            x -= speed * UnityEngine.Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            x += speed * UnityEngine.Time.deltaTime;
        base.Update();
    }
}

