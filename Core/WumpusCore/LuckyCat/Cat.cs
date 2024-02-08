using System;

namespace WumpusCore.LuckyCat
{
    public class Cat
    {
        /// <summary>
        /// Is the cat tamed
        /// </summary>
        private bool tamed;

        /// <summary>
        /// The room number that the cat is currently in
        /// </summary>
        private int location;

        /// <summary>
        /// The radius that the cat should be heard from
        /// </summary>
        public const int AudibleMewingRadius = 2;

        /// <summary>
        /// Figure out how many coins it will take to tame the cat
        /// </summary>
        /// <returns>The number of coint to tame the cat</returns>
        public int Tame()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pets the cat
        /// </summary>
        public int Pet()
        {
            throw new NotImplementedException();
        }
    }
}