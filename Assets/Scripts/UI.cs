using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    private Transform UIRoot;

    private Transform MenuP;
    private Transform DIYP;
    private Transform GameP;

    private Transform PauseP;

    private void Awake()
    {
        UIRoot = transform;

        MenuP = UIRoot.Find("Menu_P");
        DIYP = UIRoot.Find("DIY_P");
        GameP = UIRoot.Find("Game_P");

        PauseP = GameP.Find("Pause_P");
    }

    // Use this for initialization
    void Start () {
        InitMenuUI();

        RegistUIMethod();
    }

    private void InitMenuUI ()
    {
        MenuP.gameObject.SetActive(true);
        DIYP.gameObject.SetActive(false);
        GameP.gameObject.SetActive(false);

        PauseP.gameObject.SetActive(false);
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
        Button pauseB = GameP.Find("Pause_B").GetComponent<Button>();
        pauseB.onClick.AddListener(PauseGame);

        Button resumeB = PauseP.Find("Resume_B").GetComponent<Button>();
        resumeB.onClick.AddListener(ResumeGame);
        Button exitB = PauseP.Find("Exit_B").GetComponent<Button>();
        exitB.onClick.AddListener(ReturnToMenu);
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

        GameController.Instance.CreatePlayerAndStartGame();
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

    private void PauseGame ()
    {
        //TODO
        PauseP.gameObject.SetActive(true);
    }

    private void ResumeGame ()
    {
        //TODO
        PauseP.gameObject.SetActive(false);
    }

    private void ReturnToMenu ()
    {
        //TODO
        PauseP.gameObject.SetActive(false);

        GameController.Instance.DestroyPlayer();
        PoolManager.Instance.ClearAllEnemyAndBullet();

        InitMenuUI();
    }
}
