using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using 装置;

public static class 装置Setup
{
    [MenuItem("Tools/Setup 装置")]
    static void Setup()
    {
        var font = Resources.Load<TMP_FontAsset>("SmileySans-Oblique SDF");

        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // 提示（左上，小纸条）
        var 提示GO = new GameObject("提示");
        提示GO.transform.SetParent(canvasGO.transform, false);
        var 提示文本 = 建文本(提示GO, font, "不要点击这个按钮", 36);
        var 提示rect = 提示GO.GetComponent<RectTransform>();
        提示rect.anchorMin = 提示rect.anchorMax = new Vector2(0.05f, 0.95f);
        提示rect.pivot = new Vector2(0f, 1f);
        提示rect.sizeDelta = new Vector2(450f, 100f);
        var 提示组件 = 提示GO.AddComponent<提示>();
        提示组件.文本 = 提示文本;

        // 效果界面（上方）
        var 效果GO = new GameObject("效果界面", typeof(RectTransform));
        效果GO.transform.SetParent(canvasGO.transform, false);

        var 判定线GO = new GameObject("判定线", typeof(Image));
        判定线GO.transform.SetParent(效果GO.transform, false);
        var 判定线rect = 判定线GO.GetComponent<RectTransform>();
        判定线rect.anchorMin = 判定线rect.anchorMax = new Vector2(0.5f, 0.25f);
        判定线rect.sizeDelta = new Vector2(400f, 8f);
        判定线GO.GetComponent<Image>().color = new Color(1f, 0.4f, 0.4f);

        var 容器GO = new GameObject("音符容器", typeof(RectTransform));
        容器GO.transform.SetParent(效果GO.transform, false);
        var 容器rect = 容器GO.GetComponent<RectTransform>();
        容器rect.anchorMin = Vector2.zero;
        容器rect.anchorMax = Vector2.one;
        容器rect.offsetMin = 容器rect.offsetMax = Vector2.zero;

        var 反馈GO = new GameObject("反馈文本");
        反馈GO.transform.SetParent(效果GO.transform, false);
        建文本(反馈GO, font, "", 48);
        var 反馈rect = 反馈GO.GetComponent<RectTransform>();
        反馈rect.anchorMin = 反馈rect.anchorMax = new Vector2(0.5f, 0.65f);
        反馈rect.sizeDelta = new Vector2(400f, 80f);

        var 效果组件 = 效果GO.AddComponent<效果界面>();
        效果组件.反馈文本 = 反馈GO.GetComponent<TMP_Text>();
        效果组件.判定线 = 判定线rect;
        效果组件.音符容器 = 容器rect;

        // 主按钮（下方操作界面）
        var 按钮GO = new GameObject("主按钮");
        按钮GO.transform.SetParent(canvasGO.transform, false);
        建文本(按钮GO, font, "按 [空格]", 56);
        var 按钮rect = 按钮GO.GetComponent<RectTransform>();
        按钮rect.anchorMin = 按钮rect.anchorMax = new Vector2(0.5f, 0.08f);
        按钮rect.sizeDelta = new Vector2(400f, 80f);
        按钮GO.AddComponent<主按钮>();

        // 密码盘（占位）
        var 密码盘GO = new GameObject("密码盘");
        密码盘GO.transform.SetParent(canvasGO.transform, false);
        建文本(密码盘GO, font, "密码盘\n数字键输入 · 回车提交\n(123 定曲风 / 199876 彩蛋)", 28);
        var 密码盘rect = 密码盘GO.GetComponent<RectTransform>();
        密码盘rect.anchorMin = 密码盘rect.anchorMax = new Vector2(0.5f, 0.32f);
        密码盘rect.sizeDelta = new Vector2(500f, 120f);
        var 密码盘组件 = 密码盘GO.AddComponent<密码盘>();
        密码盘组件.机关名 = "密码盘";

        // 结局显示（右侧）
        var 结局GO = new GameObject("结局显示");
        结局GO.transform.SetParent(canvasGO.transform, false);
        var 结局文本 = 建文本(结局GO, font, "", 32);
        var 结局rect = 结局GO.GetComponent<RectTransform>();
        结局rect.anchorMin = 结局rect.anchorMax = new Vector2(0.95f, 0.5f);
        结局rect.pivot = new Vector2(1f, 0.5f);
        结局rect.sizeDelta = new Vector2(320f, 100f);
        var 结局组件 = 结局GO.AddComponent<结局系统>();
        结局组件.结局文本 = 结局文本;

        // 装置（顶层）
        var 装置组件 = canvasGO.AddComponent<装置.装置>();
        装置组件.提示 = 提示组件;
        装置组件.效果界面 = 效果组件;

        if (Object.FindObjectOfType<EventSystem>() == null)
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        EditorSceneManager.MarkSceneDirty(canvasGO.scene);
        Debug.Log("装置骨架搭建完成！按 Play：空格推进流程；密码盘阶段用数字键+回车测试。");
    }

    static TMP_Text 建文本(GameObject go, TMP_FontAsset font, string text, float size)
    {
        var t = go.AddComponent<TextMeshProUGUI>();
        if (font != null) t.font = font;
        t.text = text;
        t.fontSize = size;
        t.alignment = TextAlignmentOptions.Center;
        return t;
    }
}
