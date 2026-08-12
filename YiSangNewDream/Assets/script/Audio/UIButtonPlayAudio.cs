using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonPlayAudio : MonoBehaviour
{
    [Header("AudioConfig 的 uiList 中对应的 ID")]
    public string uiId;

    [Range(0f, 1f)]
    public float volume = 1f;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(PlayAudio);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(PlayAudio);
    }

    public void PlayAudio()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("UIButtonPlayAudio：场景中不存在 AudioManager。", this);
            return;
        }

        if (string.IsNullOrWhiteSpace(uiId))
        {
            Debug.LogWarning("UIButtonPlayAudio：没有填写 UI 音效 ID。", this);
            return;
        }

        AudioManager.Instance.PlayUI(uiId, volume);
    }
}
