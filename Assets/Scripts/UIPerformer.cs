using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class UIPerformer : MonoBehaviour
{
    private const float BlendSpeedIncreaser = 1f;
    [SerializeField] Button nextButton;
    [SerializeField] GameObject Scroll;
    [SerializeField] GameObject Sprays;
    [SerializeField] GameObject CandyCanvas;
    [SerializeField] GameObject RotationButtons;
    [SerializeField] SkinnedMeshRenderer Curtain;
    [SerializeField] Transform Table;

    [Tooltip("Temporary !")]
    [SerializeField] GameObject CharPrefabToSpawn;

    public Transform rotatedObject;
    [Range(1, 50)] public float rotationSpeed = 20;
    static bool rotate;
    static Vector3 direction;

    private void Start()
    {
        OpenNextButton_Img();
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
                Sprays.SetActive(false);
                //RotationButtons.SetActive(false);
                EventListener.OnPhaseEnded(GameplayPhase.Customing);
                break;
            case GameplayPhase.Customing:
                CloseNextButton_Img();
                CandyCanvas.SetActive(false);
                EventListener.OnPhaseEnded(GameplayPhase.LastPose);
                break;
            case GameplayPhase.LastPose:
                RotationButtons.SetActive(false);
                RunBlendAnimation();
                RunWayAnimation(Vector3.right,Table);
                EventListener.OnPhaseEnded(GameplayPhase.Start);
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
            case GameplayPhase.Start:
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
                Sprays.SetActive(true);
                OpenNextButton_Img();
                break;
            case GameplayPhase.Customing:
                CandyCanvas.SetActive(true);
                OpenNextButton_Img();
                break;
            case GameplayPhase.LastPose:
                OpenNextButton_Img();
                LastSpawned = Instantiate(CharPrefabToSpawn);
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
            transformToMove.position = Vector3.Lerp(transformToMove.position, target, .01f);
            mag = (transformToMove.position - target).magnitude;
            yield return null;
        }


    }
}
