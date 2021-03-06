using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIPerformer : MonoBehaviour
{
    int currentLevel;
    [Range(1,5)]public float BlendSpeedIncreaser = 1f;
    [Range(.01f,1)]public float MoveAnimationIncreaser = .01f;
    [Range(1, 10)] public float RotaterButtonTorque = 3;

    [SerializeField] Button nextButton;
    [SerializeField] GameObject Scroll;
    [SerializeField] GameObject Sprays;
    [SerializeField] GameObject CandyCanvas;
    [SerializeField] GameObject RotationButtons;
    [SerializeField] SkinnedMeshRenderer Curtain;
    [SerializeField] Transform Table;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] Transform SpreyParent;
    public static SprayIntersection[] spreys;
    [Tooltip("Temporary !")]
    [SerializeField] GameObject CharPrefabToSpawn;

    public Transform rotatedObject;
    static bool rotate;
    static Vector3 direction;

    private void Start()
    {
        spreys = SpreyParent.GetComponentsInChildren<SprayIntersection>();
        currentLevel++;
        EnableLevelText(true);
        OpenNextButton_Img();
        EventListener.PhaseEndedActions += OnPhaseUI;
        EventListener.CameraMoveEndActions += OnCameraEndsUI;
    }

    void FixedUpdate()
    {
        if (rotate == false || InputManager.LockInputs)
            return;

        rotatedObject.Rotate(direction * RotaterButtonTorque);
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

    public static GameObject CurrentModel;
    static GameObject LastSpawned;

    /// <summary>
    /// Button Actions
    /// </summary>
    public void Next()
    {
        StopRotate();
        rotatedObject.rotation = Quaternion.Euler(0, 0, 0);
        switch (EventListener.CurrentPhase)
        {
            case GameplayPhase.Start:
                CloseNextButton_Img();
                RunBlendAnimation();
                RunWayAnimation(Vector3.left,Table);
                EventListener.OnPhaseEnded(GameplayPhase.IKDrag);
                break;
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
                EventListener.OnPhaseEnded(GameplayPhase.Painting);
                break;
            case GameplayPhase.Painting:
                CloseNextButton_Img();
                CloseSafetlySprays();
                EventListener.OnPhaseEnded(GameplayPhase.Customing);
                break;
            case GameplayPhase.Customing:
                CloseNextButton_Img();
                CandyCanvas.SetActive(false);
                RotationButtons.SetActive(false);
                EventListener.OnPhaseEnded(GameplayPhase.LastPose);
                break;
            case GameplayPhase.LastPose:
                CloseNextButton_Img();
                RunBlendAnimation();
                RunWayAnimation(Vector3.right,Table);
                EventListener.OnPhaseEnded(GameplayPhase.Start);
                currentLevel++;
                LevelText.text = $"{currentLevel}";
                break;

        }
    }

    private void CloseSafetlySprays()
    {
        InputManager.ForceReleaseInput();
        foreach (var sp3 in spreys)
        {
            sp3.gameObject.SetActive(false);
        }
        Sprays.SetActive(false);
    }

    private void OpenSafetlySprays()
    {
        InputManager.ForceReleaseInput();
        foreach (var sp3 in spreys)
        {
            sp3.gameObject.SetActive(true);
        }
        Sprays.SetActive(true);
    }

    /// <summary>
    /// Ending Phase Before Changing Game State
    /// </summary>
    /// <param name="phase"></param>
    public void OnPhaseUI(GameplayPhase phase)
    {
        switch (phase)
        {
            case GameplayPhase.Start:
                LastSpawned = Instantiate(CharPrefabToSpawn);
                if (LastSpawned != null)
                {
                    Destroy(CurrentModel);
                    LastSpawned.SetActive(true);
                    rotatedObject = LastSpawned.transform;
                    LastSpawned = null;
                }
                break;
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
                RotateToRight();
                break;

        }
    }

    public void OnCameraEndsUI()
    {
        switch (EventListener.CurrentPhase)
        {
            case GameplayPhase.Start:
                break;
            case GameplayPhase.IKDrag:
                EventListener.OnIkSceneStart();
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
                RotationButtons.SetActive(true);
                OpenSafetlySprays();
                OpenNextButton_Img();
                break;
            case GameplayPhase.Customing:
                CandyCanvas.SetActive(true);
                OpenNextButton_Img();
                break;
            case GameplayPhase.LastPose:
                OpenNextButton_Img();
                break;

        }
    }

    private void OpenNextButton_Img()
    {
        nextButton.gameObject.SetActive(true);
    }

    private void CloseNextButton_Img()
    {
        nextButton.gameObject.SetActive(false);
    }

    public void RunBlendAnimation()
    {
        StartCoroutine(RunBlend());
    }

    public void RunWayAnimation(Vector3 direction,  Transform toMove,float distanceMultiplier = 5)
    {
        StartCoroutine(RunWay(direction,toMove, distanceMultiplier));
    }

    IEnumerator RunBlend()
    {
        float blendW = Curtain.GetBlendShapeWeight(0);
        float blendCacl = blendW;
        while (true)
        {
            if (blendW <= 0)
            {
                blendCacl += BlendSpeedIncreaser;
                Curtain.SetBlendShapeWeight(0, blendCacl);
                if (blendCacl >= 100)
                {
                    Curtain.SetBlendShapeWeight(0, 100);
                    break;
                }
            }
            else if (blendW > 0)
            {
                blendCacl -= BlendSpeedIncreaser;
                Curtain.SetBlendShapeWeight(0, blendCacl);
                if (blendCacl <= 0)
                {
                    Curtain.SetBlendShapeWeight(0, 0);
                    break;
                }
            }

            yield return null;
        }
    }

    IEnumerator RunWay(Vector3 way, Transform transformToMove, float distanceMultiplier = 5)
    {

        Vector3 startPos = transformToMove.position;
        Vector3 target = startPos + (way * distanceMultiplier);
        float mag = (transformToMove.position - target).magnitude;


        while (Mathf.Abs(mag) > .1f)
        {
            transformToMove.position = Vector3.Lerp(transformToMove.position, target, MoveAnimationIncreaser);
            mag = (transformToMove.position - target).magnitude;
            yield return null;
        }

        if(way.x > 0)
        {
            // Phase Ends Restarted Game Wait Table For Open
            OpenNextButton_Img();
            EnableLevelText(true);
        }

    }

    private void EnableLevelText(bool state)
    {
        LevelText.text = $"{currentLevel}";
        LevelText.transform.parent.gameObject.SetActive(state);
    }
}
