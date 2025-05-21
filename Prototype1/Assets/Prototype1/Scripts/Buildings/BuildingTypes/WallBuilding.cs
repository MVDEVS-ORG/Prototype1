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

        private bool isDoorOpen = false;
        private bool isAnimatingDoor = false;
        private Vector3 doorClosedPos;
        private Vector3 doorOpenPos;

        private void Start()
        {
            SetInitialValues();

            if (door != null)
            {
                doorClosedPos = door.transform.position;
                doorOpenPos = doorClosedPos + new Vector3(0f, 3f, 0f); // Door opens upward by 3 units
            }
        }

        void SetInitialValues()
        {
            BuildCost = 7;
            UpgradeCost = 10;
            CanBeUpgraded = true;
            isDoorOpen = false;
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

        public override void TakeDamage(int damage)
        {
            if (State != BuildingState.Completed) return;

            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                State = BuildingState.Ruined;

                foreach (GameObject wall in Walls)
                {
                    wall.GetComponent<Renderer>().material.color = Color.gray;
                }

                Debug.Log("Wall destroyed!");
            }
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
            isAnimatingDoor = false;
        }
    }
}
