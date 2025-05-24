using prototype1.scripts.systems;
using UnityEngine;

namespace Assets.Prototype1.Scripts.Buildings
{
    public enum WallType
    {
        Wood,
        Stone
    }

    public class WallBuilding : Building
    {
        public WallType CurrentWall = WallType.Wood;
        public GameObject[] Walls;
        public GameObject door;

        [SerializeField] private Collider _doorCollider;
        [SerializeField] private Collider[] _wallColliders;
        private bool isDoorOpen = false;
        private bool isAnimatingDoor = false;
        private Vector3 doorClosedPos;
        private Vector3 doorOpenPos;
        private HealthSystem _selfHealthSystem;

        private void Start()
        {
            _selfHealthSystem = GetComponent<HealthSystem>();
            _selfHealthSystem.OnZeroHealth += Die;
            SetInitialValues();
            foreach (Collider col in _wallColliders)
            {
                col.enabled = false;
            }
            _doorCollider.enabled = false;
            if (door != null)
            {
                doorClosedPos = door.transform.position;
                doorOpenPos = doorClosedPos + new Vector3(0f, 6f, 0f); // Door opens upward by 6 units
            }
        }

        void SetInitialValues()
        {
            BuildCost = 7;
            UpgradeCost = 10;
            CanBeUpgraded = true;
            _doorCollider.enabled = false;
            isDoorOpen = false;
            (_selfHealthSystem as IHealthSystem).ResetHealth();
        }

        public override void Build()
        {
            if (State == BuildingState.Ruined)
            {
                State = BuildingState.Completed;
                currentUpgrade = 1;

                foreach (GameObject wall in Walls)
                {
                    wall.SetActive(true);
                    wall.GetComponent<Renderer>().material.color = Color.blue;
                }
                foreach (Collider col in _wallColliders)
                {
                    col.enabled = true;
                }
                _doorCollider.enabled = true;
                Debug.Log($"{name} built successfully!");
            }
        }

        public override void UpgradeBuilding()
        {
            if (!CanBeUpgraded)
            {
                Debug.Log("Wall is already at max upgrade.");
                return;
            }

            CurrentWall = WallType.Stone;
            Debug.Log("Wall upgraded to Stone.");
            CanBeUpgraded = false;
        }

        void Die()
        {
            State = BuildingState.Ruined;

            foreach (GameObject wall in Walls)
            {
                wall.SetActive(false);
                wall.GetComponent<Renderer>().material.color = Color.gray;
            }
            foreach (Collider col in _wallColliders)
            {
                col.enabled = false;
            }
            _doorCollider.enabled = false;
            Debug.Log("Wall destroyed!");
            _selfHealthSystem.OnZeroHealth -= Die;
        }

        protected override void Update()
        {
            base.Update(); // Handle input only if player nearby

            if (isPlayerNearby && State == BuildingState.Completed)
            {
                if (Input.GetKeyDown(KeyCode.O) && !isAnimatingDoor)
                {
                    StartCoroutine(ToggleDoor());
                }
            }
        }

        private System.Collections.IEnumerator ToggleDoor()
        {
            isAnimatingDoor = true;
            Vector3 start = door.transform.position;
            Vector3 end = isDoorOpen ? doorClosedPos : doorOpenPos;

            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                door.transform.position = Vector3.Lerp(start, end, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            door.transform.position = end;
            isDoorOpen = !isDoorOpen;
            _doorCollider.enabled = !_doorCollider.enabled;
            isAnimatingDoor = false;
        }
    }
}
