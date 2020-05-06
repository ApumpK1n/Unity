

namespace Coroutine{

    public class Coroutine{


        private IEnumerator m_iter = null;
        public Coroutine(IEnumerator iter){
            this.m_iter = iter;
        }

        public bool MoveNext(){
            if (this.iter == null) return false;
            bool result = this.m_iter.MoveNext();
            return result;
        }
    }
}