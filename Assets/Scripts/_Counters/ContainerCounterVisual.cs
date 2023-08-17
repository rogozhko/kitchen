using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour {

    private const string OPEN_CLOSED = "OpenClose";
    
    [SerializeField] private ContainerCounter containerCounter;
    
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        containerCounter.OnPlayerGrabbedObject += (object sender, EventArgs e) => {
            animator.SetTrigger(OPEN_CLOSED);
        };
    }
}
