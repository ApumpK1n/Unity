

using System;
using System.Collections;
using System.Collections.Generic;

namespace Coroutine{

    public class Pool{

        private Stack<Coroutine> m_Coroutines = new Stack<Coroutine>();


        public Coroutine StartCoroutine(IEnumerator iter){
            Coroutine coroutine = new Coroutine(iter);
            this.m_Coroutines.Push(coroutine);
            return coroutine;
        }


        public void PopCoroutine(){
            if (this.m_Coroutines.Count > 0){
                this.m_Coroutines.Pop();
            }
        }

        public Coroutine GetCurrentCoroutine(){
            if (this.m_Coroutines.Count > 0)
            {
                return this.m_Coroutines.Peek();
            }
            return null;
        }
    }
}