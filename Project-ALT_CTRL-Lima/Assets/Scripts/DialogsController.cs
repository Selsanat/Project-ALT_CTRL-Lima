using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DialogsController : MonoBehaviour
{
    public TMP_Text dialogText;
    private TMP_TextInfo _textInfo;

    public void showDialog()
    {
        StartCoroutine(ShowDialog());

    }
    private IEnumerator ShowDialog()
    {
        _textInfo = dialogText.textInfo;
        for (int i = 0; i < _textInfo.characterCount; i++)
        {
            int meshIndex = dialogText.textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = dialogText.textInfo.characterInfo[i].vertexIndex;

            if (!char.IsWhiteSpace(dialogText.textInfo.characterInfo[i].character)){
                for (int j = 0; j < 4; ++j)
                {
                    dialogText.textInfo.meshInfo[meshIndex].colors32[vertexIndex + j].a = 255;
                    //MakeLetterJump(meshIndex, vertexIndex + j);
                }

                dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    private void MakeLetterJump(int meshIndex,int vertexIndex)
    {
        Vector3 vertex = dialogText.textInfo.meshInfo[meshIndex].vertices[vertexIndex];
        DOTween.To(() => vertex, x => vertex = x, vertex + new Vector3(0, 5f, 0), 0.5f).OnUpdate(() =>
        {
            dialogText.textInfo.meshInfo[meshIndex].vertices[vertexIndex] = vertex;
            dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        });
    }

}
