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
        //SendTextToTMP("<speed 20>你可以让文字<wave 5>波动<color=blue>起<pause 3.33>来<size=50%>或<speed -1>者<shake 3.3>进行<speed 2>抖动</wave>，如</color>上<speed 0>所</size>示。</shake>");
        SendTextToTMP(str);
    }
}
