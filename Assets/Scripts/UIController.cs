using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipUIType
{
    Weapon,
    Defense,
    Speed,
    Other,
}

public class UIController : MonoBehaviour {

    private Transform UIRoot;

    private Transform MenuP;
    private Transform DIYP;
    private Transform GameP;

    private Transform equipScrollP;
    private Transform equipItem;

    private Image hpI;
    private Text hpT;
    private Text scoreT;
    private Transform PauseP;
    private Transform GameOverP;

    /// <summary>
    /// 当前DIY Equip界面类别
    /// </summary>
    private EquipUIType equipUIType = EquipUIType.Weapon;

    private void Awake()
    {
        UIRoot = transform;

        MenuP = UIRoot.Find("Menu_P");
        DIYP = UIRoot.Find("DIY_P");
        GameP = UIRoot.Find("Game_P");

        equipScrollP = DIYP.Find("Equip_P/Mask_P/Scroll_P");
        equipItem = DIYP.Find("Equip_P/Mask_P/Item");

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
        Button leftRobotB = DIYP.Find("Robot_P/Left_B").GetComponent<Button>();
        leftRobotB.onClick.AddListener(ChooseLeftRobot);
        Button rightRobotB = DIYP.Find("Robot_P/Right_B").GetComponent<Button>();
        rightRobotB.onClick.AddListener(ChooseRightRobot);
        Button upRobotB = DIYP.Find("Robot_P/Upgrade_B").GetComponent<Button>();
        upRobotB.onClick.AddListener(DoUpRobot);

        Button equipWeaponB = DIYP.Find("Equip_P/Tag_P/0_B").GetComponent<Button>();
        equipWeaponB.onClick.AddListener(()=> { ShowEquipList(EquipUIType.Weapon); });
        Button equipDefenseB = DIYP.Find("Equip_P/Tag_P/1_B").GetComponent<Button>();
        equipDefenseB.onClick.AddListener(() => { ShowEquipList(EquipUIType.Defense); });
        Button equipSpeedB = DIYP.Find("Equip_P/Tag_P/2_B").GetComponent<Button>();
        equipSpeedB.onClick.AddListener(() => { ShowEquipList(EquipUIType.Speed); });
        Button equipOtherB = DIYP.Find("Equip_P/Tag_P/3_B").GetComponent<Button>();
        equipOtherB.onClick.AddListener(() => { ShowEquipList(EquipUIType.Other); });

        Button diyReturnB = DIYP.Find("Setting_P/Return_B").GetComponent<Button>();
        diyReturnB.onClick.AddListener(ReturnToMenu);
        Button diyStartB = DIYP.Find("Setting_P/Start_B").GetComponent<Button>();
        diyStartB.onClick.AddListener(StartGame);
        Button diyShopB = DIYP.Find("Setting_P/Shop_B").GetComponent<Button>();
        diyShopB.onClick.AddListener(DoShop);
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

        UpdateRobotUI();
        ShowEquipList(equipUIType);
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
    /// 返回主界面
    /// </summary>
    private void ReturnToMenu ()
    {
        InitMenuUI();

        if (GameController.IsGame)
        {
            GameController.Instance.StopGameAndReturnToMenu();
        }
    }

    /// <summary>
    /// 更新HP信息
    /// </summary>
    private void OnUpdateUI_HP (params int[] data)
    {
        hpI.fillAmount = (float)RobotInfo.Instance.HP / (float)RobotInfo.Instance.robotInfo.maxHP;
        hpT.text = string.Format("{0}/{1}", RobotInfo.Instance.HP, RobotInfo.Instance.robotInfo.maxHP);
    }
    /// <summary>
    /// 更新分数信息
    /// </summary>
    private void OnUpdateUI_Score(params int[] data)
    {
        scoreT.text = string.Format("得分:{0}", RobotInfo.Instance.Score);
    }

    /// <summary>
    /// 选择前一个机器人
    /// </summary>
    private void ChooseLeftRobot ()
    {
        //TODO
    }

    /// <summary>
    /// 选择后一个机器人
    /// </summary>
    private void ChooseRightRobot()
    {
        //TODO
    }

    /// <summary>
    /// 解锁/升级 机器人
    /// </summary>
    private void DoUpRobot ()
    {
        //TODO
    }

    /// <summary>
    /// 根据当前机器人和等级更新UI显示信息
    /// </summary>
    private void UpdateRobotUI ()
    {
        Image headI = DIYP.Find("Robot_P/I").GetComponent<Image>();
        Text nameT = DIYP.Find("Robot_P/Name_T").GetComponent<Text>();
        Text weaponT = DIYP.Find("Robot_P/Weapon_T").GetComponent<Text>();
        Text defenseT = DIYP.Find("Robot_P/Defense_T").GetComponent<Text>();
        Text speedT = DIYP.Find("Robot_P/Speed_T").GetComponent<Text>();
        Text otherT = DIYP.Find("Robot_P/Other_T").GetComponent<Text>();
        Text infoT = DIYP.Find("Robot_P/Info_T").GetComponent<Text>();
        Text hpT = DIYP.Find("Robot_P/HP_T").GetComponent<Text>();
        Text moveSpeedT = DIYP.Find("Robot_P/MoveSpeed_T").GetComponent<Text>();

        headI.sprite = ResourcesLoadManager.LoadRobotResources<GameObject>(RobotInfo.Instance.robotInfo.robotResName).GetComponent<SpriteRenderer>().sprite;
        nameT.text = RobotInfo.Instance.robotInfo.robotName;
        weaponT.text = string.Format("武器节点:{0}", RobotInfo.Instance.robotInfo.weaponPoints);
        defenseT.text = string.Format("防御节点:{0}", RobotInfo.Instance.robotInfo.defensePoints);
        speedT.text = string.Format("引擎节点:{0}", RobotInfo.Instance.robotInfo.movePoints);
        otherT.text = string.Format("特殊节点:{0}", RobotInfo.Instance.robotInfo.otherPoints);
        infoT.text = string.Format("{0}", RobotInfo.Instance.robotInfo.robotInfo);
        hpT.text = string.Format("生命:{0}", RobotInfo.Instance.robotInfo.maxHP);
        moveSpeedT.text = string.Format("速度:{0}", RobotInfo.Instance.robotInfo.moveSpeed);

        //TODO
    }

    /// <summary>
    /// 刷新当前Equip信息
    /// </summary>
    private void ShowEquipList (EquipUIType type)
    {
        equipUIType = type;

        UpdateEquipList();
    }

    /// <summary>
    /// 刷新当前Equip信息
    /// </summary>
    private void UpdateEquipList ()
    {
        //获取原型并隐藏
        equipItem.gameObject.SetActive(false);
        //更新列表
        for (int i = 0; i < equipScrollP.childCount; i++)
        {
            Destroy(equipScrollP.GetChild(i).gameObject);
        }

        //TODO

        //int count = PetInfo.Instance.PetMap.Count;
        //equipScrollP.rectTransform().sizeDelta = new Vector2(0, Mathf.Max(400, 80 * count));
        //equipScrollP.rectTransform().anchoredPosition = Vector2.zero;
        ////如果选择单位还在 则不会重置选择单位 否则保持原位
        //if (count == 0)
        //{
        //    SelectPetId = 0;
        //}
        //else
        //{
        //    //如果原来位置的宠物不存在 则更新选择第一个
        //    if (PetInfo.Instance.GetPetById(SelectPetId) == null)
        //    {
        //        SelectPetId = PetInfo.Instance.GetPetByIndex(0).PetId;
        //    }
        //}
        //for (int i = 0; i < count; i++)
        //{
        //    GameObject go = Instantiate<GameObject>(item.gameObject);
        //    Transform trans = go.transform;
        //    go.SetActive(true);
        //    go.name = i.ToString();
        //    trans.SetParent(equipScrollP);
        //    trans.rectTransform().anchoredPosition = new Vector2(0, -80 * i);
        //    trans.rectTransform().sizeDelta = new Vector2(0, 80);
        //    trans.rectTransform().localScale = Vector3.one;

        //    BasicPetInfo BasicPI = PetInfo.Instance.GetPetByIndex(i);
        //    int index = i;
        //    BasicPI.PetIndex = index;

        //    trans.Find("BG_I/Value_I").GetComponent<Image>().sprite = RoleManager.LoadRoleUI(CreatureType.PET, BasicPI.PetHeadId.ToString());
        //    trans.Find("Level_T").GetComponent<Text>().text = "Lv " + BasicPI.PetLevel;
        //    trans.Find("Name_T").GetComponent<Text>().text = BasicPI.PetName;
        //    Transform qualityP = go.transform.Find("Quality_P");
        //    for (int j = 0; j < PetInfo.MAX_PET_STARNUM; j++)
        //    {//目前最大5星
        //        qualityP.Find(j + "_I").gameObject.SetActive(j < BasicPI.PetStarNum);
        //    }
        //    qualityP.Find("Value_T").GetComponent<Text>().text = BasicPI.PetQualityStr;
        //    bool tempIsStandard = (BasicPI.PetStatusType == BasicPetInfo.PetStatus.PET_STANDARD && !BasicPI.PetIsGuard);
        //    trans.Find("StatusType_I").gameObject.SetActive(!tempIsStandard);
        //    trans.Find("StatusType_I/Value_T").GetComponent<Text>().text = BasicPI.PetStatusTypeStr;

        //    if (BasicPI.PetIsGuard && BasicPI.PetStatusTypeStr != "守")
        //    {
        //        trans.Find("StatusType_2_I").gameObject.SetActive(true);
        //        trans.Find("StatusType_2_I/Value_T").GetComponent<Text>().text = "守";
        //    }
        //    else
        //    {
        //        trans.Find("StatusType_2_I").gameObject.SetActive(false);
        //    }
        //    //设定选中效果
        //    SetLeftSelectPetHighLight(trans, BasicPI.PetId == SelectPetId);
        //    //注册按钮方法
        //    Button btn = go.GetComponent<Button>();
        //    btn.onClick.AddListener(() => { SetLeftSelectPet(index); });
        //}
    }

    /// <summary>
    /// 展示商店页面
    /// </summary>
    private void DoShop ()
    {
        //TODO
    }
}
