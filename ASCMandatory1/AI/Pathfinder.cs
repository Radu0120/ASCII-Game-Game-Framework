using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Pathfinder
    {
        public static Position Wander(Actor actor)
        {
            if (actor.HasStatusEffectExpired("Waiting"))
            {
                Random random = new Random();
                int X = actor.Position.X + random.Next(-5, 6);
                int Y = actor.Position.Y + random.Next(-5, 6);

                actor.AddStatusEffect("Waiting", random.Next(1000, 5000));

                return new Position(X, Y);
            }
            else
            {
                return null;
            }
            
        }
        public static bool line(Position a, Position b)
        {
            Map map = Level.GetCurrentLevel().GetCurrentMap();


            int w = b.X - a.X;
            int h = b.Y - a.Y;

            int x = a.X;
            int y = a.Y;

            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                
                if (map.GetEntitiesFromPosition(new Position(x, y)).Where(e => e.Attributes.Contains("Solid") && !Position.AreEqual(e.Position, b)).ToList().Count > 0)
                {
                    return false;
                }
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
                //map.AddEntity(Item.itemIndex[1], new Position(x, y));
                //map.DrawMap(false);
                //Thread.Sleep(16);
            }
            return true;
        }
        public static Position AStar(Position target, Actor actor)
        {
            var start = new Square();
            start.Y = actor.Position.Y;
            start.X = actor.Position.X;


            var finish = new Square();
            finish.Y = target.Y;
            finish.X = target.X;

            start.SetDistance(finish.X, finish.Y);

            var activeSquares = new List<Square>();
            activeSquares.Add(start);
            var visitedSquares = new List<Square>();

            while (activeSquares.Any())
            {
                var checkSquare = activeSquares.OrderBy(x => x.CostDistance).First();

                if (checkSquare.X == finish.X && checkSquare.Y == finish.Y)
                {
                    //We found the destination and we can be sure (Because the the OrderBy above)
                    //That it's the most low cost option. 
                    var tile = checkSquare;
                    List<Square> tileTree = new List<Square>();
                    while (tile != null)
                    {
                        tileTree.Add(tile);
                        tile = tile.Parent;
                    }
                    int index = tileTree.Count() - 2;
                    if(index < 0) { index = 0; }
                    return new Position(tileTree[index].X, tileTree[index].Y);
                }

                visitedSquares.Add(checkSquare);
                activeSquares.Remove(checkSquare);

                var walkableSquares = GetWalkableSquares(Level.GetCurrentLevel().GetCurrentMap(), checkSquare, finish, actor);

                foreach (var walkableSquare in walkableSquares)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (visitedSquares.Any(x => x.X == walkableSquare.X && x.Y == walkableSquare.Y))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    if (activeSquares.Any(x => x.X == walkableSquare.X && x.Y == walkableSquare.Y))
                    {
                        var existingSquare = activeSquares.First(x => x.X == walkableSquare.X && x.Y == walkableSquare.Y);
                        if (existingSquare.CostDistance > checkSquare.CostDistance)
                        {
                            activeSquares.Remove(existingSquare);
                            activeSquares.Add(walkableSquare);
                        }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        activeSquares.Add(walkableSquare);
                    }
                }
            }

            //no path found
            return null;
        }

        private static List<Square> GetWalkableSquares(Map map, Square currentSquare, Square targetSquare, Actor actor)
        {
            var possibleSquares = new List<Square>()
            {
                new Square { X = currentSquare.X, Y = currentSquare.Y - 1, Parent = currentSquare, Cost = currentSquare.Cost + 1 },
                new Square { X = currentSquare.X, Y = currentSquare.Y + 1, Parent = currentSquare, Cost = currentSquare.Cost + 1},
                new Square { X = currentSquare.X - 1, Y = currentSquare.Y, Parent = currentSquare, Cost = currentSquare.Cost + 1 },
                new Square { X = currentSquare.X + 1, Y = currentSquare.Y, Parent = currentSquare, Cost = currentSquare.Cost + 1 },
                new Square { X = currentSquare.X + 1, Y = currentSquare.Y + 1, Parent = currentSquare, Cost = currentSquare.Cost + 1 },
                new Square { X = currentSquare.X + 1, Y = currentSquare.Y - 1, Parent = currentSquare, Cost = currentSquare.Cost + 1 },
                new Square { X = currentSquare.X - 1, Y = currentSquare.Y + 1, Parent = currentSquare, Cost = currentSquare.Cost + 1 },
                new Square { X = currentSquare.X - 1, Y = currentSquare.Y - 1, Parent = currentSquare, Cost = currentSquare.Cost + 1 },
            };

            possibleSquares.ForEach(tile => tile.SetDistance(targetSquare.X, targetSquare.Y));

            var maxX = map.Bounds.X;
            var maxY = map.Bounds.Y;

            List<Square> validsquares = new List<Square>();
            foreach (Square move in possibleSquares)
            {
                if (move.X == targetSquare.X && move.Y == targetSquare.Y)
                {
                    validsquares.Add(move);
                }
                if (move.X > map.Bounds.X - 1 || move.X < 0 || move.Y > map.Bounds.Y - 1 || move.Y < 0) // check the map boundaries
                {
                    continue;
                }
                if (map.GetEntityFromPosition(new Position(move.X, move.Y)) != null) //if there is an entity, check for phase
                {
                    Entity thisentity = (Entity)map.GetEntityFromPosition(new Position(move.X, move.Y));
                    if (thisentity.Attributes.Contains("Phase") || actor.Attributes.Contains("Phase"))
                    {
                        validsquares.Add(move);
                    }
                    else
                    {
                        continue;
                    }
                }
                else // no entity, can move in
                {
                    validsquares.Add(move);
                }
            }
            return validsquares;

        }
    }
    class Square
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Square Parent { get; set; }
        //The distance is essentially the estimated distance, ignoring walls to our target. 
        //So how many tiles left and right, up and down, ignoring walls, to get there. 
        public void SetDistance(int targetX, int targetY)
        {
            this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }
    }
}
