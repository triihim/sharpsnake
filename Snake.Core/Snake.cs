using System.Collections.Generic;
using System.Linq;
using Snake.Common;

namespace Snake.Core
{
    internal class Snake
    {
        private const int startingLength = 3;
        private Direction movementDirection = Direction.Up;
        private bool shouldGrowOnNextCalculation = false;

        public List<Coordinate> Positions { get; private set; }

        public Direction MovementDirection 
        {
            get 
            { 
                return movementDirection; 
            }
            set 
            { 
                if(IsNotTurningOnSelf(value))
                {
                    movementDirection = value;
                }
            }
        }

        private bool IsNotTurningOnSelf(Direction direction)
        {
            Coordinate target = GetHeadPosition().GetAdjacent(direction);
            Coordinate firstBodyPartFromHead = Positions[1];
            if (firstBodyPartFromHead.Equals(target))
            {
                return false;
            }
            return true;
        }

        public Snake(Coordinate startingLocation)
        {
            Positions = new List<Coordinate>();
            Positions.Add(startingLocation);

            for (int i = 0; i < startingLength - 1; i++)
            {
                Positions.Add(Positions.Last().GetAdjacent(Direction.Down));
            }

        }

        public Coordinate GetHeadPosition()
        {
            return Positions.First();
        }

        public void Move()
        {
            Positions = CalculateNewPositions();
        }

        public void Grow()
        {
            shouldGrowOnNextCalculation = true;
        }

        private List<Coordinate> CalculateNewPositions()
        {
            List<Coordinate> newPositions = new List<Coordinate>(Positions.Count);

            Coordinate newHeadPosition = CalculateNewHeadPosition();
            List<Coordinate> newBodyPositions = CalculateNewBodyPositions();

            newPositions.Add(newHeadPosition);
            newPositions.AddRange(newBodyPositions);

            return newPositions;
        }

        private List<Coordinate> CalculateNewBodyPositions()
        {
            List<Coordinate> newBodyPositions = new List<Coordinate>();
            
            Coordinate previousPosition = Positions.First();
            foreach (Coordinate bodyPartPosition in GetBodyPositions())
            {
                newBodyPositions.Add(previousPosition);
                previousPosition = bodyPartPosition;
            }

            if(shouldGrowOnNextCalculation)
            {
                newBodyPositions.Add(previousPosition);
                shouldGrowOnNextCalculation = false;
            }

            return newBodyPositions;
        }

        private List<Coordinate> GetBodyPositions()
        {
            int head = 1;
            return Positions.Skip(head).ToList();
        }

        private Coordinate CalculateNewHeadPosition()
        {
            Coordinate oldHeadPosition = Positions.First();
            Coordinate newHeadPosition = oldHeadPosition.GetAdjacent(MovementDirection);
            return newHeadPosition;
        }
    }
}
