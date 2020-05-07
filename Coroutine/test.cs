
using UnityEngine;
using System.Collections;

namespace Coroutine{

    public class Test: MonoBehaviour
    {

        void Start()
        {
            CoroutineMgr.GetInstance().StartCoroutine(TestNesting());
            //StartCoroutine(TestUnityNesting());
        }

        void LateUpdate()
        {
            CoroutineMgr.GetInstance().LateUpdate();
        }


        IEnumerator TestNormalCoroutine()
        {
            Debug.Log("NormalCoroutine 1");
            yield return null;
            Debug.Log("NormalCoroutine 2");
            yield return 1;
            Debug.Log("NormalCoroutine 3");
            yield break;
            Debug.Log("NormalCoroutine 4");
        }

        IEnumerator TestWaitFor()
        {
            Debug.Log("WaitForSeconds 1");
            yield return new WaitForSeconds(1.5f);
            Debug.Log("WaitForSeconds 2");
        }

        IEnumerator TestUnityWaitFor()
        {
            Debug.Log("WaitForSeconds 1");
            yield return new UnityEngine.WaitForSeconds(1.5f);
            Debug.Log("WaitForSeconds 2");
        }

        IEnumerator TestNestNesting()
        {
            Debug.Log("TestNestNesting 1");
            yield return CoroutineMgr.GetInstance().StartCoroutine(TestNormalCoroutine());
            Debug.Log("TestNestNesting 2");
        }

        IEnumerator TestUnityNestNesting()
        {
            Debug.Log("TestUnityNestNesting 1");
            yield return StartCoroutine(TestNormalCoroutine());
            Debug.Log("TestUnityNestNesting 2");
        }


        IEnumerator TestNesting()
        {
            Debug.Log("TestNesting 1");
            yield return new WaitForSeconds(2.5f);
            Debug.Log("TestNesting 2");
            yield return CoroutineMgr.GetInstance().StartCoroutine(TestNormalCoroutine());
            Debug.Log("TestNesting 3");
            yield return CoroutineMgr.GetInstance().StartCoroutine(TestWaitFor());
            Debug.Log("TestNesting 4");
            yield return CoroutineMgr.GetInstance().StartCoroutine(TestNestNesting());
            Debug.Log("TestNesting 5");
        }

        IEnumerator TestUnityNesting()
        {
            Debug.Log("TestNesting 1");
            yield return new UnityEngine.WaitForSeconds(2.5f);
            Debug.Log("TestNesting 2");
            yield return StartCoroutine(TestNormalCoroutine()); // Unity协程执行规律，碰到协程嵌套，先执行完新的协程，再执行就协程，满足栈的规律。
            Debug.Log("TestNesting 3");
            yield return StartCoroutine(TestUnityWaitFor());
            Debug.Log("TestNesting 4");
            yield return StartCoroutine(TestUnityNestNesting());
            Debug.Log("TestNesting 5");
        }
    }
    
}



