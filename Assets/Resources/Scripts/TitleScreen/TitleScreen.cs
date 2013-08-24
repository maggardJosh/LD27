using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

	public class TitleScreen : FContainer
	{
        public TitleScreen(): base()
        {
            FSprite background = new FSprite("titleScreen");
            this.AddChild(background);
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
            if (Input.GetMouseButtonUp(0))
            {
                this.RemoveFromContainer();
                World world = new World(0);
                Futile.instance.SignalUpdate += world.Update;
            }
        }
	}

