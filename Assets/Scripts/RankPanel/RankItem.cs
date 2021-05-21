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
    
    [SerializeField] private List<Sprite> rankListImage = new List<Sprite>();
    [SerializeField] private List<Sprite> rankIconImageList = new List<Sprite>();
    [SerializeField] private List<Sprite> userRankImageList = new List<Sprite>();
    [SerializeField] private List<Sprite> rankAvatarImageList = new List<Sprite>();

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

    private void LoadAllImage()
    {
        for (int amount = 1; amount <= 4; amount++)
        {
            rankListImage.Add(rankBackGround.sprite = Resources.Load<Sprite>("/Image/rank list_" + amount));
        }
        for (int amount = 1; amount <= 4; amount++)
        {
            rankIconImageList.Add(rankBackGround.sprite = Resources.Load<Sprite>(Application.dataPath + "/Resources/Image/icon_rank_" + amount));
        }
        for (int amount = 1; amount <= 14; amount++)
        {
            userRankImageList.Add(rankBackGround.sprite = Resources.Load<Sprite>(Application.dataPath + "/Resources/Image/Rank/arenaBadge_" + amount));
        }
        for (int amount = 1; amount <= 4; amount++)
        {
            rankAvatarImageList.Add(rankBackGround.sprite = Resources.Load<Sprite>(Application.dataPath + "/Resources/Image/avatar_" + amount));
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
                rankBackGround.sprite = rankListImage[rankData.Rank - 1];
                rankImage.sprite = rankIconImageList[rankData.Rank - 1];
                rankAvatarImage.sprite = rankAvatarImageList[rankData.Rank - 1];
            }
            else
            {
                rankBackGround.sprite = rankListImage[3];
                rankImage.sprite = rankIconImageList[3];
                rankAvatarImage.sprite = rankAvatarImageList[3];
            }
            rankText.text = rankData.Rank.ToString();
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