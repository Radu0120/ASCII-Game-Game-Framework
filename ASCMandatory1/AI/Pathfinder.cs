using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Pathfinder
    {

        public static Position NextMove(Position target, Actor actor, Map map)
        {
            Position currentposition = new Position(actor.Position.X, actor.Position.Y);
            Position start = new Position(actor.Position.X, actor.Position.Y);
            Position finish = new Position(target.X, target.Y);

            List<Position> exploredtiles = new List<Position>();

            return PotentialMoves(start, start, target, map, actor, ref exploredtiles);
        }
        public static Position PotentialMoves(Position start, Position previousposition, Position target, Map map, Actor actor, ref List<Position> exploredtiles)
        {
            if (Position.AreEqual(previousposition, target)) return previousposition;

            List<Position> potentialmoves = new List<Position>();
            potentialmoves.Add(new Position(previousposition.X + 1, previousposition.Y));
            potentialmoves.Add(new Position(previousposition.X - 1, previousposition.Y));
            potentialmoves.Add(new Position(previousposition.X, previousposition.Y + 1));
            potentialmoves.Add(new Position(previousposition.X, previousposition.Y - 1));

            List<Position> validmoves = new List<Position>();
            foreach (Position move in potentialmoves)
            {
                if (exploredtiles.Contains(move)) continue;
                if (Position.AreEqual(start, move))
                {
                    continue;
                }
                if (move.X == target.X && move.Y == target.Y)
                {
                    return move;
                }
                if (move.X > map.Bounds.X - 1 || move.X < 0 || move.Y > map.Bounds.Y - 1 || move.Y < 0) // check the map boundaries
                {
                    continue;
                }
                if (map.GetEntityFromPosition(move) != null) //if there is an entity, check for phase
                {
                    Entity thisentity = (Entity)map.GetEntityFromPosition(move);
                    if (thisentity.Attributes.Contains("Phase") || actor.Attributes.Contains("Phase"))
                    {
                        validmoves.Add(move);
                    }
                    else
                    {
                        continue;
                    }
                }
                else // no entity, can move in
                {
                    validmoves.Add(move);
                }
                exploredtiles.Add(move);
            }

            List<List<Position>> pathlist = new List<List<Position>>();
            foreach (Position move in validmoves)
            {
                List<Position> path = new List<Position>();
                path.Add(move);
                pathlist.Add(path);
            }
            return null;
        }
        public static Position AStar(Actor target, Actor actor)
        {

            var start = new Square();
            start.Y = actor.Position.Y;
            start.X = actor.Position.X;


            var finish = new Square();
            finish.Y = target.Position.Y;
            finish.X = target.Position.X;

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
                    //Console.WriteLine("Retracing steps backwards...");
                    List<Square> tileTree = new List<Square>();
                    while (tile != null)
                    {
                        //Console.WriteLine($"{tile.X} : {tile.Y}");
                        //if (map[tile.Y][tile.X] == ' ')
                        //{
                        //    var newMapRow = map[tile.Y].ToCharArray();
                        //    newMapRow[tile.X] = '*';
                        //    map[tile.Y] = new string(newMapRow);
                        //}

                        tileTree.Add(tile);
                        tile = tile.Parent;

                        //if(tile.Parent == null)
                        //{
                        //    return new Position(tile.X, tile.Y);
                        //}
                        //else
                        //{
                        //    tile = tile.Parent;
                        //}

                        //if (tile == null)
                        //{
                        //    Console.WriteLine("Map looks like :");
                        //    map.ForEach(x => Console.WriteLine(x));
                        //    Console.WriteLine("Done!");
                        //    return;
                        //}
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
