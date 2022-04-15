using UnityEngine;
using UnityEngine.UI;
public class UIPerformer : MonoBehaviour
{
    [SerializeField]Button nextButton;
    [SerializeField]GameObject Scroll;
    [SerializeField]GameObject Sprays;
    [SerializeField]GameObject CandyCanvas;

    public Transform rotatedObject;
    [Range(1,50)]public float rotationSpeed = 20;
    static bool rotate;
    static Vector3 direction;

    private void Start()
    {
        EventListener.PhaseEndedActions += OnPhaseUI;
        EventListener.CameraMoveEndActions += OnCameraEndsUI;
    }

    void FixedUpdate()
    {
        if (rotate == false)
            return;

        rotatedObject.Rotate(direction * rotationSpeed);
    }

    public void RotateToRight()
    {
        rotate = true;
        direction = Vector3.down;
    }

    public void RotateToLeft()
    {
        rotate = true;
        direction = Vector3.up;
    }

    public void OnButtonUp()
    {
        rotate = false;
    }

    public static void RotateRight()
    {
        rotate = true;
        direction = Vector3.down;
    }

    public static void StopRotate()
    {
        rotate = false;
    }


    /// <summary>
    /// Button Actions
    /// </summary>
    public void Next()
    {
        StopRotate();
        rotatedObject.rotation = Quaternion.Euler(0,0,0);
        switch (EventListener.CurrentPhase)
        {
            case GameplayPhase.IKDrag:
                EventListener.OnPhaseEnded(GameplayPhase.MesureDia);
                break;
            case GameplayPhase.MesureDia:
                CloseNextButton_Img();
                EventListener.OnPhaseEnded(GameplayPhase.BikiniModel);
                break;
            case GameplayPhase.BikiniModel:
                CloseNextButton_Img();
                Scroll.SetActive(false);
                EventListener.OnPhaseEnded(GameplayPhase.Customing);
                break;
            case GameplayPhase.Painting:
                CloseNextButton_Img();
                Sprays.SetActive(false);
                EventListener.OnPhaseEnded(GameplayPhase.Customing);
                break;
            case GameplayPhase.Customing:
                CloseNextButton_Img();
                CandyCanvas.SetActive(false);
                EventListener.OnPhaseEnded(GameplayPhase.LastPose);
                break;
            case GameplayPhase.LastPose:
                EventListener.OnPhaseEnded(GameplayPhase.IKDrag);
                break;
        }
    }

    /// <summary>
    /// Ending Phase Before Changing Game State
    /// </summary>
    /// <param name="phase"></param>
    public void OnPhaseUI(GameplayPhase phase)
    {
        switch (phase)
        {
            case GameplayPhase.IKDrag:
                break;
            case GameplayPhase.MesureDia:
                CloseNextButton_Img();
                break;
            case GameplayPhase.BikiniModel:
                StopRotate();
                Scroll.SetActive(false);
                break;
            case GameplayPhase.Painting:
                Sprays.SetActive(false);
                break;
            case GameplayPhase.Customing:
                break;
            case GameplayPhase.LastPose:
                break;
        }
    }

    public void OnCameraEndsUI()
    {
        switch (EventListener.CurrentPhase)
        {
            case GameplayPhase.IKDrag:
                break;
            case GameplayPhase.MesureDia:
                OpenNextButton_Img();
                break;
            case GameplayPhase.BikiniModel:
                StopRotate();
                Scroll.SetActive(true);
                OpenNextButton_Img();
                break;
            case GameplayPhase.Painting:
                Sprays.SetActive(true);
                OpenNextButton_Img();
                break;
            case GameplayPhase.Customing:
                CandyCanvas.SetActive(true);
                break;
            case GameplayPhase.LastPose:
                break;
        }
    }

    private void OpenNextButton_Img()
    {
        nextButton.gameObject.SetActive(true);
        //nextButton.interactable = false;
    }

    private void CloseNextButton_Img()
    {
        nextButton.gameObject.SetActive(false);
        //nextButton.interactable = false;
    }
}
