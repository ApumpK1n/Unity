

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Coroutine{

    public class CoroutineMgr
    {
        
        public static CoroutineMgr Instance = null;

        public static CoroutineMgr GetInstance(){
            if (Instance == null){
                Instance = new CoroutineMgr();
            }
            return Instance;
        }

        private Pool m_pool = new Pool();

        public Coroutine StartCoroutine(IEnumerator iter){

            return this.m_pool.StartCoroutine(iter);
        }


        public void LateUpdate(){
            
            Coroutine coroutine = this.m_pool.GetCurrentCoroutine();
            if (coroutine == null) return;
            if (!coroutine.MoveNext()){
                this.m_pool.PopCoroutine();
            }
        }
    }
}