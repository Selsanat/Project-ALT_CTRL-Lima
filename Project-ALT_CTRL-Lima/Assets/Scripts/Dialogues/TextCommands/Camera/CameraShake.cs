using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CameraShake : TextCommand
{
        public override bool isOneShot => true;
        private float _shakePower = 1f;
        private float _shakeDuration = 1f;
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
            CameraController.Instance.ShakeCamera(_shakePower, _shakeDuration);
        }

        public override void OnExit()
        {
        }
}
