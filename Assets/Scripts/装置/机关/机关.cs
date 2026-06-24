using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace 装置
{
    // 机关基类：订阅装置阶段变更，到激活阶段自动激活；统一判定/事件
    public abstract class 机关 : MonoBehaviour
    {
        public string 机关名;
        public 游戏阶段 激活阶段 = 游戏阶段.机关操作;
        [ShowInInspector] public bool 已激活 { get; private set; }

        public event Action<机关> 成功;
        public event Action<机关> 失败;

        protected virtual void Start()
        {
            if (装置.main != null) 装置.main.阶段变更 += On阶段变更;
        }

        protected virtual void OnDestroy()
        {
            if (装置.main != null) 装置.main.阶段变更 -= On阶段变更;
        }

        void On阶段变更(游戏阶段 阶段)
        {
            if (阶段 == 激活阶段 && !已激活) 激活();
        }

        public void 激活()
        {
            已激活 = true;
            On激活();
        }

        protected abstract void On激活();

        protected void 判定成功()
        {
            装置.main?.记录机关成功(机关名);
            成功?.Invoke(this);
        }

        protected void 判定失败()
        {
            装置.main?.记录机关失误(机关名);
            失败?.Invoke(this);
        }
    }
}
