using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

	public class TitleScreen : FContainer
	{
        private float onScreenCount = 0;
        private int startingLevel = 0;
        public TitleScreen(): base()
        {
            FSprite background = new FSprite("titleScreen");
            this.AddChild(background);
        }
        public override void HandleAddedToStage()
        {
            Futile.instance.SignalUpdate += Update;
            Futile.stage.x = 0;
            Futile.stage.y = 0;
            base.HandleAddedToStage();
        }

        public override void HandleRemovedFromStage()
        {
            Futile.instance.SignalUpdate -= Update;            
            base.HandleRemovedFromStage();
        }

        public void Update()
        {
            onScreenCount += UnityEngine.Time.deltaTime;
            if(onScreenCount > 1.0f && Input.GetMouseButtonUp(0))
            {
                this.RemoveFromContainer();
                World world = new World(startingLevel);
                Futile.instance.SignalUpdate += world.Update;
            }
        }
	}

