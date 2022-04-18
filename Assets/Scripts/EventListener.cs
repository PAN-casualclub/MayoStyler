
public enum GameplayPhase
{
    Start = 35,
    IKDrag = 60,
    MesureDia = 15,
    BikiniModel = 25,
    Painting = 20,
    Customing = 28,
    LastPose = 40,
}

public static class EventListener
{
    private static GameplayPhase currentPhase = GameplayPhase.Start;
    public static GameplayPhase CurrentPhase { get => currentPhase;}

    public delegate void IKPhase();
    public delegate void PhaseCompleted(GameplayPhase gameplay);
    public delegate void CameraStopMoving();
    public static event IKPhase IKPhaseStartedActions;
    public static event PhaseCompleted PhaseEndedActions;
    public static event CameraStopMoving CameraMoveEndActions;

    public static void OnIkSceneStart()
    {
        IKPhaseStartedActions.Invoke();
    }

    public static void OnPhaseEnded(GameplayPhase  nextPhase)
    {
        PhaseEndedActions.Invoke(nextPhase);
        currentPhase = nextPhase;
    }

    public static void OnCameraMovementFinished()
    {
        CameraMoveEndActions.Invoke();
    }
}
