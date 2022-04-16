using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class DebugContentManager : MonoBehaviour
{
    [SerializeField] Slider SliderXAxis;
    [SerializeField] Slider SliderYAxis;
    GameObject configPanel;
    float ScreenLimitX;
    float ScreenLimitY;
    float YRange;
    float XRange;
    int correctHits;
    bool entryHitted;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        configPanel = transform.GetChild(0).gameObject;
        ScreenLimitY = Screen.height;
        ScreenLimitX = Screen.width -100;
        YRange = ScreenLimitY - 100;
        XRange =  100;
        SliderXAxis.maxValue = 360;
        SliderYAxis.maxValue = 360;
    }

    private void Update()
    {
        if (configPanel.activeInHierarchy)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            //Left-Top Corner Of Screen
            if(Input.mousePosition.x <= XRange && Input.mousePosition.y >= YRange)
            {
                correctHits++;
            }

            if(correctHits >= 3)
            {
                // Rigth-Top Corner Of Sreen
                if(Input.mousePosition.y >= YRange && Input.mousePosition.x >= ScreenLimitX)
                {
                    entryHitted = true;
                }
            }
        }

        if(correctHits >= 4 && entryHitted)
        {
            configPanel.SetActive(true);
            ResetConditions();
        }
    }

    private void ResetConditions()
    {
        correctHits = 0;
        entryHitted = false;
    }


    public void Exit(GameObject OwnerCanvas)
    {
        OwnerCanvas.SetActive(false);
    }

    public void ActivateObj(GameObject toObje)
    {
        toObje.SetActive(true);
    }
    public void DeactivateObj(GameObject toObje)
    {
        toObje.SetActive(false);
    }

    Color Selected;
    public void PickColor(Image img)
    {
        Selected = img.color;
    }

    public void ChangeColor(Renderer toChange)
    {
        toChange.material.color = Selected;
    }

    public void Reset(Transform toRotate)
    {
        toRotate.rotation = Quaternion.Euler(0,0,0);
        SliderXAxis.value = 0;
        SliderYAxis.value = 0;
    }

    public void OnSliderYMove(Transform toRotate)
    {
        float sliderValue = SliderYAxis.value;
        toRotate.rotation = Quaternion.Euler(toRotate.rotation.eulerAngles.x, sliderValue, toRotate.rotation.eulerAngles.z);
    }

    public void OnSliderXMove(Transform toRotate)
    {
        float sliderValue = SliderXAxis.value;
        toRotate.rotation = Quaternion.Euler(sliderValue, toRotate.rotation.eulerAngles.y, toRotate.rotation.eulerAngles.z);
    }



}
