

using UnityEngine;
using System;
using System.Collections.Generic;


namespace Coroutine{

    public class CoroutineMgr : MonoBeahavior{
        
        private Pool m_pool = new Pool();

        public void StartCoroutine<T>() where T: IEnumerator{

            this.m_pool.StartCoroutine<T>();
        }


        void LateUpdate(){
            
            Coroutine coroutine = this.m_pool.GetCurrentCoroutine();
            if (coroutine == null) return;
            if (!coroutine.MoveNext()){
                this.m_pool.PopCoroutine();
            }
        }
    }
}