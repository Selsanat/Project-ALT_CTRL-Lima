using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class playSound : TextCommand
{
    public override bool isOneShot => true;
    private string _sound = "";
    public override void SetupData(string strCommandData)
    {
        string[] args = strCommandData.Split("|");
        if (args.Length >= 1)
        {
            _sound = args[0];
        }
    }

    public override void OnEnter()
    {
        SoundManager.instance.PlayClip(_sound);
    }

    public override void OnExit()
    {
    }
}