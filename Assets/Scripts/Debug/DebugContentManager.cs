using UnityEngine;
using UnityEngine.UI;

public class DebugContentManager : MonoBehaviour
{
    [SerializeField] Slider SliderXAxis;
    [SerializeField] Slider SliderYAxis;
    [SerializeField] Slider SliderFOW;
    [SerializeField] Slider SliderCutain;
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
        SliderXAxis.maxValue = 90;
        SliderXAxis.minValue = -90;
        SliderYAxis.maxValue = 180;
        SliderYAxis.minValue = -180;
        float fow = Camera.main.fieldOfView;
        SliderFOW.maxValue = 100;
        SliderFOW.minValue = 5;
        SliderFOW.value = fow;
        SliderCutain.minValue = 0;
        SliderCutain.maxValue = 100;
        SliderCutain.value = 0;

        configPanel.SetActive(false);
    }

    private void Update()
    {
        if (configPanel.activeInHierarchy)
        {
            InputManager.LockInputs = true;
            return;
        }

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
        InputManager.LockInputs = false;
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
        toRotate.GetComponentInChildren<Camera>().fieldOfView = (int)EventListener.CurrentPhase;
    }

    public void OnSliderYMove(Transform toRotate)
    {
        float sliderValue = SliderYAxis.value;
        toRotate.eulerAngles = new Vector3(toRotate.eulerAngles.x, sliderValue, 0);

    }

    public void OnSliderXMove(Transform toRotate)
    {
        float sliderValue = SliderXAxis.value;
        toRotate.eulerAngles = new Vector3(sliderValue,toRotate.eulerAngles.y,0);
    }

    public void OnSliderFOW(Camera toFow)
    {
        float sliderValue = SliderFOW.value;
        toFow.fieldOfView = sliderValue;
    }

    public void OnSliderCutainBlend(SkinnedMeshRenderer toBlend)
    {
        float sliderValue = SliderCutain.value;
        toBlend.SetBlendShapeWeight(0,sliderValue);
    }


}
