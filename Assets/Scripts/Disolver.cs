using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Disolver : MonoBehaviour
    {
        [SerializeField] private float m_disolveTime = 1.0f;

        private MaterialPropertyBlock m_PropertyBlock;
        private Renderer myRenderer;

        private void Awake()
        {
            myRenderer = GetComponentInChildren<Renderer>();
            m_PropertyBlock = new MaterialPropertyBlock();

            ActionTweenProperty p = new ActionTweenProperty(1, -1, (val) =>
            {
                setDisolve(val);
                Debug.Log(val);
            });
            Go.to(this, m_disolveTime, new GoTweenConfig().addTweenProperty(p));
        }

        private void setDisolve(float amount)
        {
            m_PropertyBlock.SetFloat("Vector1_DisolveAmt", amount);
            for (int i = 0; i < myRenderer.materials.Length; i++)
            {
                myRenderer.SetPropertyBlock(m_PropertyBlock, i);
            }
        }
    }
}
