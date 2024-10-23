using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class textSpeed : TextCommand
{
    public override bool isOneShot => true;
    private float _speed = 5f;
    public override void SetupData(string strCommandData)
    {
        string[] args = strCommandData.Split("|");
        if (args.Length >= 1)
        {
            _speed = float.Parse(args[0], CultureInfo.InvariantCulture);
        }
    }

    public override void OnEnter()
    {
        DialogsController.instance.SetCharacterPerSeconds((int)_speed);
        DialogsController1.instance.SetCharacterPerSeconds((int)_speed);
        DialogsController2.instance.SetCharacterPerSeconds((int)_speed);
    }

    public override void OnExit()
    {
    }
}
