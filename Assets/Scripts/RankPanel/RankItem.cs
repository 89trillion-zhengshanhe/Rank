using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankItem : RecyclingListViewItem
{
    // RankListScrollView内的Rank信息
    [SerializeField] private Image rankBackGround;
    [SerializeField] private Image rankImage;
    [SerializeField] private Text rankText;
    [SerializeField] private Image rankIcon;
    [SerializeField] private Image rankAvatarImage;
    [SerializeField] private Text userNameText;
    [SerializeField] private Text trophyText;

    [SerializeField] private RankPanel rankPanel;
    static public Sprite[] rankListImage;
    static public Sprite[] rankIconImageList;
    static public Sprite[] userRankImageList = new Sprite[14];
    static public Sprite[] rankAvatarImageList;

    static public void Load()
    {
        rankListImage = Resources.LoadAll<Sprite>("Image/Rank_List");
        rankIconImageList = Resources.LoadAll<Sprite>("Image/icon_rank");
        for (int i = 0; i < 14; i++)
        {
            userRankImageList[i] = Resources.Load<Sprite>("Image/Rank/arenaBadge_" + (i + 1));
        }
        rankAvatarImageList = Resources.LoadAll<Sprite>("Image/avatar");
    }
    public struct RankChildData
    {
        public string UserID;
        public string NickName;
        public int Trophy;
        public int Rank;

        public RankChildData(ItemData data)
        {
            UserID = data.uid;
            NickName = data.nickname;
            Trophy = data.trophy;
            Rank = data.rank;
        }
    }

    private RankChildData rankData;

    public RankChildData RankData
    {
        get { return rankData; }
        set
        {
            rankData = value;
            // 设置排名和相应的背景图片
            if (rankData.Rank < 4)
            {
                rankImage.gameObject.SetActive(true);
                rankText.gameObject.SetActive(false);
                rankBackGround.sprite = rankListImage[rankData.Rank - 1];
                rankImage.sprite = rankIconImageList[rankData.Rank - 1];
                rankAvatarImage.sprite = rankAvatarImageList[rankData.Rank - 1];
                rankImage.SetNativeSize();
            }
            else
            {
                rankBackGround.sprite = rankListImage[3];
                // rankImage.sprite = rankIconImageList[3];
                rankImage.gameObject.SetActive(false);
                rankText.gameObject.SetActive(true);
                rankAvatarImage.sprite = rankAvatarImageList[3];
                rankText.text = rankData.Rank.ToString();
            }
            
            // 设置用户昵称
            userNameText.text = rankData.NickName;
            // 根据奖杯计算段位
            int rankNum = rankData.Trophy / 1000 + 1;
            rankIcon.sprite = userRankImageList[rankNum - 1];
            // 显示相应奖杯数
            trophyText.text = rankData.Trophy.ToString();
            
            // 保留所有sprite的长宽比
            rankBackGround.preserveAspect = true;
            rankImage.preserveAspect = true;
            rankAvatarImage.preserveAspect = true;
            rankIcon.preserveAspect = true;
        }
    }

    public void SetRankDataOnClicked()
    {
        rankPanel.ShowToast(rankData.NickName, rankData.Rank);
    }
}