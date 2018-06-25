using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    private Transform UIRoot;

    private Transform MenuP;
    private Transform DIYP;
    private Transform GameP;

    private void Awake()
    {
        UIRoot = transform;

        MenuP = UIRoot.Find("Menu_P");
        DIYP = UIRoot.Find("DIY_P");
        GameP = UIRoot.Find("Game_P");
    }

    // Use this for initialization
    void Start () {
        InitUI();

        RegistUIMethod();
    }

    private void InitUI ()
    {
        MenuP.gameObject.SetActive(true);
        DIYP.gameObject.SetActive(false);
        GameP.gameObject.SetActive(false);
    }

    private void RegistUIMethod ()
    {
        //MENU
        Button startB = MenuP.Find("Start_B").GetComponent<Button>();
        startB.onClick.AddListener(StartGame);
        Button diyB = MenuP.Find("DIY_B").GetComponent<Button>();
        diyB.onClick.AddListener(ShowDIYP);
        //DIY

        //GAME
    }

    private void RemoveUIMethod ()
    {
        //TODO
    }
	
    /// <summary>
    /// 开始游戏
    /// </summary>
	private void StartGame ()
    {
        MenuP.gameObject.SetActive(false);
        DIYP.gameObject.SetActive(false);
        GameP.gameObject.SetActive(true);
    }

    /// <summary>
    /// 展示DIY面板(刷新信息)
    /// </summary>
    private void ShowDIYP ()
    {
        MenuP.gameObject.SetActive(false);
        DIYP.gameObject.SetActive(true);
        GameP.gameObject.SetActive(false);

        //TODO
    }
}
