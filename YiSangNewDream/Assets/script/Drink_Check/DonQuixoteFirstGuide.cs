using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(Plot_Dy))]
public class DonQuixoteFirstGuide : MonoBehaviour
{
    private enum GuideStep
    {
        WaitingForProduction,
        Cup,
        Tea,
        Season,
        Tip,
        WaitingForServe,
        Garbage,
        Finish,
        WaitingForFeedback,
        Return
    }

    private Plot_Dy plot;
    private Product productButton;
    private FinishButtonHandler finishButton;

    private GameObject guideCup;
    private GameObject guideTea;
    private GameObject guideSeason;
    private GameObject guideTip;
    private GameObject guideGarbage;
    private GameObject guideFinish;
    private GameObject guideReturn;
    private GameObject currentGuide;

    private GuideStep step = GuideStep.WaitingForProduction;
    private int guideShownFrame = -1;

    private void Start()
    {
        plot = GetComponent<Plot_Dy>();
        productButton = FindObjectOfType<Product>(true);
        finishButton = FindObjectOfType<FinishButtonHandler>(true);

        guideCup = FindSceneObject("guide0cup");
        guideTea = FindSceneObject("guidetea");
        guideSeason = FindSceneObject("guide0season");
        guideTip = FindSceneObject("guide0Tip");
        guideGarbage = FindSceneObject("guidegarbage");
        guideFinish = FindSceneObject("guidefinish");
        guideReturn = FindSceneObject("guidereturn");

        if (!HasRequiredReferences())
        {
            Debug.LogError("DonQuixoteFirstGuide：Middle 引导对象或制作按钮配置不完整。", this);
            enabled = false;
            return;
        }

        HideAllGuides();
        productButton.ProductionStarted += BeginGuide;
        finishButton.DishServed += OnDishServed;
    }

    private void Update()
    {
        if (step == GuideStep.WaitingForFeedback)
        {
            if (plot.x != 0 && plot.dialogFinished)
                ShowGuide(guideReturn, GuideStep.Return);

            return;
        }

        if (!IsClickStep(step)
            || Time.frameCount <= guideShownFrame
            || !Input.GetMouseButtonDown(0))
        {
            return;
        }

        AdvanceGuide();
    }

    private void OnDestroy()
    {
        if (productButton != null)
            productButton.ProductionStarted -= BeginGuide;

        if (finishButton != null)
            finishButton.DishServed -= OnDishServed;

        HideAllGuides();
    }

    private void BeginGuide()
    {
        if (step == GuideStep.WaitingForProduction)
            ShowGuide(guideCup, GuideStep.Cup);
    }

    private void OnDishServed()
    {
        if (step == GuideStep.WaitingForServe)
            ShowGuide(guideGarbage, GuideStep.Garbage);
    }

    private void AdvanceGuide()
    {
        switch (step)
        {
            case GuideStep.Cup:
                ShowGuide(guideTea, GuideStep.Tea);
                break;

            case GuideStep.Tea:
                ShowGuide(guideSeason, GuideStep.Season);
                break;

            case GuideStep.Season:
                ShowGuide(guideTip, GuideStep.Tip);
                break;

            case GuideStep.Tip:
                HideCurrentGuide();
                step = GuideStep.WaitingForServe;
                break;

            case GuideStep.Garbage:
                ShowGuide(guideFinish, GuideStep.Finish);
                break;

            case GuideStep.Finish:
                HideCurrentGuide();
                step = GuideStep.WaitingForFeedback;
                break;
        }
    }

    private void ShowGuide(GameObject guide, GuideStep nextStep)
    {
        HideCurrentGuide();
        currentGuide = guide;
        currentGuide.SetActive(true);
        step = nextStep;
        guideShownFrame = Time.frameCount;
    }

    private void HideCurrentGuide()
    {
        if (currentGuide != null)
            currentGuide.SetActive(false);

        currentGuide = null;
    }

    private void HideAllGuides()
    {
        SetInactive(guideCup);
        SetInactive(guideTea);
        SetInactive(guideSeason);
        SetInactive(guideTip);
        SetInactive(guideGarbage);
        SetInactive(guideFinish);
        SetInactive(guideReturn);
        currentGuide = null;
    }

    private bool HasRequiredReferences()
    {
        return plot != null
            && productButton != null
            && finishButton != null
            && guideCup != null
            && guideTea != null
            && guideSeason != null
            && guideTip != null
            && guideGarbage != null
            && guideFinish != null
            && guideReturn != null;
    }

    private static bool IsClickStep(GuideStep currentStep)
    {
        return currentStep == GuideStep.Cup
            || currentStep == GuideStep.Tea
            || currentStep == GuideStep.Season
            || currentStep == GuideStep.Tip
            || currentStep == GuideStep.Garbage
            || currentStep == GuideStep.Finish;
    }

    private static void SetInactive(GameObject target)
    {
        if (target != null)
            target.SetActive(false);
    }

    private static GameObject FindSceneObject(string objectName)
    {
        Scene scene = SceneManager.GetActiveScene();
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == objectName)
                    return child.gameObject;
            }
        }

        return null;
    }
}