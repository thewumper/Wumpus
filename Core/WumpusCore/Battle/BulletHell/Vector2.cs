namespace WumpusCore.Battle.BulletHell
{
    /// <summary>
    /// A position in 2 dimensional space
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// The vector's X coordinate
        /// </summary>
        public double X;
        
        /// <summary>
        /// The vector's y coordinate
        /// </summary>
        public double Y;

        /// <summary>
        /// The opposite of this vector
        /// </summary>
        public Vector2 Inverse
        {
            get
            {
                return new Vector2(-X, -Y);
            }
        }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Adds a vector's value to this vector
        /// </summary>
        /// <param name="additive">The vector to add to this vector</param>
        public void Increment(Vector2 additive)
        {
            this.X += additive.X;
            this.Y += additive.Y;
        }

        /// <summary>
        /// Scales the vector up or down by a factor
        /// </summary>
        /// <param name="scalar">The amount to scale this vector by</param>
        public void Scale(double scalar)
        {
            this.X *= scalar;
            this.Y *= scalar;
        }

        /// <summary>
        /// Clones this vector
        /// </summary>
        /// <returns>A vector with the same data as this vector</returns>
        public Vector2 Clone()
        {
            return new Vector2(X, Y);
        }
    }
}