
using System;
using System.Collections;
using UnityEngine;

namespace Coroutine{

    public class Coroutine{

        private IEnumerator m_iter = null;
        
        public Coroutine(IEnumerator iter){
            this.m_iter = iter;
        }

        public bool MoveNext(){
            if (this.m_iter == null) return false;
            bool bIsOver = true;
            bool result = true;
            if (this.m_iter.Current is WaitForSeconds)
            {
                WaitForSeconds wait = this.m_iter.Current as WaitForSeconds;
                bIsOver = wait.IsTickOver(Time.deltaTime);
            }
            if (bIsOver)
            {
                result = this.m_iter.MoveNext();
            }
            
            return result;
        }
    }
}