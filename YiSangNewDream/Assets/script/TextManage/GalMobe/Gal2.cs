using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Gal2 : GalBase
{
    [Header("表情控制")]
    public ImageCharacterChange liXiangFace;
    public ImageCharacterChange dongLangFace;
    public ImageCharacterChange dongBaiFace;
    public ImageCharacterChange qiuFuFace;
    public ImageCharacterChange yaXiYaFace;

    [Header("角色图片")]
    public Image liXiangImage;
    public Image dongLangImage;
    public Image dongBaiImage;
    public Image qiuFuImage;
    public Image yaXiYaImage;

    [Header("角色离场")]
    [SerializeField] private float exitDuration = 0.8f;
    [SerializeField] private float leftExitX = -600f;
    [SerializeField] private float rightExitX = 600f;

    private readonly Color speakingColor =
        new Color32(0xE6, 0xD3, 0xCD, 255);

    private readonly Color idleColor =
        new Color32(0xB6, 0xA5, 0xA0, 255);

    private Tween qiuFuExitTween;
    private Tween dongLangExitTween;
    private Tween yaXiYaExitTween;

    private void OnDestroy()
    {
        qiuFuExitTween?.Kill();
        dongLangExitTween?.Kill();
        yaXiYaExitTween?.Kill();
    }

    public override void OnLine(int lineNum, PlayerInfo info)
    {
        UpdateSpeakerColor(info.Name);

        switch (lineNum)
        {
            case 1:
                dongLangFace?.ChangeFace("strict");
                break;

            case 2:
                dongBaiFace?.ChangeFace("Shock");
                liXiangFace?.ChangeFace("Pain");
                break;

            case 5:
                dongBaiFace?.ChangeFace("Angry");
                break;

            case 6:
            case 8:
                dongLangFace?.ChangeFace("smile");
                break;

            case 11:
                liXiangFace?.ChangeFace("Cry");
                break;

            case 14:
                dongBaiFace?.ChangeFace("Angry");
                qiuFuExitTween = MoveCharacterOut(qiuFuImage, leftExitX, qiuFuExitTween);
                break;

            case 15:
                dongLangExitTween = MoveCharacterOut(dongLangImage, rightExitX, dongLangExitTween);
                break;

            case 17:
                dongBaiFace?.ChangeFace("Shock");
                yaXiYaExitTween = MoveCharacterOut(yaXiYaImage, leftExitX, yaXiYaExitTween);
                break;
        }
    }

    private Tween MoveCharacterOut(Image characterImage, float targetX, Tween currentTween)
    {
        if (characterImage == null)
            return currentTween;

        currentTween?.Kill();
        return characterImage.rectTransform
            .DOAnchorPosX(targetX, exitDuration)
            .SetEase(Ease.InQuad);
    }

    private void UpdateSpeakerColor(string speakerName)
    {
        if (liXiangImage != null) liXiangImage.color = idleColor;
        if (dongLangImage != null) dongLangImage.color = idleColor;
        if (dongBaiImage != null) dongBaiImage.color = idleColor;
        if (qiuFuImage != null) qiuFuImage.color = idleColor;
        if (yaXiYaImage != null) yaXiYaImage.color = idleColor;

        switch (speakerName)
        {
            case "李箱":
                if (liXiangImage != null)
                    liXiangImage.color = speakingColor;
                break;

            case "东朗":
                if (dongLangImage != null)
                    dongLangImage.color = speakingColor;
                break;

            case "冬柏":
                if (dongBaiImage != null)
                    dongBaiImage.color = speakingColor;
                break;

            case "仇甫":
                if (qiuFuImage != null)
                    qiuFuImage.color = speakingColor;
                break;

            case "亚细亚":
                if (yaXiYaImage != null)
                    yaXiYaImage.color = speakingColor;
                break;
        }
    }
}