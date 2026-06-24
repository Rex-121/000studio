using System.Collections.Generic;
using 判定系统;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace 装置
{
    // 效果界面：判定块按BPM下落，空格(经装置)判定，复用 判定.抉择
    public class 效果界面 : MonoBehaviour
    {
        [LabelText("反馈文本")] public TMP_Text 反馈文本;
        [LabelText("判定线")] public RectTransform 判定线;
        [LabelText("音符容器")] public RectTransform 音符容器;

        public float bpm = 120f;
        public float 下落距离 = 600f;
        public float 判定线锚点y = 0.25f;
        public float 自动miss阈值 = 200f;

        [ShowInInspector] public int 互动数 { get; private set; }

        float 歌曲时间ms;
        readonly List<音符> 待判定 = new();
        float 一拍ms => 60000f / bpm;
        float 速度 => 下落距离 / (2f * 一拍ms);
        bool 运行中;

        class 音符 { public float 目标时刻ms; public bool 已判定; public RectTransform 视图; }

        void Update()
        {
            if (!运行中) return;
            歌曲时间ms += Time.deltaTime * 1000f;
            更新视觉();
            检查自动miss();
        }

        public void 开始判定() { 生成谱面(); 运行中 = true; }
        public void 暂停判定() => 运行中 = false;
        public void 切换谱面() { 互动数 = 0; 生成谱面(); 运行中 = true; }

        // 装置在谱面互动阶段按空格时调用
        public void 判定()
        {
            var 最近 = 选最近音符();
            if (最近 == null) return;
            var 偏差 = 歌曲时间ms - 最近.目标时刻ms;
            // var 等级 = 判定.抉择(偏差);
            // if (反馈文本 != null) 反馈文本.text = $"<color={颜色(等级.名称)}>{等级.名称}</color>";
            // 互动数++;
            // 回收(最近);
        }

        void 生成谱面()
        {
            foreach (var n in 待判定) if (n.视图 != null) Destroy(n.视图.gameObject);
            待判定.Clear();
            歌曲时间ms = 0;
            for (int i = 0; i < 16; i++)
                待判定.Add(new 音符 { 目标时刻ms = (i + 2) * 一拍ms });
        }

        void 更新视觉()
        {
            foreach (var n in 待判定)
            {
                if (n.已判定) continue;
                if (n.视图 == null) n.视图 = 建音符();
                var 偏移 = (n.目标时刻ms - 歌曲时间ms) * 速度;
                n.视图.anchoredPosition = new Vector2(0, 偏移);
            }
        }

        RectTransform 建音符()
        {
            var go = new GameObject("音符", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(音符容器, false);
            var r = (RectTransform)go.transform;
            r.anchorMin = r.anchorMax = new Vector2(0.5f, 判定线锚点y);
            r.pivot = new Vector2(0.5f, 0.5f);
            r.sizeDelta = new Vector2(120f, 40f);
            go.GetComponent<Image>().color = Color.white;
            return r;
        }

        音符 选最近音符()
        {
            音符 最近 = null;
            float 最小 = float.MaxValue;
            foreach (var n in 待判定)
            {
                if (n.已判定) continue;
                var 偏差 = Mathf.Abs(歌曲时间ms - n.目标时刻ms);
                if (偏差 > 自动miss阈值) continue;
                if (偏差 < 最小) { 最小 = 偏差; 最近 = n; }
            }
            return 最近;
        }

        void 检查自动miss()
        {
            foreach (var n in 待判定)
            {
                if (n.已判定) continue;
                if (歌曲时间ms - n.目标时刻ms > 自动miss阈值) 回收(n);
            }
        }

        void 回收(音符 n)
        {
            n.已判定 = true;
            if (n.视图 != null) { Destroy(n.视图.gameObject); n.视图 = null; }
        }

        string 颜色(string 名) => 名 switch
        {
            "Perfect" => "green",
            "Great" => "blue",
            "Okay" => "yellow",
            _ => "red"
        };
    }
}
