namespace WumpusCore.Topology
{
    /// <summary>
    /// A hexagon in infinite hexgrid-space.
    /// Rows alternate between slightly higher and slightly lower members.
    /// Hexagons along a column fall in a line, hexagons along a row do not.
    /// Row numbers increase going south.
    /// Column numbers increase going east.
    /// Hexagon (0,0) lies at a northern hexagon in its row
    /// </summary>
    public class Hexagon
    {
        /// <summary>
        /// Which horizontal row is this hexagon located in?
        /// </summary>
        public readonly int row;
        
        /// <summary>
        /// Which vertical column is this hexagon located in?
        /// </summary>
        public readonly int column;
        
        /// <summary>
        /// Is this hexagon one of the northern members of the row?
        /// </summary>
        public bool isNorthern {
            get
            {
                // Fix negative modulus
                if (column < 0)
                {
                    return -column % 2 == 0;
                }
                return (column % 2) == 0;
            }
        }

        public Hexagon(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            Hexagon other = (Hexagon)obj;
            return row == other.row && column == other.column;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (row * 397) ^ column;
            }
        }

        /// <summary>
        /// Get the hexagon in a given direction from this.
        /// </summary>
        /// <param name="direction">The direction to find the hexagon</param>
        /// <returns>The hexagon that borders this hexagon in the given direction</returns>
        public Hexagon GetFromDirection(Directions direction)
        {
            // Same no matter what
            switch (direction)
            {
                case Directions.North:
                    return new Hexagon(row - 1, column);
                case Directions.South:
                    return new Hexagon(row + 1, column);
            }

            int newRow = row;
            int newColumn = column;

            // Assume at north of row
            switch (direction)
            {
                case Directions.NorthEast:
                    newRow = row - 1;
                    newColumn = column + 1;
                    break;
                case Directions.SouthEast:
                    newColumn = column + 1;
                    break;
                case Directions.SouthWest:
                    newColumn = column - 1;
                    break;
                case Directions.NorthWest:
                    newRow = row - 1;
                    newColumn = column - 1;
                    break;
            }

            // Change functionality if southern
            if (!isNorthern)
            {
                newRow++;
            }

            return new Hexagon(newRow, newColumn);
        }
    }
}