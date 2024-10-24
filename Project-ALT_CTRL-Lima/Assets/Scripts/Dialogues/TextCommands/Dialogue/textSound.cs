using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class textSound : TextCommand
{
    public override bool isOneShot => true;
    private string _sound = "";
    private float _delay= 0.15f;
    public override void SetupData(string strCommandData)
    {
        string[] args = strCommandData.Split("|");
        if (args.Length >= 1)
        {
            _sound = args[0];
        }

        if (args.Length >= 2)
        {
            _delay = float.Parse(args[1], CultureInfo.InvariantCulture);
        }
    }

    public override void OnEnter()
    {
        DialogsController.instance.textSound = _sound;
        DialogsController.instance.characterSoundDelay = _delay;
    }

    public override void OnExit()
    {
    }
}