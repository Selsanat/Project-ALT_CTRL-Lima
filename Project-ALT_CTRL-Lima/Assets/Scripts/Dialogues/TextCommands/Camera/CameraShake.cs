using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CameraShake : TextCommand
{
    public class TextCommandCameraShake : TextCommand
    {
        private float _shakePower = 0f;
        private float _shakeDuration = 0f;

        public override void SetupData(string strCommandData)
        {
            string[] args = strCommandData.Split("|");
            if (args.Length >= 1)
            {
                _shakePower = float.Parse(args[0], CultureInfo.InvariantCulture);
            }
            if (args.Length >= 2)
            {
                _shakeDuration = float.Parse(args[1], CultureInfo.InvariantCulture);
            }
        }

        public override void OnEnter()
        {
            //Shaking Camera
        }

        public override void OnExit()
        {
            //Exited Camera Shake
        }
    }
}
