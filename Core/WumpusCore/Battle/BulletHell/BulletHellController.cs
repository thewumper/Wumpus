using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WumpusCore.Battle.BulletHell
{
    public class BulletHellController
    {
        /// <summary>
        /// All currently present bullets
        /// </summary>
        public List<Bullet> Bullets { get; private set; }

        /// <summary>
        /// All currently present bullet spawners
        /// </summary>
        public List<ISpawner> Spawners { get; private set; }
        
        /// <summary>
        /// The effects that should be displayed
        /// </summary>
        public Stack<IEntity> Effects { get; private set; }

        private Stopwatch previousTick;

        public BulletHellController()
        {
            Bullets = new List<Bullet>();
            previousTick = new Stopwatch();
            previousTick.Start();
        }

        private void AddBullet(Bullet bullet)
        {
            Bullets.Add(bullet);
        }

        public void AddSpawner(ISpawner spawner)
        {
            Spawners.Add(spawner);
        }

        /// <summary>
        /// Advances the game in time
        /// Should be called before bullets are instantiated
        /// </summary>
        /// <param name="seconds">The number of seconds since the last game tick</param>
        public void Tick(double seconds)
        {
            foreach (Bullet bullet in Bullets)
            {
                bullet.Tick(seconds);
            }

            foreach (ISpawner spawner in Spawners)
            {
                spawner.Tick(seconds);
            }
        }

        /// <summary>
        /// Advances the game in time, based on how long it has been since the previous tick.
        /// </summary>
        public void Tick()
        {
            double seconds = previousTick.Elapsed.Ticks / 10000000.0;
            previousTick.Restart();
            Tick(seconds);
        }
    }
}