using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class textJump : TextCommand
{
    private float _loops = 0f;
    private float _duration = 0f;

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
        Vector3 vertex = GetMeshInfo().vertices[_currentCharacter];
        for (int j = 0; j < 4; ++j)
        {
            DOTween.To(() => vertex, x => vertex = x, vertex + new Vector3(0, 5f, 0), 0.5f).OnUpdate(() =>
            {
                _text.textInfo.meshInfo[_meshIndex].vertices[_vertexIndex] = vertex;
                _text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            });
        }
    }

    public override void OnExit()
    {
        //Exited Camera Shake
    }
}
