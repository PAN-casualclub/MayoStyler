using UnityEngine;
using UnityEngine.UI;
public class UIPerformer : MonoBehaviour
{
    [SerializeField]Button nextButton;

    public Transform rotatedObject;
    [Range(1,50)]public float rotationSpeed = 20;
    static bool rotate;
    static Vector3 direction;

    private void Start()
    {
        EventListener.PhaseFinishedActions += OnPhaseUI;
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


    public void Next()
    {
        rotatedObject.rotation = Quaternion.Euler(0,0,0);
    }

    public void OnPhaseUI(GameplayPhase phase)
    {
        switch (phase)
        {
            case GameplayPhase.Default:
                break;
            case GameplayPhase.MesureDia:
                OpenNextButton_Img();
                break;
            case GameplayPhase.BikiniModel:
                break;
            case GameplayPhase.Customing:
                break;
            case GameplayPhase.LastPose:
                break;
        }
    }

    private void OpenNextButton_Img()
    {
        nextButton.gameObject.SetActive(true);
        nextButton.interactable = false;
    }
}
