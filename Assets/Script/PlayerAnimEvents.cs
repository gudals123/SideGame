using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    private Player player;


    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    private void AnimationOnTrigger ()
    {
        player.AttackOver();
    }



}
