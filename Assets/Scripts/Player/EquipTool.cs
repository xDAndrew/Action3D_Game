using System;
using UnityEngine;

namespace Player
{
    public class EquipTool : Equip
    {
        public float AttackRate;
        public float AttackDistance;
        public bool Attacking;
        
        [Header("Combat")]
        public bool doesDealDamage;
        public int damage;
        
        [Header("Gathering")]
        public bool doesGathering;
        public int gathering;

        //components
        private Animator animator;
        private Camera camera;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            camera = Camera.main;
        }

        public override void OnAttackInput()
        {
            if (!Attacking)
            {
                Attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", AttackRate);
            }
        }

        void OnCanAttack()
        {
            Attacking = false;
        }

        public void OnHit()
        {
            Debug.Log("OnHit");
        }
    }
}