using UnityEngine;

public enum StoryAdvanceType
{
    None,
    Scene,
    Day,
    Chapter
}

public class TimeBool : MonoBehaviour
{
    public static TimeBool Instance;

    public StoryAdvanceType AdvanceType =
        StoryAdvanceType.None;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            return;

        Instance = this;
    }

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
//这个弄一个单例的bool用于记录是否该换场景了
//大的增加小的全部归零