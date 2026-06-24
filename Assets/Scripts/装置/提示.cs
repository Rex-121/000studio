using TMPro;
using UnityEngine;

namespace 装置
{
    // 左上+左侧提示文本
    public class 提示 : MonoBehaviour
    {
        public TMP_Text 文本;

        public void 设置(string s)
        {
            if (文本 != null) 文本.text = s;
        }
    }
}
