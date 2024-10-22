using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class textPause : TextCommand
{
    public override bool isOneShot => true;
    private float _wait = 2f;
    public override void SetupData(string strCommandData)
    {
        string[] args = strCommandData.Split("|");
        if (args.Length >= 1)
        {
            _wait = float.Parse(args[0], CultureInfo.InvariantCulture);
        }
    }

    public override void OnEnter()
    {
        DialogsController.instance.SetPauseTime(_wait);
        //DialogsController1.instance.SetPauseTime((int)_wait);
        //DialogsController2.instance.SetPauseTime((int)_wait);
    }

    public override void OnExit()
    {
    }
}

