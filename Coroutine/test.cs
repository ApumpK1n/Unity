

namespace Coroutine{

    public class Test{
        IEnumerator TestNormalCoroutine()
        {
            Log("NormalCoroutine 1");
            yield return null;
            Log("NormalCoroutine 2");
            yield return 1;
            Log("NormalCoroutine 3");
            yield break;
            Log("NormalCoroutine 4");
        }

    }
    
}



