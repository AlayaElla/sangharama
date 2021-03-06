﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    ArrayList QuestList = new ArrayList();
    ArrayList QuesGrouptList = new ArrayList();

    ArrayList NowQuestInfoList = new ArrayList();
    ArrayList NewQuests = new ArrayList();

    QuestUI UIInstance;

    //定义任务的结构体
    public struct QuestBase
    {
        public int ID;
        public int Questgroup;
        public int GroupID;
        public string Smallicon;
        public string Bigicon;
        public string name;
        public string des;
        public string completedes;

        ////前置条件 暂时不需要
        //public struct QuestNeedBase
        //{
        //    public int PreQuest;
        //    public int[] NeedGoods;
        //}
        //public QuestNeedBase QuestNeed;

        //达成条件
        public struct QuestCompleteBase
        {
            public QuestTypeList QuestType;
            public int Num;
            public int[] Parameter;
        }
        public QuestCompleteBase QuestComplete;

        //奖励
        public struct AwardBase
        {
            public int Gold;
            public int Exp;
            public int[] Goods;
            public int GoodsNum;
            public int Event;
            public int TaskPoint;
        }
        public AwardBase Award;
    }

    public struct QuestGroupBase
    {

        public int ID;
        public GroupTypeList GroupType;
        public string Name;
    }

    //任务类型
    public enum QuestTypeList
    {
        PutGoods = 0,       //上架物品
        SellGoods,          //售出物品
        ComposeGoods,       //合成物品
        CollectGoods,       //采集物品
        ComposeProperty,    //合成属性
        Arrive,             //到达路点
        Golds               //持有金币数
    };

    //任务类型
    public enum GroupTypeList
    {
        Main = 0,         //主线
        Sub,          //支线
        Secret       //秘密
    };

	// Use this for initialization
	void Start () {
        XmlTool xt = new XmlTool();
        QuestList = xt.loadQuestXmlToArray();
        QuesGrouptList = xt.loadQuestGroupXmlToArray();

        //获取UI
        UIInstance = gameObject.GetComponent<QuestUI>();

        InstQuest();
        //AddShowQuest(1);
        //AddShowQuest(2);
        //AddShowQuest(3);
        //AddShowQuest(4);
        //AddShowQuest(5);
        //AddShowQuest(6);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //游戏开始时，初始化任务面板
    void InstQuest()
    {
        ArrayList questlist = PlayerInfo.GetPlayerInfo().QuestList;
        foreach (PlayerInfo.QuestInfo info in questlist)
        {
            //添加任务信息记录，方便以后查找
            QuestBase Quest = new QuestBase();
            Quest = GetQuestInfoByID(info.ID);
            NowQuestInfoList.Add(Quest);
            UIInstance.AddQustUI(GetQuestInfoByID(info.ID));
        }
    }


    public void AddShowQuest(int questID)
    {
        PlayerInfo.QuestInfo info = PlayerInfo.GetQuestInfo(questID);
        if (info.ID == 0&& !PlayerInfo.CheckCompleteQuest(questID))
        {
            AddQuestToList(questID);
            UIInstance.AddQustUI(GetQuestInfoByID(questID));
        }
        else
        {
            Debug.Log("已经添加该任务；或已经完成这个任务: " + questID);
        }
    }

    //添加任务到显示new的列表中
    void AddQuestToNew(int questID)
    {
        NewQuests.Add(questID);
    }
    //去除任务显示new
    void RemoveQuestToNew(int questID)
    {
        if(NewQuests.Count>0)
            NewQuests.Remove(questID);
    }

    public ArrayList GetNewQuests()
    {
        return NewQuests;
    }

    //添加任务信息到列表
    void AddQuestToList(int questID)
    {
        //添加任务信息记录，方便以后查找
        QuestBase Quest = new QuestBase();
        Quest = GetQuestInfoByID(questID);
        NowQuestInfoList.Add(Quest);

        //初始化任务信息到角色信息
        PlayerInfo.QuestInfo newQuest = new PlayerInfo.QuestInfo();
        newQuest.ID = Quest.ID;
        newQuest.Goal = Quest.QuestComplete.Num;
        newQuest.Progress = 0;
        newQuest.Type = PlayerInfo.QuestInfo.QuestInfoType.Todo;
        newQuest.TaskPoint = Quest.Award.TaskPoint;
        PlayerInfo.AddQuest(newQuest);
        AddQuestToNew(questID);
    }

    //移除任务信息
    public void RemoveQuestToList(int questID)
    {
        if (NowQuestInfoList.Contains(questID))
            NowQuestInfoList.Remove(questID);
    }

    //查找任务信息
    public QuestBase GetQuestInfoByID(int questID)
    {
        foreach (QuestBase q in QuestList)
        {
            if (q.ID == questID)
            {
                return q;
            }
        }
        Debug.LogWarning("Can't find questID: " + questID);
        return new QuestBase();
    }

    //查找任务组信息
    QuestGroupBase GetQuestGroupInfo(int groupID)
    {
        foreach (QuestGroupBase g in QuesGrouptList)
        {
            if (g.ID == groupID)
            {
                return g;
            }
        }
        Debug.LogWarning("Can't find questgroupID: " + groupID);
        return new QuestGroupBase();
    }

    public int GetQuestProgress(int questID)
    {
        foreach (PlayerInfo.QuestInfo q in PlayerInfo.GetPlayerInfo().QuestList)
        {
            if (q.ID == questID)
            {
                return q.Progress;
            }
        }

        Debug.LogWarning("Can't find questID: " + questID);
        return 0;
    }

    public void OpenQuestBoardInUI(int questID,float delyTime)
    {
        RemoveQuestToNew(questID);
        UIInstance.OpenQuestBoard(questID, delyTime);
    }

    /// <summary>
    /// 用于检查是否触发事件,物品判断的条件不管使用哪个EventType都一样,point是玩家所在的位置
    /// </summary>
    /// <param name="EventType"></param>
    /// <returns></returns>
    public bool CheckQuestListWithGoods(QuestTypeList EventType,CharBag.Goods goods,int point)
    {
        bool ishit = false;
        //事件判断
        foreach (QuestBase _event in NowQuestInfoList)
        {
            switch (_event.QuestComplete.QuestType)
            {
                //给物品相关的判断，统一处理
                case QuestTypeList.PutGoods:
                case QuestTypeList.SellGoods:
                case QuestTypeList.ComposeGoods:
                case QuestTypeList.CollectGoods:
                case QuestTypeList.ComposeProperty:
                    if (_event.QuestComplete.Parameter == null || _event.QuestComplete.Parameter.Length <= 0) Debug.LogWarning("任务配置表Parameter出错！ questID:" + _event.ID);
                    if (_event.QuestComplete.QuestType == QuestTypeList.ComposeProperty)
                    {
                        foreach (int p in goods.Property)
                        {
                            if (p == _event.QuestComplete.Parameter[0] && _event.QuestComplete.Num > PlayerInfo.GetQuestProgress(_event.ID) && PlayerInfo.GetQuestStatus(_event.ID) == PlayerInfo.QuestInfo.QuestInfoType.Todo)
                            {
                                PlayerInfo.AddQuestProgress(_event.ID, 1, _event.QuestComplete.Num);
                                UIInstance.UpdateQuestUI(_event.ID, point);
                                ishit = true;
                            }
                        }
                    }
                    else
                    {
                        if (_event.QuestComplete.Parameter[0] == 0)
                        {
                            if (goods.ID == _event.QuestComplete.Parameter[1] && _event.QuestComplete.Num > PlayerInfo.GetQuestProgress(_event.ID) && PlayerInfo.GetQuestStatus(_event.ID) == PlayerInfo.QuestInfo.QuestInfoType.Todo)
                            {
                                if (_event.QuestComplete.QuestType == QuestTypeList.PutGoods && _event.QuestComplete.QuestType == EventType)
                                {
                                    PlayerInfo.AddQuestProgress(_event.ID, 1, _event.QuestComplete.Num);
                                    UIInstance.UpdateQuestUI(_event.ID, point);
                                    ishit = true;
                                }
                                else if (_event.QuestComplete.QuestType == QuestTypeList.SellGoods && _event.QuestComplete.QuestType == EventType)
                                {
                                    PlayerInfo.AddQuestProgress(_event.ID, 1, _event.QuestComplete.Num);
                                    UIInstance.UpdateQuestUI(_event.ID, point);
                                    ishit = true;
                                }
                                else if (_event.QuestComplete.QuestType == QuestTypeList.ComposeGoods && _event.QuestComplete.QuestType == EventType)
                                {
                                    PlayerInfo.AddQuestProgress(_event.ID, 1, _event.QuestComplete.Num);
                                    UIInstance.UpdateQuestUI(_event.ID, point);
                                    ishit = true;
                                }
                                else if (_event.QuestComplete.QuestType == QuestTypeList.CollectGoods && _event.QuestComplete.QuestType == EventType)
                                {
                                    PlayerInfo.AddQuestProgress(_event.ID, 1, _event.QuestComplete.Num);
                                    UIInstance.UpdateQuestUI(_event.ID, point);
                                    ishit = true;
                                }
                            }
                        }   //item
                    }
                    break;
            }
        }
        if(ishit==true) PlayerData.PlayerInfoData.Save(PlayerInfo.GetPlayerInfo());
        return ishit;
    }

    //检查是否到达路点的任务
    public bool CheckQuestListWithArrive(int point)
    {
        bool ishit = false;
        foreach (QuestBase _event in NowQuestInfoList)
        {
            if (_event.QuestComplete.QuestType == QuestTypeList.Arrive
                && _event.QuestComplete.Num > PlayerInfo.GetQuestProgress(_event.ID)
                && PlayerInfo.GetQuestStatus(_event.ID) == PlayerInfo.QuestInfo.QuestInfoType.Todo
                && _event.QuestComplete.Parameter[0] == point)
            {
                PlayerInfo.AddQuestProgress(_event.ID, 1, _event.QuestComplete.Num);
                UIInstance.UpdateQuestUI(_event.ID, point);
                ishit = true;
            }
        }
        if (ishit == true) PlayerData.PlayerInfoData.Save(PlayerInfo.GetPlayerInfo());
        return ishit;
    }

    //检查是否和获取多少金钱的任务
    public bool CheckQuestListWithGold(int Num, int point)
    {
        bool ishit = false;
        foreach (QuestBase _event in NowQuestInfoList)
        {
            if (_event.QuestComplete.QuestType == QuestTypeList.Golds
                && _event.QuestComplete.Num > PlayerInfo.GetQuestProgress(_event.ID)
                && PlayerInfo.GetQuestStatus(_event.ID) == PlayerInfo.QuestInfo.QuestInfoType.Todo)
            {
                PlayerInfo.AddQuestProgress(_event.ID, Num, _event.QuestComplete.Num);
                UIInstance.UpdateQuestUI(_event.ID, point);
                ishit = true;
            }
        }
        if (ishit == true) PlayerData.PlayerInfoData.Save(PlayerInfo.GetPlayerInfo());
        return ishit;
    }

    public void TestQuest()
    {
        PlayerInfo.ClearQuestList();

        //int type = 0;
        //int id = 2;
        //CharBag.Goods newgoods = new CharBag.Goods();
        //newgoods.MateriralType = type;
        //newgoods.ID = id;
        //newgoods.Number = 1;
        //newgoods.Property = Materiral.GetMaterialProperty(type, id);
        //newgoods.Quality = 80;

        //CheckQuestListWithGoods(QuestTypeList.PutGoods, newgoods, 0);
    }

    public void PreCheckQuest(int sceneID)
    {
        ArrayList questlist = PlayerInfo.GetPlayerInfo().QuestList;
        ArrayList preCheckQuest = new ArrayList();
        foreach (PlayerInfo.QuestInfo info in questlist)
        {
            if (info.Type == PlayerInfo.QuestInfo.QuestInfoType.WaitingCheck)
            {
                Debug.Log("prehit quest: " + info.ID);
                preCheckQuest.Add(info);
            }
        }
        if (preCheckQuest.Count > 0)
            UIInstance.ShowEventHint(sceneID, preCheckQuest);
    }

    public bool IsArriveWaitingCheckPoint(int point)
    {
        if (point < 0) return false;

        ArrayList questlist = PlayerInfo.GetPlayerInfo().QuestList;
        foreach (PlayerInfo.QuestInfo info in questlist)
        {
            if (info.Type == PlayerInfo.QuestInfo.QuestInfoType.WaitingCheck && info.TaskPoint == point)
            {
                UIInstance.OpenQuestBoard(info.ID, 0);
                return true;
            }
        }
        return false;
    }
}
