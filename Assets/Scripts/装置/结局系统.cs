using TMPro;
using UnityEngine;

namespace 装置
{
    // 监听装置结局请求+统计，触发结局演出（骨架：文本+日志）
    public class 结局系统 : MonoBehaviour
    {
        public TMP_Text 结局文本;
        public float 不动超时 = 20f;
        public int 同组件失误阈值 = 5;
        public int 多组件失误总和阈值 = 10;

        float 最后活动时间;
        bool 已结局;

        void Start()
        {
            装置.main.结局请求 += 触发;
            装置.main.机关失误发生 += On机关失误;
            最后活动时间 = Time.time;
        }

        void Update()
        {
            if (已结局) return;
            if (Time.time - 最后活动时间 > 不动超时)
                触发(结局类型.长时间不动);
        }

        void LateUpdate()
        {
            if (Input.anyKey || Input.GetAxis("Mouse X") != 0f) 最后活动时间 = Time.time;
        }

        void On机关失误(string 名)
        {
            if (已结局) return;
            if (装置.main.各机关失误.TryGetValue(名, out var c) && c >= 同组件失误阈值)
                触发(结局类型.同组件多次错);
            int 总 = 0;
            foreach (var v in 装置.main.各机关失误.Values) 总 += v;
            if (总 >= 多组件失误总和阈值) 触发(结局类型.多组件错总和);
        }

        public void 触发(结局类型 类型)
        {
            if (已结局) return;
            已结局 = true;
            装置.main.进入阶段(游戏阶段.结局);
            if (结局文本 != null) 结局文本.text = $"结局：{类型}";
            Debug.Log($"[结局] {类型}");
            // TODO: 各结局专属演出（花屏/崩坏/音乐/彩蛋动画）
            // TODO: 完美通关(结局5)——机关操作阶段全部机关成功时触发
        }
    }
}
