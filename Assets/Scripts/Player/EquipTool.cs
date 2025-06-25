using Core.ResourceObjectService;
using UnityEngine;

namespace Player
{
    public class EquipTool : Equip
    {
        public float attackRate;
        public float attackDistance;
        public bool attacking;
        
        [Header("Combat")]
        public bool doesDealDamage;
        public int damage;
        
        [Header("Gathering")]
        public bool doesGathering;
        public int gathering;

        //components
        private Animator _animator;
        private Camera _camera;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _camera = Camera.main;
        }

        public override void OnAttackInput()
        {
            if (attacking) return;
            
            attacking = true;
            _animator.SetTrigger("Attack");
            Invoke(nameof(OnCanAttack), attackRate);
        }

        private void OnCanAttack()
        {
            attacking = false;
        }

        public void OnHit()
        {
            if (doesGathering)
            {
                var ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                if (Physics.Raycast(ray, out var hit, attackDistance))
                {
                    var collectableObject = hit.collider?.gameObject.GetComponent<IGatherable>();
                    if (collectableObject is null) return;
                    collectableObject.TakeGathering(gathering);
                    Debug.Log("Gathering something");
                }
                else
                {
                    Debug.Log("Missing Gathering");
                }
            }
        }
    }
}