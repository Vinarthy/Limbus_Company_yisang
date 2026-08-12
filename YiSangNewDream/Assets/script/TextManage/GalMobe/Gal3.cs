using UnityEngine;
using UnityEngine.UI;

public class Gal3 : GalBase
{
    [Header("表情控制")]
    public ImageCharacterChange liXiangFace;
    public ImageCharacterChange dongLangFace;
    public ImageCharacterChange dongBaiFace;

    [Header("角色图片")]
    public Image liXiangImage;
    public Image dongLangImage;
    public Image dongBaiImage;

    [Header("烟雾控制")]
    public ObjectVisibleControl objectVisibleControl;
    public GameObject smoke;

    [Header("角色移动")]
    public CharacterImageMove liXiangMove;
    public CharacterImageMove dongLangMove;
    public CharacterImageMove dongBaiMove;

    private readonly Color speakingColor =
        new Color32(0xE6, 0xD3, 0xCD, 255);

    private readonly Color idleColor =
        new Color32(0xB6, 0xA5, 0xA0, 255);

    private void Awake()
    {
        ConfigureGalControl();
        ConfigureSmokeControl();
        objectVisibleControl?.SetVisible(false);
    }

    private void OnValidate()
    {
        ConfigureGalControl();
    }

    private void ConfigureGalControl()
    {
        GalControl galControl = GetComponent<GalControl>();
        if (galControl != null)
        {
            galControl.dialogPath = "Dialog/Chapter1/S5/4";
            galControl.galEvent = this;
        }
    }

    private void ConfigureSmokeControl()
    {
        if (objectVisibleControl == null)
            objectVisibleControl = GetComponent<ObjectVisibleControl>();

        if (objectVisibleControl == null)
            objectVisibleControl = gameObject.AddComponent<ObjectVisibleControl>();

        if (objectVisibleControl.targetObject == null && transform.parent != null)
        {
            Transform smokeTransform = transform.parent.Find("Particle System");
            if (smokeTransform != null)
                objectVisibleControl.targetObject = smokeTransform.gameObject;
        }
    }
    public override void OnLine(int lineNum, PlayerInfo info)
    {
        UpdateSpeakerColor(info.Name);
        objectVisibleControl?.SetVisible(lineNum >= 6 && lineNum <= 18);
        //这里开始控制角色表情和动画，liXiangMove?.MoveByX(-200f, 1.2f);移动调用，liXiangFace?.ChangeFace("Pain");表情调用
        switch (lineNum)
        {
            case 1:
                dongBaiFace?.ChangeFace("default");
                liXiangFace?.ChangeFace("smile");
                dongLangFace?.ChangeFace("2");
                break;
            case 2:
                break;
            case 3:
                smoke.SetActive(true);
                break;
            case 4:
                dongLangFace?.ChangeFace("smile");
                break;
            case 5:

                break;
            case 6:

                break;
            case 7:
                liXiangFace?.ChangeFace("default");
                break;
            case 8:
                break;
            case 9:
                liXiangFace?.ChangeFace("embarrassed");
                break;
            case 10:
                break;
            case 11:
                liXiangFace?.ChangeFace("default");
                break;
            case 12:
                smoke.SetActive(false);
                break;
            case 13:
                //东柏瞬移
                dongBaiMove?.MoveByX(-194f, 0.6f);
                liXiangFace?.ChangeFace("Smoke");
                break;
            case 14:
                dongBaiMove?.MoveByX(194f, 0.6f);
                break;
            case 15:
                break;
            case 16:
                liXiangFace?.ChangeFace("Smoke2");
                break;
            case 17:
                break;
            case 18:
                liXiangFace?.ChangeFace("default");
                break;
            case 19:
                dongLangFace?.ChangeFace("strict");
                break;
            case 20:
                break;
            case 21:
                dongLangFace?.ChangeFace("embarrassed");
                break;
            case 22:
                break;
            case 23:
                break;
            case 24:
                dongLangMove?.FadeTo(0f, 0.9f);
                dongBaiMove?.FadeTo(0f, 0.9f);
                liXiangMove?.MoveByX(190f,1f);
                liXiangFace?.ChangeFace("Pain");
                break;
            case 25:
                if (dongLangImage != null)
                    dongLangImage.gameObject.SetActive(false);

                if (dongBaiImage != null)
                    dongBaiImage.gameObject.SetActive(false);
                break;
            case 26:
                break;
            case 27:
                break;
        }
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