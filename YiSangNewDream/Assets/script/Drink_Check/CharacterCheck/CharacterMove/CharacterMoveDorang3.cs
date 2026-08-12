using System.Collections;
using UnityEngine;

public class CharacterMoveDorang3 : MonoBehaviour
{
    private RecordLineNumber record;
    private Plot_Dy plot;
    private GameObject produce;
    private bool produceShown;
    private bool sceneAdvanced;

    private void Start()
    {
        record = GetComponent<RecordLineNumber>();
        plot = GetComponent<Plot_Dy>();
        produce = FindProduceObject();
        if (record == null || plot == null)
        {
            Debug.LogError("CharacterMoveDorang3 needs Plot_Dy and RecordLineNumber.", this);
            enabled = false;
            return;
        }
        record.OnLineChanged += OnLineChanged;
    }

    private void OnDestroy()
    {
        if (record != null) record.OnLineChanged -= OnLineChanged;
    }

    private void OnLineChanged(int line)
    {
        if (plot.x == 0 && line >= 10) StartCoroutine(ShowProduce());
        else if (plot.x == 1 && line >= 2) AdvanceScene();
    }

    private IEnumerator ShowProduce()
    {
        if (produceShown) yield break;
        produceShown = true;
        yield return new WaitForSeconds(0.3f);
        if (produce != null) produce.SetActive(true);
    }

    private void AdvanceScene()
    {
        if (sceneAdvanced) return;
        sceneAdvanced = true;
        if (TimeBool.Instance != null) TimeBool.Instance.AdvanceType = StoryAdvanceType.Scene;
        else Debug.LogError("CharacterMoveDorang3 cannot find TimeBool.", this);
    }

    private static GameObject FindProduceObject()
    {
        foreach (Canvas canvas in FindObjectsOfType<Canvas>(true))
        {
            if (canvas.name != "Canvas") continue;
            Transform target = canvas.transform.Find("produce");
            if (target != null) return target.gameObject;
        }
        return null;
    }
}