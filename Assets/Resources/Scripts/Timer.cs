using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Timer : FLabel
{
    private float count = 0;
    public Timer() : base("Small", "00:00:00")
    {

        y = Futile.screen.halfHeight - 10;
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

    private void Update()
    {
        count += UnityEngine.Time.deltaTime;
        int hours = (int)(count / 60 )/60;
        string hourString = "";
        if(hours < 10)
            hourString += "0";
        hourString += hours;

        int minutes = (int)(count / 60 )%60;
        string minuteString = "";
        if(minutes < 10)
            minuteString += "0";
        minuteString += minutes;

         int seconds = (int)(count) % 60;
        string secondString = "";
        if(seconds < 10)
            secondString += "0";
        secondString += seconds;

        int milliseconds = (int)(count * 1000) % 1000;
        string millisecondString = "";
        millisecondString = String.Format("{0:0000}", milliseconds);


        this.text = hourString + ":" + minuteString + ":" + secondString + "." + millisecondString;
    }
}
