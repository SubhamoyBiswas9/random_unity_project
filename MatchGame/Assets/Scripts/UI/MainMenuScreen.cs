using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] Button playBtn;
    [SerializeField] Button quitBtn;

    public event Action OnPlayBtnClicked;

    private void OnEnable()
    {
        playBtn.onClick.AddListener(OnClickPlayBtn);
        quitBtn.onClick.AddListener(()=>Application.Quit());
    }

    private void OnDisable()
    {
        playBtn.onClick.RemoveListener(OnClickPlayBtn);
        quitBtn.onClick.RemoveAllListeners();
    }

    public void OnClickPlayBtn()
    {
        OnPlayBtnClicked?.Invoke();
        gameObject.SetActive(false);
    }
}
