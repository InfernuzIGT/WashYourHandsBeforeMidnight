using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MinigamesManager : MonoBehaviour
{
    public Image BarMaskMelee;
    public GameObject BarGOMelee;
    public GameObject LightOn;
    private Image BarMask;
    private GameObject BarGO;
    private float barChangeSpeed = 4.5f;
    private float maxBarValue = 100;
    private float currentBarValue;
    private bool barIsIncreasing;
    private bool BarON;
    private bool isTiming;
    private bool isLightOn;
    private bool isTilt;
    private bool isTilting;
    public float tiltTimer;
    public float tiltTimer_threshold = 1f;
    private bool isHold;
    private bool isHolding;

    // UPDATE TO INPUT MANAGER
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isTiming = true;

            CheckBar();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isLightOn = true;

            CheckBar();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isTilt = true;

        }

        if (isTilt)
        {
            if (isTilting)
            {
                tiltTimer += Time.deltaTime;

                if (tiltTimer >= tiltTimer_threshold)
                {
                    tiltTimer = 0;
                    isTilting = false;
                }

                if (Input.GetKeyDown(KeyCode.U))
                {
                    isTilting = true;

                    InitBar();
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            isHold = true;

        }

        if (isHold)
        {

            if (Input.GetKeyDown(KeyCode.V))
            {
                isHolding = true;
                Debug.Log("pressing");

                InitBar();
            }

            if (Input.GetKeyUp(KeyCode.V))
            {
                BarON = false;

                Debug.Log("releasing");
                Debug.Log(currentBarValue);

                StartCoroutine(StopBar());
            }

        }
    }

    private void CheckBar()
    {
        if (BarON)
        {
            StartCoroutine(StopBar());
            return;
        }

        else
        {
            InitBar();
        }
    }

    private void InitBar()
    {

        BarGO = BarGOMelee;
        BarMask = BarMaskMelee;

        BarGO.SetActive(true);
        currentBarValue = 0;
        barIsIncreasing = false;
        BarON = true;

        StartCoroutine(UpdateBar());

    }

    IEnumerator UpdateBar()
    {
        while (BarON)
        {

            if (!barIsIncreasing)
            {
                currentBarValue -= barChangeSpeed;

                if (currentBarValue <= 0)
                {
                    barIsIncreasing = true;
                }
            }

            if (barIsIncreasing)
            {
                currentBarValue += barChangeSpeed;

                if (currentBarValue >= maxBarValue)
                {
                    barIsIncreasing = false;
                }
            }

            // if (isTilt)
            // {
            //     if (barIsIncreasing)
            //     {

            //         if (isTilting)
            //         {
            //             currentBarValue += barChangeSpeed;
            //         }

            //         if (currentBarValue >= maxBarValue)
            //         {
            //             barIsIncreasing = false;
            //         }
            //     }

            //     if (!barIsIncreasing)
            //     {
            //         StartCoroutine(StopBar());
            //     }

            // }

            if (isTiming || isLightOn)
            {
                float fill = currentBarValue / maxBarValue;
                BarMask.fillAmount = fill;
            }

            if (isHolding || isTilt)
            {
                barChangeSpeed = 1.5f;

                float fill = currentBarValue / maxBarValue;
                BarMask.fillAmount = fill;
            }

            yield return new WaitForSeconds(0.02f);

        }

        yield return null;
    }

    IEnumerator StopBar()
    {
        if (BarON)
        {
            float updateValue = currentBarValue / 100;

            // UPDATE TO INPUT MANAGER
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
            {

                if (isTiming)
                {

                    if (updateValue <= .3 && updateValue >= .9)
                    {
                        Debug.Log("DONE");
                    }

                    else
                    {
                        Debug.Log("MISS");
                    }

                    isTiming = false;
                }

                if (isLightOn)
                {

                    if (updateValue > .4 && updateValue < .8)
                    {
                        LightOn.SetActive(true);
                    }

                    else
                    {
                        LightOn.SetActive(false);
                    }
                }

            }

            // FIX
            if (isTilt)
            {

                if (Input.GetKey(KeyCode.C))
                {
                    Debug.Log(updateValue);

                    yield return new WaitForSeconds(4f);

                    if (updateValue <= .9)
                    {
                        Debug.Log("DONE");

                    }

                }

            }
            // 

            BarON = false;
        }

        yield return new WaitForSeconds(.7f);

        BarGO.SetActive(false);
    }
}