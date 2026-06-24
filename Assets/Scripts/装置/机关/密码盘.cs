using UnityEngine;

namespace 装置
{
    // 密码盘（点击类，文档：稳定第一个出现）：输入数字组合，提交→定曲风 / 199876彩蛋
    public class 密码盘 : 机关
    {
        public string 正确曲风输入 = "123";   // 占位：正确曲风对应数字
        public string 彩蛋输入 = "199876";

        string 输入;

        protected override void Start()
        {
            激活阶段 = 游戏阶段.密码盘;
            base.Start();
        }

        protected override void On激活()
        {
            输入 = "";
            Debug.Log("[密码盘] 激活，输入数字选择曲风");
        }

        // 骨架测试用：数字键输入，回车提交（文档为鼠标点击，后续做UI）
        void Update()
        {
            if (!已激活) return;
            for (int i = 0; i <= 9; i++)
                if (Input.GetKeyDown(KeyCode.Alpha0 + i)) 输入数字(i.ToString());
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) 提交();
        }

        // 外部（UI按钮/键盘）调用追加数字
        public void 输入数字(string d)
        {
            if (!已激活) return;
            输入 += d;
            Debug.Log($"[密码盘] 当前输入：{输入}");
        }

        // 提交判定
        public void 提交()
        {
            if (!已激活) return;

            if (输入 == 彩蛋输入)
            {
                装置.main.请求结局(结局类型.彩蛋199876);
                return;
            }
            if (输入 == 正确曲风输入)
            {
                判定成功();
                装置.main.进入阶段(游戏阶段.曲风确定);
                return;
            }

            // 文档：没有则清除全部重新输入；失败多次不进结局
            Debug.Log($"[密码盘] 输入「{输入}」无效，清空重输（正确：{正确曲风输入}）");
            输入 = "";
            判定失败();
        }
    }
}
