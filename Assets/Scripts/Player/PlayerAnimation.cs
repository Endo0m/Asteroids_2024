using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator engineAnimator;
    private void Update()
    {
        UpdateEngineAnimation();
    }
    private void UpdateEngineAnimation()
    {
        if (engineAnimator != null)
        {
            bool thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            bool boosting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            if (thrusting)
            {
                if (boosting)
                {
                    engineAnimator.SetBool("BoostEngine", true);
                    engineAnimator.SetBool("NormalEngine", false);
                }
                else
                {
                    engineAnimator.SetBool("BoostEngine", false);
                    engineAnimator.SetBool("NormalEngine", true);
                }
            }
            else
            {
                engineAnimator.SetBool("BoostEngine", false);
                engineAnimator.SetBool("NormalEngine", false);
            }
        }
    }
}
