using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Tyrant
{
    public class SingletonSO<T> : SerializedScriptableObject where T : SingletonSO<T>
    {

        private static T _instance;

        public static T main
        {
            get
            {
                if (_instance != null) return _instance;
            
                var sw = new Stopwatch();

                sw.Start();

                var assets = Resources.LoadAll<T>($"Singleton/");

                if (assets == null || assets.Length < 1)
                {
                    throw new System.Exception("#SingletonSO# 没有此单例！");
                }
            
                if (assets.Length > 1)
                {
                    Debug.Log("#SingletonSO# 多个单例");
                }
                
                _instance = assets[0];
                sw.Stop();
                _instance.SingletonInit();
                Debug.Log("#SingletonSO# 单例耗时" + _instance + " " + sw.ElapsedMilliseconds);

                return _instance;
            }
        }

        protected virtual void SingletonInit()
        {
        
        }

    }
}