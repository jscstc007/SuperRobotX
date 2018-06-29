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

public class UIController : MonoBehaviour
{

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
    void Start()
    {
        InitMenuUI();
    }

    private void InitMenuUI()
    {
        MenuP.gameObject.SetActive(true);
        DIYP.gameObject.SetActive(false);
        GameP.gameObject.SetActive(false);

        PauseP.gameObject.SetActive(false);
        GameOverP.gameObject.SetActive(false);
    }

    private void RegistUIMethod()
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
        equipWeaponB.onClick.AddListener(() => { ShowEquipList(EquipUIType.Weapon); });
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
        EventManager.Instance.RegistEvent(EventType.PlayerCoinChange, OnUpdateUI_Coin);
    }

    private void RemoveUIMethod()
    {
        //TODO
    }

    private void RemoveEvent()
    {
        EventManager.Instance.RemoveEvent(EventType.PlayerHPChange, OnUpdateUI_HP);
        EventManager.Instance.RemoveEvent(EventType.PlayerScoreChange, OnUpdateUI_Score);
        EventManager.Instance.RemoveEvent(EventType.PlayerCoinChange, OnUpdateUI_Coin);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void StartGame()
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
    private void ShowDIYP()
    {
        MenuP.gameObject.SetActive(false);
        DIYP.gameObject.SetActive(true);
        GameP.gameObject.SetActive(false);

        EventManager.Instance.ExcuteEvent(EventType.PlayerCoinChange, null);
        UpdateRobotUI();
        ShowEquipList(equipUIType);
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    private void PauseGame()
    {
        PauseP.gameObject.SetActive(true);

        GameController.IsPause = true;
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    private void ResumeGame()
    {
        PauseP.gameObject.SetActive(false);

        GameController.IsPause = false;
    }

    /// <summary>
    /// 展示游戏结束UI
    /// </summary>
    public void ShowGameOverUI()
    {
        GameOverP.gameObject.SetActive(true);
    }

    /// <summary>
    /// 返回主界面
    /// </summary>
    private void ReturnToMenu()
    {
        InitMenuUI();

        if (GameController.IsGame)
        {
            GameController.Instance.StopGameAndReturnToMenu();
        }
    }

    /// <summary>
    /// 更新金币信息
    /// </summary>
    private void OnUpdateUI_Coin(params int[] data)
    {
        Text coinT = DIYP.Find("Coin_T").GetComponent<Text>();
        coinT.text = string.Format("金币:{0}", RobotInfo.Instance.Coin);
    }

    /// <summary>
    /// 更新HP信息
    /// </summary>
    private void OnUpdateUI_HP(params int[] data)
    {
        hpI.fillAmount = (float)RobotInfo.Instance.HP / (float)RobotInfo.Instance.MainRobotInfo.maxHP;
        hpT.text = string.Format("{0}/{1}", RobotInfo.Instance.HP, RobotInfo.Instance.MainRobotInfo.maxHP);
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
    private void ChooseLeftRobot()
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
    /// 升级 机器人
    /// </summary>
    private void DoUpRobot()
    {
        BaseRobotInfo robotInfo = RobotInfo.Instance.MainRobotInfo;

        bool isEnoughMoney = RobotInfo.Instance.Coin >= robotInfo.cost;

        if (isEnoughMoney)
        {
            RobotInfo.Instance.Coin -= robotInfo.cost;

            RobotType robotType = robotInfo.robotType;
            int newLevel = robotInfo.robotLevel + 1;

            Debug.Log("DoUpRobot:" + newLevel);

            //更新robot数据
            RobotInfo.Instance.MainRobotInfo = RobotInfo.Instance.LoadBaseRobotData(robotType, newLevel);

            UpdateRobotUI();
        }
        else
        {
            //TODO
        }
    }

    /// <summary>
    /// 根据当前机器人和等级更新UI显示信息
    /// </summary>
    private void UpdateRobotUI()
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
        Button upgradeB = DIYP.Find("Robot_P/Upgrade_B").GetComponent<Button>();
        Text costT = DIYP.Find("Robot_P/Upgrade_B/Value_T").GetComponent<Text>();

        bool isMaxLevel = RobotInfo.Instance.MainRobotInfo.robotLevel == RobotInfo.Instance.MainRobotInfo.robotMaxLevel - 1;

        headI.sprite = ResourcesLoadManager.LoadRobotResources<GameObject>(RobotInfo.Instance.MainRobotInfo.robotResName).GetComponent<SpriteRenderer>().sprite;
        nameT.text = RobotInfo.Instance.MainRobotInfo.robotName;
        weaponT.text = string.Format("武器节点:{0}", RobotInfo.Instance.MainRobotInfo.weaponPoints);
        defenseT.text = string.Format("防御节点:{0}", RobotInfo.Instance.MainRobotInfo.defensePoints);
        speedT.text = string.Format("引擎节点:{0}", RobotInfo.Instance.MainRobotInfo.movePoints);
        otherT.text = string.Format("特殊节点:{0}", RobotInfo.Instance.MainRobotInfo.otherPoints);
        infoT.text = string.Format("{0}", RobotInfo.Instance.MainRobotInfo.robotInfo);
        hpT.text = string.Format("生命:{0}", RobotInfo.Instance.MainRobotInfo.maxHP);
        moveSpeedT.text = string.Format("速度:{0}", RobotInfo.Instance.MainRobotInfo.moveSpeed);

        upgradeB.interactable = !isMaxLevel;
        costT.text = isMaxLevel ? "Max" : string.Format("升级:{0}", RobotInfo.Instance.MainRobotInfo.cost);
    }

    /// <summary>
    /// 刷新当前Equip信息
    /// </summary>
    private void ShowEquipList(EquipUIType type)
    {
        equipUIType = type;

        UpdateEquipList();
    }

    /// <summary>
    /// 刷新当前Equip信息
    /// </summary>
    private void UpdateEquipList()
    {
        //获取原型并隐藏
        equipItem.gameObject.SetActive(false);
        //更新列表
        for (int i = 0; i < equipScrollP.childCount; i++)
        {
            Destroy(equipScrollP.GetChild(i).gameObject);
        }

        switch (equipUIType)
        {
            case EquipUIType.Weapon:

                WeaponType[] types = WeaponInfo.GetAllWeapons();
                int count = types.Length;

                equipScrollP.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Mathf.Max(900, 200 * count));
                equipScrollP.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                for (int i = 0; i < count; i++)
                {
                    GameObject go = Instantiate<GameObject>(equipItem.gameObject);
                    Transform trans = go.transform;
                    go.SetActive(true);
                    go.name = i.ToString();
                    trans.SetParent(equipScrollP);
                    trans.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200 * i);
                    trans.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 200);
                    trans.GetComponent<RectTransform>().localScale = Vector3.one;

                    WeaponType tempType = types[i];
                    int tempLevel = DataManager.Instance.LoadWeaponLevel(tempType);

                    BaseWeaponInfo baseWeaponInfo = WeaponInfo.Instance.LoadBaseWeaponData(tempType, tempLevel);
                    bool isMaxLevel = tempLevel >= WeaponInfo.MAX_WEAPON_LEVEL;

                    trans.Find("I").GetComponent<Image>().sprite = ResourcesLoadManager.LoadWeaponResources<GameObject>(baseWeaponInfo.weaponResName).GetComponent<SpriteRenderer>().sprite;
                    trans.Find("Cost_T").GetComponent<Text>().text = string.Format("节点消耗:{0}", baseWeaponInfo.weaponPointsCost);
                    trans.Find("Power_T").GetComponent<Text>().text = string.Format("武器威力:{0}", baseWeaponInfo.shootPower + tempLevel * baseWeaponInfo.shootPowerUpPerLevel);
                    trans.Find("Speed_T").GetComponent<Text>().text = string.Format("武器射速:{0}", baseWeaponInfo.shootSpeed + tempLevel * baseWeaponInfo.shootSpeedUpPerLevel);

                    Button upgradeB = trans.Find("Upgrade_B").GetComponent<Button>();
                    Button equipB = trans.Find("Equip_B").GetComponent<Button>();

                    upgradeB.interactable = !isMaxLevel;
                    trans.Find("Upgrade_B/Value_T").GetComponent<Text>().text = isMaxLevel ? "Max" : string.Format("升级:{0}", baseWeaponInfo.weaponUpgradeCost + tempLevel * baseWeaponInfo.weaponUpgradeCostUp);

                    //注册按钮方法
                    upgradeB.onClick.AddListener(() => { DoUpgradeModule(tempType); });
                    equipB.onClick.AddListener(() => { DoEquipOrUnequipModule(tempType); });
                }

                break;

            case EquipUIType.Defense:
                break;
            case EquipUIType.Speed:
                break;
            case EquipUIType.Other:
                break;
        }
    }
    /// <summary>
    /// 升级 模块
    /// </summary>
    private void DoUpgradeModule(WeaponType type)
    {
        //TODO
    }
    /// <summary>
    /// 穿上/卸下 模块
    /// </summary>
    private void DoEquipOrUnequipModule(WeaponType type)
    {
        //TODO
    }

    /// <summary>
    /// 展示商店页面
    /// </summary>
    private void DoShop()
    {
        //TODO
    }
}
