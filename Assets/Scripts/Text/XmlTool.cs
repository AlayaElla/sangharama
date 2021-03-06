﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.Text;
using System;

public class XmlTool
{
    //////////////////////////////////////////
    //读取XML表
    public ArrayList loadCollectionXmlToArray()
    {
        //保存路径
        string filepath = "Config/Materiral/Collection";

        string _result = Resources.Load(filepath).ToString();

        ArrayList collection = new ArrayList();
                    
        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList=xmlDoc.SelectSingleNode("CollectionList").ChildNodes;

        foreach(XmlElement map in nodeList)
        {
            CollectAction.CollectionMap collectionMap = new CollectAction.CollectionMap();
            collectionMap.RandomQuality = new int[2];
            collectionMap.RandomProperty = new int[4, 2];

            //读取node内属性，把string转化为对应的属性
            if (map.GetAttribute("Map") != "")
                collectionMap.Map = int.Parse(map.GetAttribute("Map"));
            if (map.GetAttribute("MateriralType") != "")
                collectionMap.MateriralType = int.Parse(map.GetAttribute("MateriralType"));
            if (map.GetAttribute("ID") != "")
                collectionMap.ID = int.Parse(map.GetAttribute("ID"));
            if (map.GetAttribute("Weight") != "")
                collectionMap.Weight = int.Parse(map.GetAttribute("Weight"));
            if(map.GetAttribute("RandomQuality")!="")
            {
                string qualityString = map.GetAttribute("RandomQuality");
                string[] quaList = qualityString.Split(',');

                for (int i = 0; i < quaList.Length; i++)
                {
                    collectionMap.RandomQuality[i] = int.Parse(quaList[i]);
                }
            }
            if (map.GetAttribute("PropertyProbability") != "")
                collectionMap.PropertyProbability = int.Parse(map.GetAttribute("PropertyProbability"));
            if ((map.GetAttribute("RandomProperty") != "") && (map.GetAttribute("PropertyWeight") != ""))
            {
                string proString = map.GetAttribute("RandomProperty");
                string[] proList = proString.Split(',');

                string weiString = map.GetAttribute("PropertyWeight");
                string[] weiList = weiString.Split(',');

                for (int i = 0; i < proList.Length; i++)
                {
                    collectionMap.RandomProperty[i, 0] = int.Parse(proList[i]);
                    collectionMap.RandomProperty[i, 1] = int.Parse(weiList[i]);
                }
            }
            collection.Add(collectionMap);
        }
        return collection;
    }

    public ArrayList loadItemXmlToArray()
    {
        //保存路径
        string filepath = "Config/Materiral/Item";

        string _result = Resources.Load(filepath).ToString();

        ArrayList itemList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("ItemList").ChildNodes;

        foreach (XmlElement item in nodeList)
        {
            Materiral.Items _items = new Materiral.Items();

            //读取node内属性，把string转化为对应的属性
            if (item.GetAttribute("ID") != "")
                _items.ID = int.Parse(item.GetAttribute("ID"));
            if (item.GetAttribute("Name") != "")
                _items.Name = item.GetAttribute("Name");
            if (item.GetAttribute("Image") != "")
                _items.IMG = item.GetAttribute("Image");
            if (item.GetAttribute("Type") != "")
                _items.Type = int.Parse(item.GetAttribute("Type"));
            if (item.GetAttribute("Price") != "")
                _items.Price = int.Parse(item.GetAttribute("Price"));
            if (item.GetAttribute("Property") != "")
            {
                string[] _proStr = item.GetAttribute("Property").Split(',');
                _items.Property = new int[_proStr.Length];
                for (int i = 0; i < _proStr.Length; i++)
                {
                    _items.Property[i] = int.Parse(_proStr[i]);
                }
            }
            if (item.GetAttribute("des") != "")
                _items.Des = item.GetAttribute("des");

            //添加进itemList中
            itemList.Add(_items);
        }
        return itemList;
    }

    public ArrayList loadCharacterXmlToArray()
    {
        //保存路径
        string filepath = "Config/Character/CharacterList";

        string _result = Resources.Load(filepath).ToString();

        ArrayList characterList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("CharacterList").ChildNodes;

        foreach (XmlElement character in nodeList)
        {
            CharacterModle.CharacterInfo _character = new CharacterModle.CharacterInfo();

            //读取node内属性，把string转化为对应的属性
            if (character.GetAttribute("ID") != "")
                _character.ID = int.Parse(character.GetAttribute("ID"));
            if (character.GetAttribute("Name") != "")
                _character.Name = character.GetAttribute("Name");
            if (character.GetAttribute("Skin") != "")
                _character.Skin = character.GetAttribute("Skin");
            if (character.GetAttribute("Type") != "")
                _character.Sex = (CharacterModle.SexType)int.Parse(character.GetAttribute("Sex"));
            if (character.GetAttribute("OutTime") != "")
                _character.OutTime = (CharacterModle.TimeType)int.Parse(character.GetAttribute("OutTime"));

            if (character.GetAttribute("FavoriteItems") != "")
            {
                string[] _items = character.GetAttribute("FavoriteItems").Split(',');
                _character.FavoriteItems = new int[_items.Length];
                for(int i=0;i<_items.Length;i++)
                {
                    _character.FavoriteItems[i] = int.Parse(_items[i]);
                }
            }
            if (character.GetAttribute("FavoriteMinds") != "")
            {
                string[] _minds = character.GetAttribute("FavoriteMinds").Split(',');
                _character.FavoriteMinds = new int[_minds.Length];
                for (int i = 0; i < _minds.Length; i++)
                {
                    _character.FavoriteMinds[i] = int.Parse(_minds[i]);
                }
            }
            if (character.GetAttribute("Weight") != "")
                _character.Weight = int.Parse(character.GetAttribute("Weight"));
            if (character.GetAttribute("Des") != "")
                _character.Des = character.GetAttribute("Des");


            //添加进itemList中
            characterList.Add(_character);
        }
        return characterList;
    }

    public ArrayList loadSkinXmlToArray()
    {
        //保存路径
        string filepath = "Config/Character/skinConfig";

        string _result = Resources.Load(filepath).ToString();

        ArrayList skinList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("SkinList").ChildNodes;

        foreach (XmlElement skin in nodeList)
        {
            CharacterModle.Skin _skin = new CharacterModle.Skin();

            //读取node内属性，把string转化为对应的属性
            if (skin.GetAttribute("SkinName") != "")
                _skin.SkinName = skin.GetAttribute("SkinName");
            for (int i = 1; i <= skin.Attributes.Count - 1; i++)
            {
                string _att = "Action" + i.ToString();
                if (skin.HasAttribute(_att))
                {
                    string _temp = skin.GetAttribute(_att);
                    int index = _temp.IndexOf(":");
                    string _name = _temp.Substring(0, index);
                    string[] _value = _temp.Substring(index + 1).Split(',');
                    _skin.ActionList[_name] = new int[2];
                    _skin.ActionList[_name][0] = int.Parse(_value[0]);
                    _skin.ActionList[_name][1] = int.Parse(_value[1]);
                }
            }

            //添加进itemList中
            skinList.Add(_skin);
        }
        return skinList;
    }

    public ArrayList loadMindXmlToArray()
    {
        //保存路径
        string filepath = "Config/Materiral/Mind";

        string _result = Resources.Load(filepath).ToString();

        ArrayList mindList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("MindList").ChildNodes;

        foreach (XmlElement mind in nodeList)
        {
            Materiral.Minds _mind = new Materiral.Minds();

            //读取node内属性，把string转化为对应的属性
            if (mind.GetAttribute("ID") != "")
                _mind.ID = int.Parse(mind.GetAttribute("ID"));
            if (mind.GetAttribute("Name") != "")
                _mind.Name = mind.GetAttribute("Name");
            if (mind.GetAttribute("Image") != "")
                _mind.IMG = mind.GetAttribute("Image");
            if (mind.GetAttribute("Type") != "")
                _mind.Type = int.Parse(mind.GetAttribute("Type"));
            if (mind.GetAttribute("Price") != "")
                _mind.Price = int.Parse(mind.GetAttribute("Price"));
            if (mind.GetAttribute("Property") != "")
            {
                string[] _proStr = mind.GetAttribute("Property").Split(',');
                _mind.Property = new int[_proStr.Length];
                for (int i=0;i<_proStr.Length;i++)
                {
                    _mind.Property[i] = int.Parse(_proStr[i]);
                }
            }
            if (mind.GetAttribute("des") != "")
                _mind.Des = mind.GetAttribute("des");

            //添加进itemList中
            mindList.Add(_mind);
        }
        return mindList;
    }


    public ArrayList loadSpecialItemXmlToArray()
    {

        //保存路径
        string filepath = "Config/Materiral/SpecialItem";

        string _result = Resources.Load(filepath).ToString();

        ArrayList specialItemList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("SpecialItemList").ChildNodes;

        foreach (XmlElement special in nodeList)
        {
            Materiral.SpecialItem _sitem = new Materiral.SpecialItem();

            //读取node内属性，把string转化为对应的属性
            if (special.GetAttribute("ID") != "")
                _sitem.ID = int.Parse(special.GetAttribute("ID"));
            if (special.GetAttribute("Name") != "")
                _sitem.Name = special.GetAttribute("Name");
            if (special.GetAttribute("Image") != "")
                _sitem.IMG = special.GetAttribute("Image");
            if (special.GetAttribute("Type") != "")
                _sitem.Type = int.Parse(special.GetAttribute("Type"));
            if (special.GetAttribute("Price") != "")
                _sitem.Price = int.Parse(special.GetAttribute("Price"));
            if (special.GetAttribute("Property") != "")
            {
                string[] _proStr = special.GetAttribute("Property").Split(',');
                _sitem.Property = new int[_proStr.Length];
                for (int i = 0; i < _proStr.Length; i++)
                {
                    _sitem.Property[i] = int.Parse(_proStr[i]);
                }
            }
            if (special.GetAttribute("des") != "")
                _sitem.Des = special.GetAttribute("des");

            //添加进itemList中
            specialItemList.Add(_sitem);
        }
        return specialItemList;
    }

    public ArrayList loadTypeXmlToArray()
    {
        ArrayList _typeList = new ArrayList();

        string filepath = "Config/Materiral/MaterialType";
        string _result = Resources.Load(filepath).ToString();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(_result);
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("TypeList").ChildNodes;

        foreach (XmlElement type in nodeList)
        {
            Materiral.MaterialType _type = new Materiral.MaterialType();

            if (type.GetAttribute("ID") != "")
                _type.ID = int.Parse(type.GetAttribute("ID"));

            if (type.GetAttribute("Name") != "")
                _type.Name = type.GetAttribute("Name");

            if (type.GetAttribute("Image") != "")
                _type.IMG = type.GetAttribute("Image");

            _typeList.Add(_type);
        }

        return _typeList;
    }

    public ArrayList loadRecipeXmlToArray()
    {
        //保存路径
        string filepath = "Config/Materiral/Recipe";

        string _result = Resources.Load(filepath).ToString();

        ArrayList recipeList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("RecipeList").ChildNodes;

        foreach (XmlElement recipe in nodeList)
        {
            Recipe.RecipeMap _recipe = new Recipe.RecipeMap();

            //读取node内属性，把string转化为对应的属性
            if (recipe.GetAttribute("ID") != "")
                _recipe.ID = int.Parse(recipe.GetAttribute("ID"));
            if (recipe.GetAttribute("Name") != "")
                _recipe.Name = recipe.GetAttribute("Name");
            if (recipe.GetAttribute("Target") != "")
                _recipe.Target = recipe.GetAttribute("Target");
            if (recipe.GetAttribute("Mar1Qua") != "")
                _recipe.EffetBox.Eft1.NeedQuality = int.Parse(recipe.GetAttribute("Mar1Qua"));
            if (recipe.GetAttribute("Mar1Eft") != "")
                _recipe.EffetBox.Eft1.EftID = int.Parse(recipe.GetAttribute("Mar1Eft"));
            if (recipe.GetAttribute("Mar2Qua") != "")
                _recipe.EffetBox.Eft2.NeedQuality = int.Parse(recipe.GetAttribute("Mar2Qua"));
            if (recipe.GetAttribute("Mar2Eft") != "")
                _recipe.EffetBox.Eft2.EftID = int.Parse(recipe.GetAttribute("Mar2Eft"));
            if (recipe.GetAttribute("Mar3Qua") != "")
                _recipe.EffetBox.Eft3.NeedQuality = int.Parse(recipe.GetAttribute("Mar3Qua"));
            if (recipe.GetAttribute("Mar3Eft") != "")
                _recipe.EffetBox.Eft3.EftID = int.Parse(recipe.GetAttribute("Mar3Eft"));

            _recipe.Slots = new Recipe.Slot[recipe.ChildNodes.Count];

            for (int i = 0; i < recipe.ChildNodes.Count; i++)
            {
                XmlElement element = (XmlElement)recipe.ChildNodes[i];
                string slotName = "Slot" + (i + 1).ToString();

                if (element.Name == slotName)
                {
                    if (element.GetAttribute("NeedNum") != "")
                        _recipe.Slots[i].Num = int.Parse(element.GetAttribute("NeedNum"));

                    string mat = element.InnerText;

                    //解析材料内容
                    if (mat[0] == char.Parse("T"))
                    {
                        _recipe.Slots[i].SlotType = (Recipe.SlotTypeList)1;
                        string mat_str = mat.Substring(mat.IndexOf(":") + 1);
                        _recipe.Slots[i].MatType = int.Parse(mat_str);
                        _recipe.Slots[i].MatId = -1;
                    }
                    else if (mat[0] == char.Parse("M"))
                    {
                        _recipe.Slots[i].SlotType = (Recipe.SlotTypeList)0;

                        string mat_type = mat.Substring(mat.IndexOf(":") + 1, mat.IndexOf(",") - 2);
                        string mat_str = mat.Substring(mat.IndexOf(",") + 1);
                        _recipe.Slots[i].MatType = int.Parse(mat_type);
                        _recipe.Slots[i].MatId = int.Parse(mat_str);
                    }
                }
            }

            //添加进itemList中
            recipeList.Add(_recipe);
        }
        return recipeList;
    }

    //读取材料属性列表
    public ArrayList loadPropertyEffetXmlToArray()
    {
        //保存路径
        string filepath = "Config/Materiral/PropertyEffet";

        string _result = Resources.Load(filepath).ToString();

        ArrayList propertyList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("PropertyList").ChildNodes;

        foreach (XmlElement property in nodeList)
        {
            Materiral.Property _property = new Materiral.Property();

            //读取node内属性，把string转化为对应的属性
            if (property.GetAttribute("ID") != "")
                _property.ID = int.Parse(property.GetAttribute("ID"));
            if (property.GetAttribute("Name") != "")
                _property.Name = property.GetAttribute("Name");
            if (property.GetAttribute("Image") != "")
                _property.IMG = property.GetAttribute("Image");
            if (property.GetAttribute("des") != "")
                _property.Des = property.GetAttribute("des");
            if (property.GetAttribute("Effet") != "")
                _property.Effet = int.Parse(property.GetAttribute("Effet"));

            //添加进itemList中
            propertyList.Add(_property);
        }
        return propertyList;
    }

    //读取合成属性列表
    public ArrayList loadMaterialEffetXmlToArray()
    {
        //保存路径
        string filepath = "Config/Materiral/MaterialEffet";

        string _result = Resources.Load(filepath).ToString();

        ArrayList materialEffetList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("MaterialEffetList").ChildNodes;

        foreach (XmlElement property in nodeList)
        {
            Materiral.Effect _effect = new Materiral.Effect();

            //读取node内属性，把string转化为对应的属性
            if (property.GetAttribute("ID") != "")
                _effect.ID = int.Parse(property.GetAttribute("ID"));
            if (property.GetAttribute("Name") != "")
                _effect.Name = property.GetAttribute("Name");
            if (property.GetAttribute("Image") != "")
                _effect.IMG = property.GetAttribute("Image");
            if (property.GetAttribute("des") != "")
                _effect.Des = property.GetAttribute("des");
            if (property.GetAttribute("Effet") != "")
                _effect.Effet = int.Parse(property.GetAttribute("Effet"));

            //添加进itemList中
            materialEffetList.Add(_effect);
        }
        return materialEffetList;
    }

    //读取属性配方表
    public ArrayList loadPropertyRecipeXmlToArray()
    {
        //保存路径
        string filepath = "Config/Materiral/PropertyRecipe";

        string _result = Resources.Load(filepath).ToString();

        ArrayList recipeList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("PropertyRecipeList").ChildNodes;

        foreach (XmlElement recipe in nodeList)
        {
            Recipe.PropertyRecipe _recipe = new Recipe.PropertyRecipe();

            //读取node内属性，把string转化为对应的属性
            if (recipe.GetAttribute("ID") != "")
                _recipe.ID = int.Parse(recipe.GetAttribute("ID"));
            if (recipe.GetAttribute("Target") != "")
                _recipe.Target = int.Parse(recipe.GetAttribute("Target"));

            _recipe.Slots = new int[recipe.ChildNodes.Count];

            for (int i = 0; i < recipe.ChildNodes.Count; i++)
            {
                XmlElement element = (XmlElement)recipe.ChildNodes[i];
                string slotName = "Property" + (i + 1).ToString();

                if (element.Name == slotName)
                {
                    int mat = int.Parse(element.InnerText);
                    _recipe.Slots[i] = mat;
                }
            }

            //添加进itemList中
            recipeList.Add(_recipe);
        }
        return recipeList;
    }

    //读取路径配置表
    public ArrayList loadPathXmlToArray()
    {
        //保存路径
        string filepath = "Config/Map/MapPath";

        string _result = Resources.Load(filepath).ToString();

        ArrayList PathList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("PathList").ChildNodes;

        foreach (XmlElement recipe in nodeList)
        {

            MapPathManager.Path _path = new MapPathManager.Path();

            //读取node内属性，把string转化为对应的属性
            if (recipe.GetAttribute("Map") != "")
                _path.Map = int.Parse(recipe.GetAttribute("Map"));
            if (recipe.GetAttribute("Name") != "")
                _path.Name = recipe.GetAttribute("Name");
            if (recipe.GetAttribute("Next") != "")
            {
                string str_next = recipe.GetAttribute("Next");
                if (str_next != "")
                {
                    string[] str_temp = str_next.Split(',');
                    _path.Next = new int[str_temp.Length];
                    for (int i = 0; i < _path.Next.Length; i++)
                    {
                        _path.Next[i] = int.Parse(str_temp[i]);
                    }
                }
            }
            if (recipe.GetAttribute("Pre") != "")
            {
                string str_pre = recipe.GetAttribute("Pre");
                if (str_pre != "")
                {
                    string[] str_temp = str_pre.Split(',');
                    _path.Pre = new int[str_temp.Length];
                    for (int i = 0; i < _path.Pre.Length; i++)
                    {
                        _path.Pre[i] = int.Parse(str_temp[i]);
                    }
                }
            }
            if (recipe.GetAttribute("Price") != "")
                _path.Price = int.Parse(recipe.GetAttribute("Price"));
            else
                _path.Price = 0;
            //添加进itemList中
            PathList.Add(_path);
        }
        return PathList;
    }


     //读取文本配置表
    public ArrayList loadChatConfigXmlToArray()
    {
        //保存路径
        string filepath = "Config/Story/ChatConfig";

        string _result = Resources.Load(filepath).ToString();

        ArrayList ChatConfig = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("ChatConfig").ChildNodes;

        foreach (XmlElement config in nodeList)
        {
            ChatManager.ChatConfig _chatconfig = new ChatManager.ChatConfig();

            //读取node内属性，把string转化为对应的属性
            if (config.GetAttribute("Languege") != "")
                _chatconfig.Languege = config.GetAttribute("Languege");
            if (config.GetAttribute("Speed") != "")
                _chatconfig.speed = float.Parse(config.GetAttribute("Speed"));
            //添加进itemList中
            ChatConfig.Add(_chatconfig);
        }
        return ChatConfig;
    }

    //读取事件配置表
    public ArrayList loadChatEventListXmlToArray()
    {
        //保存路径
        string filepath = "Config/Story/ChatEventList";

        string _result = Resources.Load(filepath).ToString();

        ArrayList ChatEventsList = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("ChatEventList").ChildNodes;

        foreach (XmlElement events in nodeList)
        {

            ChatEventManager.ChatEvent _event = new ChatEventManager.ChatEvent();

            //读取node内属性，把string转化为对应的属性
            if (events.GetAttribute("ID") != "")
                _event.ID = int.Parse(events.GetAttribute("ID"));
            if (events.GetAttribute("GroupType") != "")
                _event.GroupType = int.Parse(events.GetAttribute("GroupType"));
            if (events.GetAttribute("EventType") != "")
            {
                switch (events.GetAttribute("EventType"))
                {
                    case "PutGoods":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.PutGoods;
                        break;
                    case "SellGoods":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.SellGoods;
                        break;
                    case "ComposeGoods":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.ComposeGoods;
                        break;
                    case "CollectGoods":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.CollectGoods;
                        break;
                    case "ComposeProperty":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.ComposeProperty;
                        break;
                    case "InShop":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.InShop;
                        break;
                    case "InMap":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.InMap;
                        break;
                    case "Arrive":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.Arrive;
                        break;
                    case "Mines":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.Mines;
                        break;
                    case "Golds":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.Golds;
                        break;
                    case "Quest":
                        _event.EventType = ChatEventManager.ChatEvent.EventTypeList.Quests;
                        break;
                    default:
                        Debug.LogError("Unkown EventType:" + events.GetAttribute("EventType") + "!!!!");
                        break;
                }
            }
            if (events.GetAttribute("Num") != "")
                _event.Num = int.Parse(events.GetAttribute("Num"));
            if (events.GetAttribute("Parameter") != "")
            {
                string[] ParameterList = events.GetAttribute("Parameter").Split(',');
                _event.Parameter = new int[ParameterList.Length];
                for (int i = 0; i < ParameterList.Length; i++)
                {
                    _event.Parameter[i] = int.Parse(ParameterList[i]);
                }
            }
            if (events.GetAttribute("EventItem") != "")
            {
                string[] EventItemList = events.GetAttribute("EventItem").Split(',');
                _event.EventItem = new int[EventItemList.Length];
                for (int i = 0; i < EventItemList.Length; i++)
                {
                    _event.EventItem[i] = int.Parse(EventItemList[i]);
                }
            }
            if (events.GetAttribute("NeedQuest") != "")
                _event.NeedQuest = int.Parse(events.GetAttribute("NeedQuest"));
            if (events.GetAttribute("StoryName") != "")
                _event.StoryName = events.GetAttribute("StoryName");
            //添加进itemList中
            ChatEventsList.Add(_event);
        }
        return ChatEventsList;
    }


    //读取任务配置
    public ArrayList loadQuestXmlToArray()
    {
        //保存路径
        string filepath = "Config/Quest/QuestList";

        string _result = Resources.Load(filepath).ToString();

        ArrayList List = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("QuestList").ChildNodes;

        foreach (XmlElement quest in nodeList)
        {
            QuestManager.QuestBase _quest = new QuestManager.QuestBase();

            //读取node内属性，把string转化为对应的属性
            if (quest.GetAttribute("ID") != "")
                _quest.ID = int.Parse(quest.GetAttribute("ID"));
            if (quest.GetAttribute("Questgroup") != "")
                _quest.Questgroup = int.Parse(quest.GetAttribute("Questgroup"));
            if (quest.GetAttribute("GroupID") != "")
                _quest.GroupID = int.Parse(quest.GetAttribute("GroupID"));
            if (quest.GetAttribute("Smallicon") != "")
                _quest.Smallicon = quest.GetAttribute("Smallicon");
            if (quest.GetAttribute("Bigicon") != "")
                _quest.Bigicon = quest.GetAttribute("Bigicon");
            if (quest.GetAttribute("Name") != "")
                _quest.name = quest.GetAttribute("Name");
            if (quest.GetAttribute("Des") != "")
                _quest.des = quest.GetAttribute("Des");
            if (quest.GetAttribute("CompleteDes") != "")
                _quest.completedes = quest.GetAttribute("CompleteDes");

            for (int i = 0; i < quest.ChildNodes.Count; i++)
            {
                XmlElement element = (XmlElement)quest.ChildNodes[i];

                //暂时不需要前置条件
                //if (element.Name == "Need")
                //{
                //    if (element.GetAttribute("Quest") != "")
                //        _quest.QuestNeed.PreQuest = int.Parse(element.GetAttribute("Quest"));
                //    if (element.GetAttribute("Goods") != "")
                //    {
                //        string[] Goods = element.GetAttribute("Goods").Split(',');
                //        _quest.QuestNeed.NeedGoods = new int[Goods.Length];
                //        for (int j = 0; j < Goods.Length; j++)
                //        {
                //            _quest.QuestNeed.NeedGoods[j] = int.Parse(Goods[j]);
                //        }
                //    }
                //}

                if (element.Name == "Complete")
                {
                    if (element.GetAttribute("QuestType") != "")
                    {
                        switch (element.GetAttribute("QuestType"))
                        {
                            case "PutGoods":
                                _quest.QuestComplete.QuestType = QuestManager.QuestTypeList.PutGoods;
                                break;
                            case "SellGoods":
                                _quest.QuestComplete.QuestType = QuestManager.QuestTypeList.SellGoods;
                                break;
                            case "ComposeGoods":
                                _quest.QuestComplete.QuestType = QuestManager.QuestTypeList.ComposeGoods;
                                break;
                            case "CollectGoods":
                                _quest.QuestComplete.QuestType = QuestManager.QuestTypeList.CollectGoods;
                                break;
                            case "ComposeProperty":
                                _quest.QuestComplete.QuestType = QuestManager.QuestTypeList.ComposeProperty;
                                break;
                            case "Arrive":
                                _quest.QuestComplete.QuestType = QuestManager.QuestTypeList.Arrive;
                                break;
                            case "Golds":
                                _quest.QuestComplete.QuestType = QuestManager.QuestTypeList.Golds;
                                break;
                            default:
                                Debug.LogError("Unkown QuestType:" + element.GetAttribute("QuestType") + "!!!!");
                                break;
                        }
                    }
                    if (element.GetAttribute("Num") != "")
                        _quest.QuestComplete.Num = int.Parse(element.GetAttribute("Num"));
                    if (element.GetAttribute("Parameter") != "")
                    {
                        string[] parameter = element.GetAttribute("Parameter").Split(',');
                        _quest.QuestComplete.Parameter = new int[parameter.Length];
                        for (int j = 0; j < parameter.Length; j++)
                        {
                            _quest.QuestComplete.Parameter[j] = int.Parse(parameter[j]);
                        }
                    }
                }

                if (element.Name == "Award")
                {
                    if (element.GetAttribute("Golds") != "")
                        _quest.Award.Gold = int.Parse(element.GetAttribute("Golds"));
                    if (element.GetAttribute("Exp") != "")
                        _quest.Award.Exp = int.Parse(element.GetAttribute("Exp"));
                    if (element.GetAttribute("Goods") != "")
                    {
                        string[] parameter = element.GetAttribute("Goods").Split(',');
                        _quest.Award.Goods = new int[parameter.Length];
                        for (int j = 0; j < parameter.Length; j++)
                        {
                            _quest.Award.Goods[j] = int.Parse(parameter[j]);
                        }
                    }
                    if (element.GetAttribute("GoodsNum") != "")
                        _quest.Award.GoodsNum = int.Parse(element.GetAttribute("GoodsNum"));
                    if (element.GetAttribute("Event") != "")
                        _quest.Award.Event = int.Parse(element.GetAttribute("Event"));
                    if (element.GetAttribute("TaskPoint") != "")
                        _quest.Award.TaskPoint = int.Parse(element.GetAttribute("TaskPoint"));
                }
            }
            //添加进List中
            List.Add(_quest);
        }
        return List;
    }

    //读取任务类型配置
    public ArrayList loadQuestGroupXmlToArray()
    {
        //保存路径
        string filepath = "Config/Quest/QuestGroup";

        string _result = Resources.Load(filepath).ToString();

        ArrayList List = new ArrayList();

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(_result);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("QuestGroupList").ChildNodes;

        foreach (XmlElement group in nodeList)
        {
            QuestManager.QuestGroupBase _group = new QuestManager.QuestGroupBase();

            //读取node内属性，把string转化为对应的属性
            if (group.GetAttribute("ID") != "")
                _group.ID = int.Parse(group.GetAttribute("ID"));
            if (group.GetAttribute("GroupType") != "")
            {
                switch (group.GetAttribute("GroupType"))
                {
                    case "0":
                        _group.GroupType = QuestManager.GroupTypeList.Main;
                        break;
                    case "1":
                        _group.GroupType = QuestManager.GroupTypeList.Sub;
                        break;
                    case "2":
                        _group.GroupType = QuestManager.GroupTypeList.Secret;
                        break;
                    default:
                        Debug.LogError("Unkown GroupType:" + group.GetAttribute("GroupType") + "!!!!");
                        break;
                }
            }
            if (group.GetAttribute("Name") != "")
                _group.Name = group.GetAttribute("Name");

            //添加进itemList中
            List.Add(_group);
        }
        return List;
    }

    
    //获取路径//
    private static string GetDataPath()
    {
        return Application.dataPath + "/Resources";
    }
}