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
    #endregion
    #region 指令列表
    public List<ProcessedCommand> commandList;
    Stack<ProcessedCommand> waveStack;
    Stack<ProcessedCommand> shakeStack;
    public List<ProcessedCommand> pauseList;
    public List<ProcessedCommand> speedList;
    public List<ProcessedCommand> animList;
    #endregion
    public string rawString;//原始文本
    public string initString;//去除rich text后的文本
    public string rawProcessedString;//rawString去除自定义标签后文本
    public string processedString;//initString去除自定义标签后文本
    public void ProcessCommand(string rawText,string text)
    {
        #region 获取全部匹配
        MatchCollection pauseRes = PAUSE_REG.Matches(text);
        MatchCollection speedRes = SPEED_REG.Matches(text);
        MatchCollection waveRes = WAVE_REG.Matches(text);
        MatchCollection waveEndRes = WAVE_END_REG.Matches(text);
        MatchCollection shakeRes = SHAKE_REG.Matches(text);
        MatchCollection shakeEndRes = SHAKE_END_REG.Matches(text);
        #endregion
        #region 初始化
        commandList = new List<ProcessedCommand>();
        waveStack = new Stack<ProcessedCommand>();
        shakeStack = new Stack<ProcessedCommand>();
        pauseList = new List<ProcessedCommand>();
        speedList = new List<ProcessedCommand>();
        animList = new List<ProcessedCommand>();
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
        rawProcessedString = Regex.Replace(rawProcessedString, PAUSE_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, SPEED_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, WAVE_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, WAVE_END_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, SHAKE_PATTERN, "");
        rawProcessedString = Regex.Replace(rawProcessedString, SHAKE_END_PATTERN, "");
        #endregion
        #region 初步分析指令
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
        #endregion
        #region 进一步分析指令
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
}