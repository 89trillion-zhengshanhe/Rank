using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;


public class ItemData
{
    public string uid;
    public string nickname;
    public int trophy;
    public int rank;
}


public class LoadJson
{
    private string path;
    private string jsonString;
    private int rankSeasonCountDown;
    private int seasonID;
    private int selfRank;
    private List<ItemData> itemList = new List<ItemData>();

    public List<RankItem.RankChildData> LoadJsonToRankItem()
    {
        List<RankItem.RankChildData> listRank = new List<RankItem.RankChildData>();
        path = Application.dataPath + "/Resources/JsonData/ranklist.json";
        jsonString = File.ReadAllText(path);
        JSONNode data = JSON.Parse(jsonString);

        // 转换Json数据为ItemData类型并存储在itemList内
        foreach (JSONNode record in data["list"])
        {
            ItemData itemData = new ItemData();
            itemData.uid = record["uid"];
            itemData.nickname = record["nickName"];
            itemData.trophy = record["trophy"];
            itemList.Add(itemData);
        }

        // 根据奖杯数量对itemlist内的数据进行降序排序
        itemList.Sort((y, x) => x.trophy.CompareTo(y.trophy));

        // 更新rank信息
        for (int index = 0; index < itemList.Count; index++)
        {
            itemList[index].rank = index + 1;
            listRank.Add(new RankItem.RankChildData(itemList[index]));
        }

        rankSeasonCountDown = data["countDown"].AsInt;
        seasonID = data["seasonID"].AsInt;
        selfRank = data["selfRank"].AsInt;
        return listRank;
    }

    public int GetRankSeasonCountDown()
    {
        return rankSeasonCountDown;
    }

    public int GetSeasonID()
    {
        return seasonID;
    }

    public int GetSelfRank()
    {
        return selfRank;
    }
}