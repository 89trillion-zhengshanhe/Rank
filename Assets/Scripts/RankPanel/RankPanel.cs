using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    [SerializeField] private GameObject rankPanel;
    [SerializeField] private ToastPanel toastPanel;
    [SerializeField] private GameObject startButton;

    [SerializeField] private RecyclingListView theList;

    // 自己的Rank信息
    [SerializeField] private Text myRankText;
    [SerializeField] private Text myNameText;
    [SerializeField] private Text myTrophyText;

    private List<RankItem.RankChildData> data = new List<RankItem.RankChildData>();

    // RankPanel内的Title信息
    [SerializeField] private Text rankSeasonText;
    [SerializeField] private Text countDownText;
    private LoadJson loadJson = new LoadJson(); 
    
    public static Sprite[] rankListImage;
    public static Sprite[] rankIconImageList;
    public static Sprite[] userRankImageList = new Sprite[14];
    public static Sprite[] rankAvatarImageList;

    static int rankSeasonCountDown;
    private int seasonID;
    private int selfRank;

    static public void LoadResources()
    {
        rankListImage = Resources.LoadAll<Sprite>("Image/Rank_List");
        rankIconImageList = Resources.LoadAll<Sprite>("Image/icon_rank");
        for (int i = 0; i < 14; i++)
        {
            userRankImageList[i] = Resources.Load<Sprite>("Image/Rank/arenaBadge_" + (i + 1));
        }

        rankAvatarImageList = Resources.LoadAll<Sprite>("Image/avatar");
    }

    /// <summary>
    /// 把秒格式化为DateTime格式
    /// </summary> 
    private string ChangeSecondsToDate(int time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);
        string days = string.Format("{0:D2}", timeSpan.Days);
        string hours = string.Format("{0:D2}", timeSpan.Hours);
        string minutes = string.Format("{0:D2}", timeSpan.Minutes);
        string seconds = string.Format("{0:D2}", timeSpan.Seconds);
        string date = days + "d " + hours + "h " + minutes + "m " + seconds + "s";
        return date;
    }

    /// <summary>
    /// 加载赛季信息并开启倒计时
    /// </summary>
    private void CreateRankTitle(int seasonID)
    {
        rankSeasonText.text = "Season " + seasonID.ToString() + " Ranking";
        StartCoroutine(CountDown());
    }

    /// <summary>
    /// 显示用户自己的赛季排名信息
    /// </summary>
    private void LoadMyRank(int selfRank)
    {
        myRankText.text = (selfRank).ToString();
        myNameText.text = data[selfRank - 1].NickName;
        myTrophyText.text = data[selfRank - 1].Trophy.ToString();
    }

    private void Start()
    {
        LoadResources();
        data = loadJson.LoadJsonToRankItem();
        rankSeasonCountDown = loadJson.GetRankSeasonCountDown();
        seasonID = loadJson.GetSeasonID();
        CreateRankTitle(seasonID);
        selfRank = loadJson.GetSelfRank();
        LoadMyRank(selfRank);
        theList.ItemCallback = PopulateItem;
        theList.RowCount = data.Count;
    }

    private void PopulateItem(RecyclingListViewItem item, int rowIndex)
    {
        var child = item as RankItem;
        Vector3 childPos = child.gameObject.transform.position;
        child.gameObject.transform.position = new Vector3(0, childPos.y, childPos.z);
        child.RankData = data[rowIndex];
    }

    public void ShowRank()
    {
        startButton.SetActive(false);
        rankPanel.SetActive(true);
        StartCoroutine(CountDown());
    }

    public void CloseRank()
    {
        rankPanel.SetActive(false);
        startButton.SetActive(true);
    }

    public void ShowToast(string name, int rank)
    {
        toastPanel.ShowUserData(name, rank);
    }

    IEnumerator CountDown()
    {
        while (rankSeasonCountDown > 0)
        {
            countDownText.text = "Ends in: " + ChangeSecondsToDate(rankSeasonCountDown);
            yield return new WaitForSeconds(1);
            rankSeasonCountDown--;
        }
    }
}