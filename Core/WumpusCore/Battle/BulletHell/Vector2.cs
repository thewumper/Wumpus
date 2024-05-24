namespace WumpusCore.Battle.BulletHell
{
    /// <summary>
    /// A position in 2 dimensional space
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// A Vector where all components are zero
        /// </summary>
        public static readonly Vector2 Zero = new Vector2(0, 0);
        
        /// <summary>
        /// The vector's X coordinate
        /// </summary>
        public double x;
        
        /// <summary>
        /// The vector's y coordinate
        /// </summary>
        public double y;

        /// <summary>
        /// The opposite of this vector
        /// </summary>
        public Vector2 Inverse
        {
            get
            {
                return new Vector2(-x, -y);
            }
        }

        /// <summary>
        /// Creates a vector
        /// </summary>
        /// <param name="x">The x component of the vector</param>
        /// <param name="y">The y component of the vector</param>
        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Adds a vector's value to this vector
        /// </summary>
        /// <param name="additive">The vector to add to this vector</param>
        public void Increment(Vector2 additive)
        {
            this.x += additive.x;
            this.y += additive.y;
        }

        /// <summary>
        /// Scales the vector up or down by a factor
        /// </summary>
        /// <param name="scalar">The amount to scale this vector by</param>
        public void Scale(double scalar)
        {
            this.x *= scalar;
            this.y *= scalar;
        }

        /// <summary>
        /// Clones this vector
        /// </summary>
        /// <returns>A vector with the same data as this vector</returns>
        public Vector2 Clone()
        {
            return new Vector2(x, y);
        }
    }
}