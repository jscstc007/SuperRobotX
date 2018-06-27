using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private Transform UIRoot;

    private Transform MenuP;
    private Transform DIYP;
    private Transform GameP;

    private Image hpI;
    private Text hpT;
    private Text scoreT;
    private Transform PauseP;
    private Transform GameOverP;

    private void Awake()
    {
        UIRoot = transform;

        MenuP = UIRoot.Find("Menu_P");
        DIYP = UIRoot.Find("DIY_P");
        GameP = UIRoot.Find("Game_P");

        hpI = GameP.Find("HP_I/Value_I").GetComponent<Image>();
        hpT = GameP.Find("HP_I/Value_T").GetComponent<Text>();
        scoreT = GameP.Find("Score_T").GetComponent<Text>();
        PauseP = GameP.Find("Pause_P");
        GameOverP = GameP.Find("GameOver_P");
    }

    // Use this for initialization
    void Start () {
        InitMenuUI();
    }

    private void InitMenuUI ()
    {
        MenuP.gameObject.SetActive(true);
        DIYP.gameObject.SetActive(false);
        GameP.gameObject.SetActive(false);

        PauseP.gameObject.SetActive(false);
        GameOverP.gameObject.SetActive(false);
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
        Button gameoverB = GameOverP.Find("Exit_B").GetComponent<Button>();
        gameoverB.onClick.AddListener(ReturnToMenu);
    }

    private void OnEnable()
    {
        RegistUIMethod();
        RegistEvent();
    }

    private void OnDisable()
    {
        RemoveUIMethod();
        RemoveEvent();
    }

    private void RegistEvent()
    {
        EventManager.Instance.RegistEvent(EventType.PlayerHPChange, OnUpdateUI_HP);
        EventManager.Instance.RegistEvent(EventType.PlayerScoreChange, OnUpdateUI_Score);
    }

    private void RemoveUIMethod ()
    {
        //TODO
    }

    private void RemoveEvent()
    {
        EventManager.Instance.RemoveEvent(EventType.PlayerHPChange, OnUpdateUI_HP);
        EventManager.Instance.RemoveEvent(EventType.PlayerScoreChange, OnUpdateUI_Score);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void StartGame ()
    {
        MenuP.gameObject.SetActive(false);
        DIYP.gameObject.SetActive(false);
        GameP.gameObject.SetActive(true);

        PauseP.gameObject.SetActive(false);
        GameOverP.gameObject.SetActive(false);

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

    /// <summary>
    /// 暂停游戏
    /// </summary>
    private void PauseGame ()
    {
        PauseP.gameObject.SetActive(true);

        GameController.IsPause = true;
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    private void ResumeGame ()
    {
        PauseP.gameObject.SetActive(false);

        GameController.IsPause = false;
    }

    /// <summary>
    /// 展示游戏结束UI
    /// </summary>
    public void ShowGameOverUI ()
    {
        GameOverP.gameObject.SetActive(true);
    }

    /// <summary>
    /// 退出游戏 返回主界面
    /// </summary>
    private void ReturnToMenu ()
    {
        InitMenuUI();

        GameController.Instance.StopGameAndReturnToMenu();
    }

    /// <summary>
    /// 更新HP信息
    /// </summary>
    private void OnUpdateUI_HP (params int[] data)
    {
        hpI.fillAmount = (float)RobotInfo.Instance.HP / (float)RobotInfo.Instance.MaxHP;
        hpT.text = string.Format("{0}/{1}", RobotInfo.Instance.HP, RobotInfo.Instance.MaxHP);
    }
    /// <summary>
    /// 更新分数信息
    /// </summary>
    private void OnUpdateUI_Score(params int[] data)
    {
        scoreT.text = string.Format("得分:{0}", RobotInfo.Instance.Score);
    }
}
