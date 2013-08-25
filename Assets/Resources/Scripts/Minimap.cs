using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Minimap : FContainer
{
    List<FSprite> playerBlips = new List<FSprite>();
    public const float BLIP_POS_MULT = .1f;
    private FNode followNode;

    private Vector2 displacement;
    private float screenMargin = 60;
    public Minimap(World world) 
    {
        setupWalls(world);
        displacement = new Vector2(-Futile.screen.halfWidth + screenMargin, -Futile.screen.halfHeight + screenMargin);
    }

    public void setFollow(FNode node)
    {
        this.followNode = node;
    }

    public void setupWalls(World world)
    {
        for (int xInd = 0; xInd < world.Tilemap.widthInTiles; xInd++)
            for (int yInd = 0; yInd < world.Tilemap.heightInTiles; yInd++)
            {
                if (world.isWalkable(xInd, yInd))
                {
                    FSprite newFloor = new FSprite(Futile.whiteElement);
                    newFloor.color = new Color(0, 0, 0, .8f);
                    newFloor.width = world.Tilemap._tileWidth * BLIP_POS_MULT;
                    newFloor.height = world.Tilemap._tileHeight * BLIP_POS_MULT;
                    newFloor.x = xInd * newFloor.width;
                    newFloor.y = -yInd * newFloor.height;
                    AddChild(newFloor);
                }
                else if (world.isWalkable(xInd - 1, yInd) ||
                    world.isWalkable(xInd +1, yInd) ||
                    world.isWalkable(xInd, yInd-1) ||
                    world.isWalkable(xInd, yInd+1))
                {
                    FSprite newWall = new FSprite(Futile.whiteElement);
                    newWall.color = new Color(.6f,.6f,.6f, .8f);
                    newWall.width = world.Tilemap._tileWidth * BLIP_POS_MULT;
                    newWall.height = world.Tilemap._tileHeight * BLIP_POS_MULT;
                    newWall.x = xInd * newWall.width;
                    newWall.y = -yInd * newWall.height;
                    AddChild(newWall);
                }
            }
    }
    public override void HandleAddedToStage()
    {
        Futile.instance.SignalUpdate += Update;
        base.HandleAddedToStage();
    }

    public override void HandleRemovedFromStage()
    {
        Futile.instance.SignalUpdate -= Update;
        base.HandleRemovedFromStage();
    }

    public void Update()
    {
        if(followNode!=null)
        {
            this.x = -followNode.x * BLIP_POS_MULT + displacement.x;
            this.y = -followNode.y * BLIP_POS_MULT + displacement.y ;

        }
    }

    public void addBlip(FSprite newBlip)
    {
        AddChild(newBlip);
    }
}

