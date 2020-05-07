using System.Collections;
using System.Collections.Generic;


namespace Coroutine
{
    public class WaitForSeconds
    {
        private float m_elapsedTime;
        private float m_endTime;

        public WaitForSeconds(float delay)
        {
            m_elapsedTime = 0;
            m_endTime = delay;
        }

        public bool IsTickOver(float deltaTime) // 增量时间，及每帧执行时间
        {
            m_elapsedTime += deltaTime;
            return m_elapsedTime > m_endTime;
        }


        public void Reset()
        {
            m_elapsedTime = 0;
        }
    }
}


