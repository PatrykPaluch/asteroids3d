using UnityEngine;

namespace Asteroids3D  {
    public class Generator : MonoBehaviour {

        [SerializeField] private int poolSize = 1000;

        public Vector3 startSize;

        public Bounds[] safeAreaArray;

        public GameObject asteroidPrefab;

        private ChaoticPool<Rigidbody> asteroidPool;

        void Start() {
            if (asteroidPrefab == null || asteroidPrefab.GetComponent<Rigidbody>() == null) {
                Debug.LogError("asteroid prefab is null or has no Rigidbody", this);
                Destroy(this);
                return;
            }

            Vector3 min = - startSize / 2;
            Vector3 max = startSize / 2;
            asteroidPool = new ChaoticPool<Rigidbody>(poolSize, () => {
                Vector3 randomPosition;
                do {
                    randomPosition = RandomVector(min, max);
                } while (IsInSafeArea(randomPosition)); //do poprawy


                GameObject ob = Instantiate(asteroidPrefab,
                    randomPosition,
                    Quaternion.LookRotation(RandomVector(-1, 1)),
                    transform);
                ob.SetActive(true);
                return ob.GetComponent<Rigidbody>();
            });

        }

        /*
         
         +----------+ safeArea
         |        / |
         |      /   |
         |    *     |  center
         |  /       |
         |/         |
         +----------+
         
         */
        private bool IsInSafeArea(Vector3 pos) {
            foreach (Bounds safeArea in safeAreaArray) {
                if (safeArea.Contains(pos))
                    return true;
            }

            return false;
        }
        // private bool IsInSafeArea(Vector3 pos)
        // {
        //     return pos.x < center.x + safeArea.x / 2 && pos.x > center.x - safeArea.x / 2 &&
        //            pos.y < center.y + safeArea.y / 2 && pos.y > center.y - safeArea.y / 2 &&
        //            pos.z < center.z + safeArea.z / 2 && pos.z > center.z - safeArea.z / 2;
        // }

        private Vector3 RandomVector(float min, float max)
        {
            return RandomVector(Vector3.one * min, Vector3.one * max);
        }

        private Vector3 RandomVector(Vector3 min, Vector3 max)
        {
            return new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)
            );
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Vector3.zero, 0.25f);
            Gizmos.DrawWireCube(Vector3.zero, startSize);
            foreach (Bounds safeArea in safeAreaArray) {
                Gizmos.DrawWireCube(safeArea.center, safeArea.size);
            }
        }
    }
}
