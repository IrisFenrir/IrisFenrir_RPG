using UnityEngine;

namespace IrisFenrir
{
    public static class ErrorLog
    {
        public static void Log(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#else
            // 其他显示错误的方式
#endif
        }
    }
}
