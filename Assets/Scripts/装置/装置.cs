using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace 装置
{
    // 装置顶层：状态机 + 全局统计 + 流程推进
    public class 装置 : MonoBehaviour
    {
        public static 装置 main;

        [Header("界面引用")]
        public 提示 提示;
        public 效果界面 效果界面;

        [Header("流程参数")]
        public int 启动到密码盘点击数 = 5;
        public int 谱面到机关互动数 = 5;
        public int 频繁点击判定数 = 10;

        [ShowInInspector] public 游戏阶段 当前阶段 { get; private set; }

        // 全局统计（结局触发依据）
        [ShowInInspector] public int 主按钮点击数;
        [ShowInInspector] public int 主按钮快速点击数;
        [ShowInInspector] public int 连击;
        public readonly Dictionary<string, int> 各机关失误 = new();

        public event Action<游戏阶段> 阶段变更;
        public event Action<结局类型> 结局请求;
        public event Action<string> 机关失误发生;
        public event Action<string> 机关成功;

        float 上次主按钮时间;

        void Awake() => main = this;

        void Start() => 进入阶段(游戏阶段.未开始);

        public void 点击主按钮()
        {
            主按钮点击数++;

            // 频繁点击检测（结局4）
            var now = Time.time;
            主按钮快速点击数 = now - 上次主按钮时间 < 0.3f ? 主按钮快速点击数 + 1 : 1;
            上次主按钮时间 = now;
            if (主按钮快速点击数 >= 频繁点击判定数)
            {
                结局请求?.Invoke(结局类型.频繁点击主按钮);
                return;
            }

            switch (当前阶段)
            {
                case 游戏阶段.未开始:
                    进入阶段(游戏阶段.启动);
                    break;
                case 游戏阶段.启动:
                    if (主按钮点击数 >= 启动到密码盘点击数) 进入阶段(游戏阶段.密码盘);
                    break;
                case 游戏阶段.谱面互动:
                    效果界面?.判定();
                    if (效果界面 != null && 效果界面.互动数 >= 谱面到机关互动数)
                        进入阶段(游戏阶段.机关解锁);
                    break;
            }
        }

        public void 进入阶段(游戏阶段 新阶段)
        {
            当前阶段 = 新阶段;
            阶段变更?.Invoke(新阶段);
            Debug.Log($"[装置] 阶段 → {新阶段}");

            switch (新阶段)
            {
                case 游戏阶段.未开始:
                    提示?.设置("不要点击这个按钮");
                    break;
                case 游戏阶段.启动:
                    提示?.设置("跟着节奏点击");
                    效果界面?.开始判定();
                    break;
                case 游戏阶段.密码盘:
                    提示?.设置("输入数字选择曲风");
                    效果界面?.暂停判定();
                    break;
                case 游戏阶段.曲风确定:
                    提示?.设置("按谱面互动");
                    效果界面?.切换谱面();
                    进入阶段(游戏阶段.谱面互动);
                    break;
                case 游戏阶段.机关解锁:
                    提示?.设置("操作出现的机关");
                    进入阶段(游戏阶段.机关操作);
                    break;
            }
        }

        public void 记录机关失误(string 机关名)
        {
            各机关失误.TryGetValue(机关名, out var c);
            各机关失误[机关名] = c + 1;
            机关失误发生?.Invoke(机关名);
        }

        public readonly HashSet<string> 成功机关 = new();

        public void 记录机关成功(string 机关名)
        {
            成功机关.Add(机关名);
            机关成功?.Invoke(机关名);
        }

        // 外部请求触发结局（event 只能本类Invoke）
        public void 请求结局(结局类型 类型) => 结局请求?.Invoke(类型);
    }
}
