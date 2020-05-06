

using System;
using System.Collections.Generic;

namespace Coroutine{

    public class Pool{

        private Stack<Coroutine> m_Coroutines = new Stack<Coroutine>();


        public void StartCoroutine<T>() where T: IEnumerator{
            Coroutine coroutine = new Coroutine(T);
            this.m_Coroutines.Push(coroutine);
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