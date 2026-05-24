using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class SpaceKeyTrackerSetup
{
    [MenuItem("Tools/Setup SpaceKeyTracker")]
    static void Setup()
    {
        // Canvas
        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Prompt Label (题目)
        var promptGO = new GameObject("PromptLabel");
        promptGO.transform.SetParent(canvasGO.transform, false);
        var prompt = promptGO.AddComponent<TextMeshProUGUI>();
        prompt.text = "短按 / 长按";
        prompt.fontSize = 64;
        prompt.alignment = TextAlignmentOptions.Center;
        var promptRect = promptGO.GetComponent<RectTransform>();
        promptRect.anchorMin = new Vector2(0.5f, 0.7f);
        promptRect.anchorMax = new Vector2(0.5f, 0.7f);
        promptRect.pivot = new Vector2(0.5f, 0.5f);
        promptRect.sizeDelta = new Vector2(600, 120);

        // Result Label (结果/计时)
        var resultGO = new GameObject("ResultLabel");
        resultGO.transform.SetParent(canvasGO.transform, false);
        var result = resultGO.AddComponent<TextMeshProUGUI>();
        result.text = "";
        result.fontSize = 36;
        result.alignment = TextAlignmentOptions.Center;
        var resultRect = resultGO.GetComponent<RectTransform>();
        resultRect.anchorMin = new Vector2(0.5f, 0.5f);
        resultRect.anchorMax = new Vector2(0.5f, 0.5f);
        resultRect.pivot = new Vector2(0.5f, 0.5f);
        resultRect.sizeDelta = new Vector2(600, 80);

        // Tracker
        var tracker = canvasGO.AddComponent<SpaceKeyTracker>();
        tracker.promptLabel = prompt;
        tracker.resultLabel = result;

        // EventSystem
        if (Object.FindObjectOfType<EventSystem>() == null)
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        EditorSceneManager.MarkSceneDirty(canvasGO.scene);
        Debug.Log("SpaceKeyTracker 设置完成！按空格开始游戏。");
    }
}
