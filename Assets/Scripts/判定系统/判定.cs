using Tyrant;
using UnityEngine;

namespace 判定系统
{
    [CreateAssetMenu(fileName = "判定", menuName = "Game/判定系统")]
    public class 判定 : SingletonSO<判定>
    {
        
        public 判定等级[] 判定等级;

        public 判定等级 miss;

        public static 判定等级 抉择(float reactionMs)
        {

            foreach (var 等级 in main.判定等级)
            {
                if (!等级.判定(reactionMs)) continue;
                return 等级;
            }

            return main.miss;
        }

    }
}
