using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Gal4 : GalBase
{
    [Header("表情控制")]
    public ImageCharacterChange liXiangFace;
    public ImageCharacterChange dongLangFace;
    public ImageCharacterChange dongBaiFace;

    [Header("角色图片")]
    public Image liXiangImage;
    public Image dongLangImage;
    public Image dongBaiImage;

    [Header("角色移动")]
    public CharacterImageMove liXiangMove;
    public CharacterImageMove dongLangMove;
    public CharacterImageMove dongBaiMove;

    [Header("CG 控制")]
    public ObjectVisibleControl cgVisibleControl;
    [Min(0f)] public float cgFadeDuration = 0.6f;
    [Header("特殊物件演出")]
    public GameObject Shit;
    public GameObject boom;

    private Image cgImage;
    private Tween cgFadeTween;

    private readonly Color speakingColor =
        new Color32(0xE6, 0xD3, 0xCD, 255);

    private readonly Color idleColor =
        new Color32(0xB6, 0xA5, 0xA0, 255);

    private void Awake()
    {
        ConfigureGalControl();
        ConfigureCg();
    }

    private void OnDestroy()
    {
        cgFadeTween?.Kill();
    }

    private void ConfigureGalControl()
    {
        GalControl galControl = GetComponent<GalControl>();
        if (galControl == null)
            return;

        galControl.dialogPath = "Dialog/Chapter1/S4/4";
        galControl.galEvent = this;
    }

    public override void OnLine(int lineNum, PlayerInfo info)
    {
        UpdateSpeakerColor(info.Name);
        if (lineNum <= 5)
            ShowCg();
        else if (lineNum == 6)
            HideCgWithFade();

        // 每句单独保留，后续角色表情与移动直接写入对应 case。
        switch (lineNum)
        {
            case 1:
                dongBaiFace?.ChangeFace("default");
                liXiangFace?.ChangeFace("smile");
                dongLangFace?.ChangeFace("smile");
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                //这句结束之后是剧情演出咯
                //众所周知倪瓶说话最喜欢回旋镖了
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                //炸，然后箱柏跑
                boom.SetActive(true);
                dongBaiMove?.MoveByX(-194f, 0.6f);
                liXiangMove?.MoveByX(-194f, 0.6f);
                break;
            case 15:
                boom.SetActive(false);
                Shit.SetActive(true);
                break;
            case 16:
                break;
            case 17:
                break;
            case 18:
                break;
            case 19:
                break;
            case 20:
                break;
            case 21:
                break;
        }
    }

    private void ConfigureCg()
    {
        if (cgVisibleControl == null || cgVisibleControl.targetObject == null)
            return;

        cgImage = cgVisibleControl.targetObject.GetComponent<Image>();
        ShowCg();
    }

    private void ShowCg()
    {
        if (cgVisibleControl == null)
            return;

        cgFadeTween?.Kill();
        cgVisibleControl.SetVisible(true);

        if (cgImage != null)
            cgImage.color = new Color(cgImage.color.r, cgImage.color.g, cgImage.color.b, 1f);
    }

    private void HideCgWithFade()
    {
        if (cgVisibleControl == null)
            return;

        if (cgImage == null)
        {
            cgVisibleControl.SetVisible(false);
            return;
        }

        cgFadeTween?.Kill();
        cgFadeTween = cgImage
            .DOFade(0f, Mathf.Max(0f, cgFadeDuration))
            .SetEase(Ease.OutQuad)
            .OnComplete(() => cgVisibleControl.SetVisible(false));
    }

    private void UpdateSpeakerColor(string speakerName)
    {
        if (liXiangImage != null) liXiangImage.color = idleColor;
        if (dongLangImage != null) dongLangImage.color = idleColor;
        if (dongBaiImage != null) dongBaiImage.color = idleColor;

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
        }
    }
}