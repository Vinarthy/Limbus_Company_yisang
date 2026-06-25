using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Gal1 : GalBase
{
    [Header("角色表情控制")]
    public ImageCharacterChange liXiangFace;
    public ImageCharacterChange dongLangFace;
    public ImageCharacterChange dongBaiFace;

    [Header("角色图片")]
    public Image liXiangImage;
    public Image dongLangImage;
    public Image dongBaiImage;

    [Header("回忆入场演出")]
    [SerializeField] private float liXiangMoveDuration = 0.8f;
    [SerializeField] private float characterFadeDuration = 0.5f;

    private static readonly Vector2 LiXiangStartPosition =
        new Vector2(-21f, -33.596f);

    private static readonly Vector2 LiXiangEndPosition =
        new Vector2(-247f, -33.596f);

    private readonly Color speakingColor =
        new Color32(0xE6, 0xD3, 0xCD, 255);

    private readonly Color idleColor =
        new Color32(0xB6, 0xA5, 0xA0, 255);

    private RectTransform liXiangRect;
    private Tween liXiangMoveTween;
    private Tween dongLangFadeTween;
    private Tween dongBaiFadeTween;
    private bool dongLangRevealed;
    private bool dongBaiRevealed;

    private void Awake()
    {
        if (liXiangImage != null)
        {
            liXiangRect = liXiangImage.rectTransform;
            liXiangRect.anchoredPosition = LiXiangStartPosition;
            SetAlpha(liXiangImage, 1f);
        }

        SetAlpha(dongLangImage, 0f);
        SetAlpha(dongBaiImage, 0f);
    }

    private void OnDestroy()
    {
        liXiangMoveTween?.Kill();
        dongLangFadeTween?.Kill();
        dongBaiFadeTween?.Kill();
    }

    public override void OnLine(int lineNum, PlayerInfo info)
    {
        UpdateSpeakerColor(info.Name);

        switch (lineNum)
        {
            case 1:
                liXiangFace?.ChangeFace("Pain");
                break;

            case 2:
                MoveLiXiangToMemoryPosition();
                RevealDongLang();
                dongLangFace?.ChangeFace("Normal");
                break;

            case 3:
                RevealDongBai();
                dongBaiFace?.ChangeFace("Shock");
                break;

            case 5:
                dongBaiFace?.ChangeFace("Shock");
                break;

            case 6:
                liXiangFace?.ChangeFace("Cry");
                break;
        }
    }

    private void MoveLiXiangToMemoryPosition()
    {
        if (liXiangRect == null)
            return;

        liXiangMoveTween?.Kill();
        liXiangMoveTween = liXiangRect
            .DOAnchorPos(LiXiangEndPosition, liXiangMoveDuration)
            .SetEase(Ease.InOutSine);
    }

    private void RevealDongLang()
    {
        if (dongLangRevealed || dongLangImage == null)
            return;

        dongLangRevealed = true;
        dongLangFadeTween?.Kill();
        dongLangFadeTween = dongLangImage
            .DOFade(1f, characterFadeDuration)
            .SetEase(Ease.Linear);
    }

    private void RevealDongBai()
    {
        if (dongBaiRevealed || dongBaiImage == null)
            return;

        dongBaiRevealed = true;
        dongBaiFadeTween?.Kill();
        dongBaiFadeTween = dongBaiImage
            .DOFade(1f, characterFadeDuration)
            .SetEase(Ease.Linear);
    }

    private void UpdateSpeakerColor(string speakerName)
    {
        ApplyTone(liXiangImage, idleColor);
        ApplyTone(dongLangImage, idleColor);
        ApplyTone(dongBaiImage, idleColor);

        switch (speakerName)
        {
            case "李箱":
                ApplyTone(liXiangImage, speakingColor);
                break;

            case "东朗":
                ApplyTone(dongLangImage, speakingColor);
                break;

            case "冬柏":
                ApplyTone(dongBaiImage, speakingColor);
                break;
        }
    }

    private static void ApplyTone(Image image, Color tone)
    {
        if (image == null)
            return;

        float alpha = image.color.a;
        image.color = new Color(tone.r, tone.g, tone.b, alpha);
    }

    private static void SetAlpha(Image image, float alpha)
    {
        if (image == null)
            return;

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}