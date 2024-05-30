using UnityEngine;

namespace WumpusUnity.Battle
{
    public class SpawnerMode : MonoBehaviour
    {
        protected GameObject Room;
        protected new Rigidbody2D Rigidbody;
        protected GameObject Prefab;

        public void Initialize(GameObject room, Rigidbody2D rigidbody, GameObject bullet)
        {
            this.Room = room;
            this.Rigidbody = rigidbody;
            this.Prefab = bullet;
        }
    }
}