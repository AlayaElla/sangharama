﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ChatLoader{

    ArrayList chatconifg = new ArrayList();

    PlayerInfo.Info info;

    public ChatLoader()
    {
        info = PlayerInfo.GetPlayerInfo();
        //获取配置表
        XmlTool xt = new XmlTool();
        chatconifg = xt.loadChatConfigXmlToArray();
    }

    public void Clear()
    {
        chatconifg = null;
        info = new PlayerInfo.Info();
    }


    //读取通用配置
    public ChatManager.ChatConfig LoadNowConfig()
    {
        ChatManager.ChatConfig config = (ChatManager.ChatConfig)chatconifg[0];
        foreach (ChatManager.ChatConfig temp in chatconifg)
        {
            if (temp.Languege == info.Languege)
                config = temp;
        }
        return config;
    }


    //读取故事文件
    public ChatManager.ChatActionBox LoadStory(string path, ChatManager.ChatConfig config)
    {
        ChatManager.ChatActionBox storylist = new ChatManager.ChatActionBox();

        TxtTool tt = new TxtTool();
        string[] txt = tt.ReadFile(config.Languege, path);

        storylist = ParseActionList(txt, config);
        
        //初始化index
        storylist.NowIndex = 0;
        
        return storylist;
    }

    //解析故事文件
    ChatManager.ChatActionBox ParseActionList(string[] story, ChatManager.ChatConfig config)
    {
        ChatManager.ChatActionBox box = new ChatManager.ChatActionBox();
        box.ActionList = new ArrayList();
        box.CharacterList = new Dictionary<string, ChatAction.StoryCharacter>();

        //读取故事的类型，如果为0则读取类型为角色模式，如果为1则读取类型为故事模式
        int loadType = 0;

        foreach (string str in story)
        {
            //如果碰见注释符号或为空行，则忽略本行
            if (str.Contains("//") || str == "")
                continue;

            //设置读取类型
            if (str == "[Character]")
            {
                loadType = 0;
                continue;
            }
            else if (str == "[Background]")
            {
                loadType = 1;
                continue;
            }
            else if (str == "[ChatList]")
            {
                loadType = 2;
                continue;
            }

            //读取角色模式的方法
            if (loadType == 0)
            {
                ChatAction.StoryCharacter character = new ChatAction.StoryCharacter();
                character.CharacterID = str.Substring(0, str.IndexOf("<"));
                character.Orientation = 1;

                string tempstr = str.Substring(str.IndexOf("<") + 1, str.IndexOf(">") - character.CharacterID.Length - 1);
                string[] parameter = tempstr.Split(';');

                for (int i = 0; i < parameter.Length; i++)
                {
                    //读取name
                    if (i == 0)
                        character.Name = parameter[i].Substring(5, parameter[i].Length - 5);
                    else if (i == 1)
                        character.Image = parameter[i].Substring(6, parameter[i].Length - 6);
                    else if (i == 2)
                        character.Windows = parameter[i].Substring(8, parameter[i].Length - 8);
                }

                box.CharacterList.Add(character.CharacterID, character);
            }
            //读取背景的方法
            if (loadType == 1)
            {
                box.BG = str.Split(',');
            }
            //读取故事模式的方法
            else if (loadType == 2)
            {
                //{rourou:idle}我是<t 200>大笨蛋<t 100>！！<s>
                //{rourou:idle}我是大笨蛋！！<s>

                //方法字段
                if (str[0] == '<')
                {
                    ChatAction.StoryAction action = new ChatAction.StoryAction();
                    action.Command = str.Substring(1, str.IndexOf(' ') - 1);
                    string tempstr = str.Substring(action.Command.Length + 2, str.Length - action.Command.Length - 3);
                    string[] parameters = tempstr.Split(',');

                    //设定参数大小
                    action.Parameter = new string[parameters.Length - 3];
                    //区分参数
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        //设置方法参数
                        if (i < parameters.Length - 3)
                        {
                            action.Parameter[i] = parameters[i];
                        }
                        //设置角色id参数
                        else if (i == parameters.Length - 3)
                        {
                            action.CharacterID = parameters[i];
                        }
                        //设置动作参数LOOPTYPE
                        else if (i == parameters.Length - 2)
                        {
                            if (parameters[i] == "notloop")
                                action.LoopType = ChatAction.LOOPTYPE.NOTLOOP;
                            else if (parameters[i] == "loop")
                                action.LoopType = ChatAction.LOOPTYPE.LOOP;
                        }
                        //设置进入下一步的方式SKIPTYPE
                        else if (i == parameters.Length - 1)
                        {
                            if (parameters[i] == "auto")
                                action.SkipType = ChatAction.SKIPTYPE.AUTO;
                            else if (parameters[i] == "click")
                                action.SkipType = ChatAction.SKIPTYPE.CLICK;
                            else if (parameters[i] == "sametime")
                                action.SkipType = ChatAction.SKIPTYPE.SAMETIME;
                        }
                    }
                    box.ActionList.Add(action);
                }
                //对话字段
                else if (str[0] == '{')
                {
                    string CharacterID = str.Substring(1, str.IndexOf(':') - 1);
                    string Command = "talk";
                    string Face = str.Substring(str.IndexOf(':') + 1, str.IndexOf('}') - str.IndexOf(':') - 1);

                    string tempstr = str.Substring(str.IndexOf('}') + 1, str.Length - str.IndexOf('}') - 1);

                    //第一条储存文本，第二条储存速度，第三条储存跳转方式
                    if (tempstr.Contains("<t "))
                    {
                        string[] tempwords = System.Text.RegularExpressions.Regex.Split(tempstr, "<t ");
                        int autoindex = box.ActionList.Count;

                        for (int i = 0; i < tempwords.Length; i++)
                        {
                            //设置速度，第一条为默认速度
                            string speed = "1";
                            if (i == 0)
                                speed = config.speed.ToString();
                            else
                                speed = tempwords[i].Substring(0, tempwords[i].IndexOf(">"));

                            string[] tempwords_click = System.Text.RegularExpressions.Regex.Split(tempwords[i], "<c>");
                            for (int j = 0; j < tempwords_click.Length; j++)
                            {
                                ChatAction.StoryAction action_click = new ChatAction.StoryAction();
                                action_click.CharacterID = CharacterID;
                                action_click.Command = Command;
                                action_click.Parameter = new string[4];

                                //移除前面的速度表示
                                if (i != 0 && j == 0)
                                    tempwords_click[j] = tempwords_click[j].Substring(tempwords_click[j].IndexOf(">") + 1, tempwords_click[j].Length - tempwords_click[j].IndexOf(">") - 1);

                                action_click.Parameter[0] = replaceRichText(tempwords_click[j], out action_click.Richparamater);
                                action_click.Parameter[1] = speed;
                                action_click.Parameter[2] = Face;
                                action_click.Parameter[3] = "nowpage";

                                action_click.SkipType = ChatAction.SKIPTYPE.CLICK;

                                //如果是第一个时间分割字段，则在除了最后一条，其余都为点击
                                if (j == tempwords_click.Length - 1)
                                    action_click.SkipType = ChatAction.SKIPTYPE.TimeAUTO;

                                //如果是最后一个时间分割条目，全点击
                                if (i == tempwords.Length - 1)
                                    action_click.SkipType = ChatAction.SKIPTYPE.CLICK;
                                
                                //检测最后一个条目是否为自动
                                if (i == tempwords.Length - 1 && j == tempwords_click.Length - 1)
                                {
                                    if (tempwords_click[j].Contains("<a>"))
                                    {
                                        action_click.Parameter[0] = action_click.Parameter[0].Substring(0, action_click.Parameter[0].Length - 3);
                                        action_click.SkipType = ChatAction.SKIPTYPE.AUTO;

                                        //把之前设置为能点点击跳过的动作设置为不能点击跳过，click除外
                                        for (int changeindex = autoindex; changeindex < box.ActionList.Count; changeindex++)
                                        {
                                            ChatAction.StoryAction action_change = (ChatAction.StoryAction)box.ActionList[changeindex];
                                            if (action_change.SkipType == ChatAction.SKIPTYPE.TimeAUTO)
                                            {
                                                action_change.SkipType = ChatAction.SKIPTYPE.AUTO;
                                                box.ActionList[changeindex] = action_change;
                                            }
                                        }
                                    }
                                    action_click.Parameter[3] = "endpage";
                                }                             
                                box.ActionList.Add(action_click);
                            }
                        }

                    }
                    else
                    {
                        //设置速度，第一条为默认速度
                        string speed = config.speed.ToString();

                        string[] tempwords_click = System.Text.RegularExpressions.Regex.Split(tempstr, "<c>");
                        for (int j = 0; j < tempwords_click.Length; j++)
                        {
                            ChatAction.StoryAction action_click = new ChatAction.StoryAction();

                            action_click.CharacterID = CharacterID;
                            action_click.Command = Command;
                            action_click.Parameter = new string[4];
                            action_click.Parameter[0] = replaceRichText(tempwords_click[j], out action_click.Richparamater);
                            action_click.Parameter[1] = speed;
                            action_click.Parameter[2] = Face;
                            action_click.Parameter[3] = "nowpage";

                            action_click.SkipType = ChatAction.SKIPTYPE.CLICK;

                            //检测是否为自动
                            if (j == tempwords_click.Length - 1)
                            {
                                if (tempwords_click[j].Contains("<a>"))
                                {
                                    action_click.Parameter[0] = action_click.Parameter[0].Substring(0, action_click.Parameter[0].Length - 3);
                                    action_click.SkipType = ChatAction.SKIPTYPE.AUTO;
                                }
                                action_click.Parameter[3] = "endpage";
                            }
                            box.ActionList.Add(action_click);
                        }
                    }
                }
            }
        }
        //最后一条强制设为click，因为最有一条动作执行完成之后点击关闭，在这里做判断少点
        return box;
    }

    //string replaceRichText(string str, out MatchCollection richparamater)
    //{
    //    string richstr = str;
    //    Regex reg = new Regex("(<.*?>)(.*?)(<.*?>)", RegexOptions.IgnoreCase);
    //    richparamater = reg.Matches(richstr);
    //    string replacestr = reg.Replace(richstr, @"$2");

    //    //richstr = reg.Replace(str,delegate(Match m)
    //    //{
    //    //    string richwords = Regex.Replace(m.Groups[2].Value, "(.)", delegate(Match m2)
    //    //    {
    //    //        string words = "";
    //    //        words += m.Groups[1].Value + m2.Value + m.Groups[3].Value;
    //    //        return words;
    //    //    });
    //    //    return richwords;
    //    //});

    //    //richparamater = reg.Matches(richstr);  
    //    return replacestr;
    //}

    string replaceRichText(string str, out MatchCollection richparamater)
    {
        string richstr = str;
        Regex reg = new Regex("(<.*?>)(.*?)(<.*?>)", RegexOptions.IgnoreCase);
        //richparamater = reg.Matches(richstr);
        //string replacestr = reg.Replace(richstr, @"$2");

        richstr = reg.Replace(str, delegate(Match m)
        {
            string richwords = Regex.Replace(m.Groups[2].Value, "(.)", delegate(Match m2)
            {
                string words = "";
                words += m.Groups[1].Value + m2.Value + m.Groups[3].Value;
                return words;
            });
            return richwords;
        });

        richparamater = reg.Matches(richstr);
        return richstr;
    }

}
