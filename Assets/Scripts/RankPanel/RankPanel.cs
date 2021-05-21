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
    static int rankSeasonCountDown;
    private int seasonID;
    private int selfRank;

    private string ChangeSecondsToDate(int time)
    {
        /// <summary>
        /// 把秒格式化为DateTime格式
        /// </summary> 
        var timeSpan = TimeSpan.FromSeconds(time);
        string days = string.Format("{0:D2}", timeSpan.Days);
        string hours = string.Format("{0:D2}", timeSpan.Hours);
        string minutes = string.Format("{0:D2}", timeSpan.Minutes);
        string seconds = string.Format("{0:D2}", timeSpan.Seconds);
        string date = days + "d " + hours + "h " + minutes + "m " + seconds + "s";
        return date;
    }

    private void CreateRankTitle(int seasonID)
    {
        /// <summary>
        /// 加载赛季信息并开启倒计时
        /// </summary>
        
        rankSeasonText.text = "Season " + seasonID.ToString() + " Ranking";
        StartCoroutine(CountDown());
    }

    private void LoadMyRank(int selfRank)
    {
        /// <summary>
        /// 显示用户自己的赛季排名信息
        /// </summary>
        myRankText.text = (selfRank).ToString();
        myNameText.text = data[selfRank - 1].NickName;
        myTrophyText.text = data[selfRank - 1].Trophy.ToString();
    }

    private void Start()
    {
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