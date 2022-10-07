using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lym
{
    public abstract class View : MonoBehaviour
    {
        [SerializeField]
        public GameObject[] uiObject;

        public virtual void Init()
        {
            foreach(GameObject go in uiObject)
            {
                go.SetActive(true);
            }
        }

        public virtual void Deinit()
        {
            foreach(GameObject go in uiObject)
            {
                go.SetActive(false);
            }
        }
    }

}
