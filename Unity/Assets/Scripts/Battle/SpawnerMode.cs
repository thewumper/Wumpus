using UnityEngine;

namespace WumpusUnity.Battle
{
    public class SpawnerMode : MonoBehaviour
    {
        protected GameObject Room;
        protected Rigidbody2D Rigidbody;
        protected GameObject Prefab;
        [SerializeField] public float duration;

        public void Initialize(GameObject room, Rigidbody2D rigidbody, GameObject bullet)
        {
            this.Room = room;
            this.Rigidbody = rigidbody;
            this.Prefab = bullet;
            this.enabled = false;
        }
    }
}