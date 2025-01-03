﻿using System.Numerics;

public static class MatrixHelper
{
    public static readonly Vector2 Up = new(0, -1);
    public static readonly Vector2 Down = new(0, 1);
    public static readonly Vector2 Left = new(-1, 0);
    public static readonly Vector2 Right = new(1, 0);
    public static readonly Vector2 UpperLeft = new(-1, -1);
    public static readonly Vector2 UpperRight = new(1, -1);
    public static readonly Vector2 BottomLeft = new(-1, 1);
    public static readonly Vector2 BottomRight = new(1, 1);
    
    public static Vector2[] FourDirections => [Up, Down, Right, Left];
    
    public static Vector2 At(this Vector2 position, int distance) => position * new Vector2(distance, distance);
    
    public static Vector2 TurnRight(this Vector2 direction) => direction switch
    {
        _ when direction == Up => Right,
        _ when direction == Down => Left,
        _ when direction == Right => Down,
        _ when direction == Left => Up,
        _ => throw new NotImplementedException()
    };

    public static Vector2 TurnLeft(this Vector2 direction) => direction switch
    {
        _ when direction == Up => Left,
        _ when direction == Down => Right,
        _ when direction == Right => Up,
        _ when direction == Left => Down,
        _ => throw new NotImplementedException()
    };
}
