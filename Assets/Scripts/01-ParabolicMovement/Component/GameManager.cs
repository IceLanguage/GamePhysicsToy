using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinHowe_GamePhysics.ParabolicMovement
{
    public class GameManager : MonoBehaviour
    {
        public GameObject DaggerPrefab;
        public GameObject DaggerAIm;
        [ContextMenu("随机投掷匕首")]
        public void RandomThrowingDagger()
        {
            
            GameObject dagger = Instantiate(DaggerPrefab);
            dagger.GetComponent<DaggerController>().RandomThrowingDagger();

        }

        [ContextMenu("向随机目标投掷匕首")]
        public void ThrowingDaggerForAim()
        {
            //随机初始化匕首目标高度
            Vector3 aimv = DaggerAIm.transform.position;
            aimv = new Vector3(aimv.x, Random.Range(1f, 3f), aimv.z);
            DaggerAIm.transform.position = aimv;

            
            GameObject dagger = Instantiate(DaggerPrefab);
            dagger.GetComponent<DaggerController>().ThrowingDaggerForAim(aimv,Random.Range(10,100f));
        }
    }
}