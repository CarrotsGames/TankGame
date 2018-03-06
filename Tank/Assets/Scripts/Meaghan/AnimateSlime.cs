using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class AnimateSlime : MonoBehaviour
{
    //Variables
    private Animator anim;
    int shootHash = Animator.StringToHash("Shoot");
    int runStateHash = Animator.StringToHash("Base Layer.Run");

    [Header("Select Player Number")]
    [SerializeField]
    private XboxController controller;





    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveVertical = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float moveHorizontal = XCI.GetAxis(XboxAxis.LeftStickY, controller);
        anim.SetFloat("Speed", moveHorizontal);
        anim.SetFloat("Speed", moveVertical);


        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (XCI.GetButton(XboxButton.A, controller) && stateInfo.fullPathHash == runStateHash)
        {
            anim.SetTrigger(shootHash);
        }
    }
}

