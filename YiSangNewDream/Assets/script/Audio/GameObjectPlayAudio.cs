using UnityEngine;

public class GameObjectPlayAudio : MonoBehaviour
{
    [Header("AudioConfig 的 sfxList 中对应的 ID")]
    public string sfxId;

    [Range(0f, 1f)]
    public float volume = 1f;

    private void OnMouseDown()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("GameObjectPlayAudio：场景中不存在 AudioManager。", this);
            return;
        }

        if (string.IsNullOrWhiteSpace(sfxId))
        {
            Debug.LogWarning("GameObjectPlayAudio：没有填写 SFX ID。", this);
            return;
        }

        AudioManager.Instance.PlaySFX(sfxId, volume);
    }
}