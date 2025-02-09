using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonUpgrade : MonoBehaviour
{

    [Header("References")]
    public TextMeshProUGUI MoneyText;

    [Header("Settings")]

    public float CostInitial = 10;
    public float CostMultipler = 2;

    [Tooltip("Only for the Tollerance Upgrade, Reduces in Percentage the happyness degree")]
    public AnimationCurve TolleranceIncreasePercentage;

    [Min(0)]
    [Tooltip("How Many times can the upgrade be applied? 0 is Unlimited Times. If limit reached, it will turn itshelf off.")]
    public int MaxUpgrades = 0;
    public Color ColorReachedMaxUpgrades = Color.gray;

    public UpgradeType TypeOfUpgrade;
    [Tooltip("This tells how many times/how strong this upgarde get Applied, if you put -1 in the value it will substract it")]
    public int UpgradeTimes = 1;

    public bool DoSupriseGrowthAnimation = false;

    private UpgradeManager manager;
    private BaseGameMode gamemode;

    [ReadOnly]
    public int UpgradedTimes = 0;

    [ReadOnly][SerializeField]
    private float currentCost = 0;

    private Button button;

    public void OnEnable()
    {
        UpdateReferences();
        UpdateVisuals();
    }

    private void Update()
    {
        UpdateVisuals();
    }

    private void UpdateReferences()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (manager == null)
        {
            manager = UpgradeManager.Instance;
        }

        if (gamemode == null)
        {
            gamemode = BaseGameMode.instance;
        }

        CalcCurrentCost();

    }

    public void UpdateVisuals()
    {
        if (MaxUpgrades > 0 && UpgradedTimes >= MaxUpgrades)
        {
            Debug.Log("Upgrade Times Limit Reached");

            button.interactable = false;
            button.enabled = false;
            button.targetGraphic.color = ColorReachedMaxUpgrades;


            if (MoneyText != null)
            {
                MoneyText.text = Statics.ButtonMaxUpgrades;
            }
        }
        else
        {
            if (gamemode == null)
            {
                UpdateReferences();
            }

            if (MoneyText != null)
            {
                MoneyText.text = currentCost.ToString("0.00") + " " + Statics.CurrencySymbol;
            }

            if (currentCost < gamemode.Money)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
    }

    private void CalcCurrentCost()
    {
        if (UpgradedTimes == 0)
        {
            currentCost = CostInitial;
        }
        else
        {
            float additionalCost = CostInitial;
            for (int i = 0; i < (UpgradedTimes); i++)
            {
                additionalCost = additionalCost * CostMultipler;
            }

           currentCost = additionalCost;
        }
    }

    public void OnClick()
    {
        DoUpgrade();
        CalcCurrentCost();
    }

    private void DoUpgrade()
    {
        CalcCurrentCost();

        if (currentCost > gamemode.Money)
        {
            return;
        }

        UpgradedTimes++;

        gamemode.SubMoney(currentCost);


        switch (TypeOfUpgrade)
        {
            case UpgradeType.InitialUpgarde:
                manager.BuyInitialUpgarde();
                break;
            case UpgradeType.Production:
                gamemode.BuyUpgradeProduction(UpgradeTimes);
                break;
            case UpgradeType.MaxSize:
                gamemode.BuyMaxSize(UpgradeTimes);
                break;
            case UpgradeType.Happyness:
                gamemode.BuyHappyness(UpgradeTimes);
                break;
            case UpgradeType.MilkFullness:
                gamemode.BuyMilkFullness(UpgradeTimes, DoSupriseGrowthAnimation);
                break;
            case UpgradeType.TolleranceBeeingFull:
                gamemode.BuyTolerance(UpgradedTimes, TolleranceIncreasePercentage);
                break;
            default:
                break;
        }


    }

    public enum UpgradeType
    {
        InitialUpgarde = 0,
        Production = 1,
        MaxSize = 2,
        Happyness = 3,
        MilkFullness = 4,
        TolleranceBeeingFull = 5,
    }
}


