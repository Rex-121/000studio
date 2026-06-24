using UnityEngine;

namespace 装置
{
    // 中心按钮，空格触发（文档：主按钮按键类，按空格）
    public class 主按钮 : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                装置.main.点击主按钮();
        }
    }
}
