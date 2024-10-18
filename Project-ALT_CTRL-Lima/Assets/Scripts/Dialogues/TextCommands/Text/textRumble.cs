using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class textRumble : TextCommand
{
    private int _loops = -1;
    private float _duration = 0.1f;
    private float _force = 15f;

    public override void SetupData(string strCommandData)
    {
        if (strCommandData == "") return;
        string[] args = strCommandData.Split("|");
        if (args.Length >= 1)
        {
            _duration = float.Parse(args[0], CultureInfo.InvariantCulture);
        }
        if (args.Length >= 2)
        {
            _loops = int.Parse(args[1], CultureInfo.InvariantCulture);
        }
        if (args.Length >= 3)
        {
            _force = float.Parse(args[1], CultureInfo.InvariantCulture);
        }
    }

    public override void OnEnter()
    {
        if (!char.IsWhiteSpace(_text.textInfo.characterInfo[_currentCharacter].character))
        {
            for (int j = 0; j < 4; ++j)
            {
                MakeLetterJump(_meshIndex, _vertexIndex + j);
            }
        }
    }

    private void MakeLetterJump(int meshIndex, int vertexIndex)
    {
        Vector3 vertex = _text.textInfo.meshInfo[meshIndex].vertices[vertexIndex];
        DOTween.To(() => vertex, x => vertex = x, vertex + new Vector3(_force, 0, 0), _duration).SetLoops(_loops, LoopType.Yoyo).OnUpdate(() =>
        {
            _text.textInfo.meshInfo[meshIndex].vertices[vertexIndex] = vertex;
            _text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        });
    }
    public override void OnExit()
    {
        //Exited Camera Shake
    }
}
