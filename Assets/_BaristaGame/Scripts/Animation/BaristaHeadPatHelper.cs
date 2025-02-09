using UnityEngine;

public class BaristaHeadPatHelper : MonoBehaviour
{
    [Tooltip("The needed time until the headpat animation starts")]
    public float TimeUntilPatStart = 0.2f;
    [Tooltip("Time Between Possibile Headpats")]
    public float HeadpatCooldown = 1f;

    private BaristaController barsita;
    private BaristaTalkManager talkManager;
    private BaseGameMode gameMode;

    private bool MouseHoldDown = false;
    private float TimeMouseDownHold = 0;
    private float TimeLastHeadpat = 0;
    private bool FailedToPat = false;

    private void Start()
    {
        barsita = BaristaController.instance;
        talkManager = FindObjectOfType<BaristaTalkManager>();
        gameMode = BaseGameMode.instance;
    }

    void OnMouseDown()
    {
        if (Time.timeSinceLevelLoad > (TimeLastHeadpat + HeadpatCooldown) && FailedToPat == false)
        {
            TimeLastHeadpat = Time.timeSinceLevelLoad;
            talkManager.TryBaristaEventPatHead();
            MouseHoldDown = true;
            gameMode.BaristaStartHeadpatting();
        }
        else
        {
            FailedToPat = true;
        }
    }

    private void Update()
    {
        if (MouseHoldDown == true)
        {
            TimeMouseDownHold = TimeMouseDownHold + Time.deltaTime;
        }

        barsita.HeadPatting = (TimeMouseDownHold > TimeUntilPatStart);
    }

    private void OnMouseUp()
    {
        ResetMouse();
    }

    public void ResetMouse()
    {
        FailedToPat = false;
        MouseHoldDown = false;
        TimeMouseDownHold = 0;
        gameMode.BaristaStoptHeadpatting();
    }
}
