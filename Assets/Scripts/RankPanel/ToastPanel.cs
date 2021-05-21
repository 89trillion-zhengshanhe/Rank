using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastPanel : MonoBehaviour
{
    [SerializeField] private Text userDataText;

    public void ShowUserData(string user, int rank)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        userDataText.text = String.Format("User: {0}\nRank: {1}", user, rank); 
    }

    public void CloseToastPanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
