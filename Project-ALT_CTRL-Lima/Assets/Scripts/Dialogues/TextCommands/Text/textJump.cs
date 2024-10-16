using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class textJump : TextCommand
{
    private float _loops = 1f;
    private float _duration = 1f;
    public override void SetupData(string strCommandData)
    {
        string[] args = strCommandData.Split("|");
        if (args.Length >= 1)
        {
            _duration = float.Parse(args[0], CultureInfo.InvariantCulture);
        }
        if (args.Length >= 2)
        {
            _loops = float.Parse(args[1], CultureInfo.InvariantCulture);
        }
    }

    public override void OnEnter()
    {
        if (!char.IsWhiteSpace(_text.textInfo.characterInfo[_currentCharacter].character))
        {
            
            for (int j = 0; j < 4; ++j)
            {
                Vector3 vertex = _text.textInfo.meshInfo[_meshIndex].vertices[_vertexIndex + j];
                DOTween.To(() => vertex, x => vertex = x, vertex + new Vector3(0, 5f, 0), 1).SetLoops(-1, LoopType.Yoyo).OnUpdate(() =>
                {
                    _text.textInfo.meshInfo[_meshIndex].vertices[_vertexIndex + j] = vertex;
                    _text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                });
            }
        }
    }

    public override void OnExit()
    {
        //Exited Camera Shake
    }
}
