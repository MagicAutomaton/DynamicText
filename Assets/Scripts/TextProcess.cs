using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TextProcess
{
    #region REGEX
    public const string PAUSE_PATTERN = "<pause (?<LENGTH>\\d*(?:\\.\\d*)?)>";
    static readonly Regex PAUSE_REG = new Regex(PAUSE_PATTERN);
    public const string SPEED_PATTERN = "<speed (?<SPEED>\\d*(?:\\.\\d*)?)>";
    static readonly Regex SPEED_REG = new Regex(SPEED_PATTERN);
    public const string WAVE_PATTERN = "<wave (?<INTENSITY>\\d*(?:\\.\\d*)?)>";
    static readonly Regex WAVE_REG = new Regex(WAVE_PATTERN);
    public const string WAVE_END_PATTERN = "</wave>";
    static readonly Regex WAVE_END_REG = new Regex(WAVE_END_PATTERN);
    public const string SHAKE_PATTERN = "<shake (?<INTENSITY>\\d*(?:\\.\\d*)?)>";
    static readonly Regex SHAKE_REG = new Regex(SHAKE_PATTERN);
    public const string SHAKE_END_PATTERN = "</shake>";
    static readonly Regex SHAKE_END_REG = new Regex(SHAKE_END_PATTERN);
    public const string COLORFUL_PATTERN = "<colorful>";
    static readonly Regex COLORFUL_REG = new Regex(COLORFUL_PATTERN);
    public const string COLORFUL_END_PATTERN = "</colorful>";
    static readonly Regex COLORFUL_END_REG = new Regex(COLORFUL_END_PATTERN);
    public const string ALPHA_PATTERN = "<alpha (?<START>\\d*(?:\\.\\d*)?) (?<END>\\d*(?:\\.\\d*)?)>";
    static readonly Regex ALPHA_REG = new Regex(ALPHA_PATTERN);
    public const string ALPHA_END_PATTERN = "</alpha>";
    static readonly Regex ALPHA_END_REG = new Regex(ALPHA_END_PATTERN);
    #endregion
    #region commands
    public List<ProcessedCommand> commandList;
    Stack<ProcessedCommand> waveStack;
    Stack<ProcessedCommand> shakeStack;
    Stack<ProcessedCommand> colorfulStack;
    Stack<ProcessedCommand> alphaStack;
    public List<ProcessedCommand> pauseList;
    public List<ProcessedCommand> speedList;
    public List<ProcessedCommand> animList;
    public List<ProcessedCommand> colorList;
    #endregion
    public string rawString;//init text
    public string initString;//text without rich texts
    public string rawProcessedString;//text without custom tags
    public string processedString;//text without both rich texts and custom tags
    public void ProcessCommand(string rawText,string text)
    {
        #region regex
        MatchCollection pauseRes = PAUSE_REG.Matches(text);
        MatchCollection speedRes = SPEED_REG.Matches(text);
        MatchCollection waveRes = WAVE_REG.Matches(text);
        MatchCollection waveEndRes = WAVE_END_REG.Matches(text);
        MatchCollection shakeRes = SHAKE_REG.Matches(text);
        MatchCollection shakeEndRes = SHAKE_END_REG.Matches(text);
        MatchCollection colorfulRes = COLORFUL_REG.Matches(text);
        MatchCollection colorfulEndRes = COLORFUL_END_REG.Matches(text);
        MatchCollection alphaRes = ALPHA_REG.Matches(text);
        MatchCollection alphaEndRes = ALPHA_END_REG.Matches(text);
        #endregion
        #region init
        commandList = new List<ProcessedCommand>();
        waveStack = new Stack<ProcessedCommand>();
        shakeStack = new Stack<ProcessedCommand>();
        colorfulStack = new Stack<ProcessedCommand>();
        alphaStack = new Stack<ProcessedCommand>();
        pauseList = new List<ProcessedCommand>();
        speedList = new List<ProcessedCommand>();
        animList = new List<ProcessedCommand>();
        colorList = new List<ProcessedCommand>();
        #endregion
        #region text pre-process
        rawString = (string)rawText.Clone();
        initString = (string)text.Clone();
        processedString = (string)text.Clone();
        rawProcessedString= (string)rawText.Clone();
        processedString = Regex.Replace(processedString, PAUSE_PATTERN, "");
        processedString = Regex.Replace(processedString, SPEED_PATTERN, "");
        processedString = Regex.Replace(processedString, WAVE_PATTERN, "");
        processedString = Regex.Replace(processedString, WAVE_END_PATTERN, "");
        processedString = Regex.Replace(processedString, SHAKE_PATTERN, "");
        processedString = Regex.Replace(processedString, SHAKE_END_PATTERN, "");
        processedString = Regex.Replace(processedString, COLORFUL_PATTERN, "");
        processedString = Regex.Replace(processedString, COLORFUL_END_PATTERN, "");
        processedString = Regex.Replace(rawProcessedString, ALPHA_PATTERN, "");
        processedString = Regex.Replace(rawProcessedString, ALPHA_END_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, PAUSE_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, SPEED_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, WAVE_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, WAVE_END_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, SHAKE_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, SHAKE_END_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, COLORFUL_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, COLORFUL_END_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, ALPHA_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, ALPHA_END_PATTERN, "");
        #endregion
        #region process
        foreach (Match m in pauseRes)
            commandList.Add(new ProcessedCommand(m.Index, m.Length, TagType.pause, float.Parse(m.Groups["LENGTH"].Value)));
        foreach (Match m in speedRes)
            commandList.Add(new ProcessedCommand(m.Index, m.Length, TagType.speed, float.Parse(m.Groups["SPEED"].Value)));
        foreach (Match m in waveRes)
            commandList.Add(new ProcessedCommand(m.Index, m.Length, TagType.wave, float.Parse(m.Groups["INTENSITY"].Value)));
        foreach (Match m in shakeRes)
            commandList.Add(new ProcessedCommand(m.Index, m.Length, TagType.shake, float.Parse(m.Groups["INTENSITY"].Value)));
        foreach (Match m in waveEndRes)
            commandList.Add(new ProcessedCommand(m.Index, m.Length, TagType.waveEnd));
        foreach (Match m in shakeEndRes)
            commandList.Add(new ProcessedCommand(m.Index, m.Length, TagType.shakeEnd));
        foreach (Match m in colorfulRes)
            commandList.Add(new ProcessedCommand(m.Index, m.Length, TagType.colorful));
        foreach (Match m in colorfulEndRes)
            commandList.Add(new ProcessedCommand(m.Index, m.Length, TagType.colorfulEnd));
        foreach (Match m in alphaRes)
            commandList.Add(new ProcessedCommand(m.Index, m.Length, TagType.alpha, float.Parse(m.Groups["START"].Value), float.Parse(m.Groups["END"].Value)));
        foreach (Match m in alphaEndRes)
            commandList.Add(new ProcessedCommand(m.Index, m.Length, TagType.alphaEnd));
        #endregion
        #region farther process
        commandList.Sort(new ProcessedCommand.CommandComparer());
        int sumCount = 0;
        for (int i = 0; i < commandList.Count; ++i)
        {
            commandList[i].pos -= sumCount;
            sumCount += commandList[i].length;
            switch (commandList[i].command)
            {
                case TagType.pause:
                    AddPause(commandList[i]);
                    break;
                case TagType.speed:
                    AddSpeed(commandList[i]);
                    break;
                case TagType.wave:
                    PushWave(commandList[i]);
                    break;
                case TagType.waveEnd:
                    AddWave(commandList[i]);
                    break;
                case TagType.shake:
                    PushShake(commandList[i]);
                    break;
                case TagType.shakeEnd:
                    AddShake(commandList[i]);
                    break;
                case TagType.colorful:
                    PushColorful(commandList[i]);
                    break;
                case TagType.colorfulEnd:
                    AddColorful(commandList[i]);
                    break;
                case TagType.alpha:
                    PushAlpha(commandList[i]);
                    break;
                case TagType.alphaEnd:
                    AddAlpha(commandList[i]);
                    break;
            }
        }
        while (waveStack.Count > 0)
        {
            AddWave(new ProcessedCommand(processedString.Length, 0, TagType.waveEnd));
        }
        while (shakeStack.Count > 0)
        {
            AddShake(new ProcessedCommand(processedString.Length, 0, TagType.shakeEnd));
        }
        while(colorfulStack.Count>0)
        {
            AddColorful(new ProcessedCommand(processedString.Length, 0, TagType.colorfulEnd));
        }
        while(alphaStack.Count>0)
        {
            AddAlpha(new ProcessedCommand(processedString.Length, 0, TagType.alphaEnd));
        }
        Debug.Log("Finish Process");
        #endregion
    }
    void AddPause(ProcessedCommand command)
    {
        pauseList.Add(command);
    }
    void AddSpeed(ProcessedCommand command)
    {
        speedList.Add(command);
    }
    void PushWave(ProcessedCommand command)
    {
        waveStack.Push(command);
    }
    void PushShake(ProcessedCommand command)
    {
        shakeStack.Push(command);
    }
    void PushColorful(ProcessedCommand command)
    {
        colorfulStack.Push(command);
    }
    void PushAlpha(ProcessedCommand command)
    {
        alphaStack.Push(command);
    }
    void AddWave(ProcessedCommand command)
    {
        if (waveStack.Count <= 0)
            return;
        ProcessedCommand waveStart = waveStack.Pop();
        waveStart.length = command.pos - waveStart.pos;
        animList.Add(waveStart);
    }
    void AddShake(ProcessedCommand command)
    {
        if (shakeStack.Count <= 0)
            return;
        ProcessedCommand shakeStart = shakeStack.Pop();
        shakeStart.length = command.pos - shakeStart.pos;
        animList.Add(shakeStart);
    }
    void AddColorful(ProcessedCommand command)
    {
        if (colorfulStack.Count <= 0)
            return;
        ProcessedCommand colorfulStart = colorfulStack.Pop();
        colorfulStart.length = command.pos - colorfulStart.pos;
        colorList.Add(colorfulStart);
    }
    void AddAlpha(ProcessedCommand command)
    {
        if (alphaStack.Count <= 0)
            return;
        ProcessedCommand alphaStart = alphaStack.Pop();
        alphaStart.length = command.pos - alphaStart.pos;
        colorList.Add(alphaStart);
    }

}
public class ProcessedCommand
{
    public int pos;
    public int length;
    public TagType command;
    public float[] val;
    public ProcessedCommand(int _pos,int _length,TagType _command,params float[] _val)
    {
        pos = _pos;
        length = _length;
        command = _command;
        val = (float[])_val.Clone();
    }
    public class CommandComparer : IComparer<ProcessedCommand>
    {
        public int Compare(ProcessedCommand x, ProcessedCommand y)
        {
            return x.pos - y.pos;
        }
    }
}
public enum TagType
{
    pause,
    speed,
    wave,
    waveEnd,
    shake,
    shakeEnd,
    colorful,
    colorfulEnd,
    alpha,
    alphaEnd,
}