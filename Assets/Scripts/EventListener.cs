
public enum GameplayPhase
{
    Default = 60,
    MesureDia = 15,
    BikiniModel = 25,
    Customing = 20,
    LastPose = 30,
}

public static class EventListener
{
    private static GameplayPhase currentPhase;

    public static GameplayPhase CurrentPhase { get => currentPhase;}

    public delegate void OnIKScene();
    public delegate void OnIKSceneCompleted(GameplayPhase gameplay);
    public static event OnIKScene OnIKPlayStarted;
    public static event OnIKSceneCompleted PhaseFinishedActions;

    public static void OnIkSceneStart()
    {
        OnIKPlayStarted.Invoke();
    }

    public static void OnPhaseEnded(GameplayPhase  nextPhase)
    {
        PhaseFinishedActions.Invoke(nextPhase);
        currentPhase = nextPhase;
    }
}
