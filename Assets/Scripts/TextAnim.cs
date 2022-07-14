using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnim
{
    TextSender sender;
    int visibleCharCount;
    int maxCharCount;
    float textSpeed = 8;
    const float textTime = 0.125f;
    TMP_Text textMeshPro
    {
        get { return sender.textMeshPro; }
    }
    TextProcess textProcess
    {
        get { return sender.textProcess; }
    }
    public TextAnim(TextSender _sender)
    {
        sender = _sender;
    }
    public IEnumerator TextAppearCoroutine()
    {
        visibleCharCount = -1;
        maxCharCount = textProcess.processedString.Length;
        textMeshPro.text = textProcess.rawProcessedString;
        textMeshPro.ForceMeshUpdate();
        TMP_MeshInfo[] initMeshInfo = textMeshPro.textInfo.CopyMeshInfoVertexData();
        float[] sumTime = new float[maxCharCount];
        float prevStartTime = float.MinValue;
        while (true)
        {
            if (visibleCharCount < maxCharCount && Time.time - prevStartTime >= 1f / textSpeed)
            {
                ++visibleCharCount;
                prevStartTime = Time.time;
                foreach (ProcessedCommand c in textProcess.pauseList)
                    if (c.pos == visibleCharCount)
                        prevStartTime += c.val[0];
                foreach (ProcessedCommand c in textProcess.speedList)
                    if (c.pos == visibleCharCount)
                        textSpeed = c.val[0] == 0 ? 8 : c.val[0];
            }

            for(int i=0;i<maxCharCount;++i)
            {
                TMP_CharacterInfo charInfo = textMeshPro.textInfo.characterInfo[i];
                int matInd = charInfo.materialReferenceIndex;
                int vertInd = charInfo.vertexIndex;

                if (!charInfo.isVisible)
                    continue;

                if (i < visibleCharCount)
                {
                    sumTime[i] += Time.deltaTime;
                }

                float sizeScale = Mathf.Min(sumTime[i] / textTime, 1);

                Vector3[] initVertices = initMeshInfo[matInd].vertices;
                Vector3 centerPos = (initVertices[vertInd+0] + initVertices[vertInd+2]) / 2;
                Vector3[] newVertices = textMeshPro.textInfo.meshInfo[matInd].vertices;

                Vector3 animOffset = CalcAnimOffset(i);

                for (int j = 0; j < 4; ++j)
                    newVertices[vertInd + j] = (animOffset + initVertices[vertInd + j] - centerPos) * sizeScale + centerPos;

                CalcColor(i, vertInd, ref textMeshPro.textInfo.meshInfo[matInd]);

                textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            }
            yield return null;
        }
    }
    Vector3 CalcAnimOffset(int charInd)
    {
        Vector3 offset = Vector3.zero;
        foreach (ProcessedCommand anim in sender.textProcess.animList)
            if (anim.pos <= charInd && anim.pos + anim.length > charInd)
            {
                switch (anim.command)
                {
                    case TagType.wave:
                        offset += Vector3.up * Mathf.Sin(charInd + Time.time * 8) * anim.val[0];
                        break;
                    case TagType.shake:
                        offset += new Vector3(Mathf.PerlinNoise(charInd + Time.time * 10, 0), Mathf.PerlinNoise(0, charInd + Time.time * 10), 0) * anim.val[0];
                        break;
                }
            }
        return offset;
    }
    void CalcColor(int charInd,int vertInd, ref TMP_MeshInfo meshes)
    {
        Color32[] newColor = new Color32[4];
        for (int i = 0; i < 4; ++i)
            newColor[i] = meshes.colors32[vertInd + i];
        foreach(ProcessedCommand command in sender.textProcess.colorList)
            if(command.pos<=charInd&&command.pos+command.length>charInd)
            {
                switch(command.command)
                {
                    case TagType.colorful:
                        for (int i = 0; i < 4; ++i)
                        {
                            Vector3 vec = meshes.vertices[vertInd + i];
                            newColor[i].r = (byte)(256 * Mathf.PerlinNoise(vec.x + Time.time + 1003, vec.y + Time.time + 1009));
                            newColor[i].g = (byte)(256 * Mathf.PerlinNoise(vec.x + Time.time + 1007, vec.y + Time.time + 1007));
                            newColor[i].b = (byte)(256 * Mathf.PerlinNoise(vec.x + Time.time + 1009, vec.y + Time.time + 1003));
                        }
                        break;
                }
            }
        newColor.CopyTo(meshes.colors32, vertInd);
    }
}