using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSender : MonoBehaviour
{
    public TMP_Text textMeshPro;
    string rawString;
    string preProcessString;
    TextAnim textAnim;
    Coroutine textAppearCoroutine;
    public TextProcess textProcess;
    public void SendTextToTMP(string _rawString)
    {
        if (textAppearCoroutine != null)
        {
            StopCoroutine(textAppearCoroutine);
            textAppearCoroutine = null;
        }
        rawString = _rawString;
        textMeshPro.text = rawString;
        textMeshPro.ForceMeshUpdate();
        preProcessString = textMeshPro.GetParsedText();
        textMeshPro.text = "";
        textMeshPro.ForceMeshUpdate();
        textProcess.ProcessCommand(rawString,preProcessString);
        textAppearCoroutine = StartCoroutine(textAnim.TextAppearCoroutine());
    }
    public string str;
    private void Start()
    {
        textProcess = new TextProcess();
        textAnim = new TextAnim(this);
        SendTextToTMP(str);
    }
}
