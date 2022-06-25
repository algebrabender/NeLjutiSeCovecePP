using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pawn
{
    [SerializeField]
    public Sprite typeSprite; //heart, diamond, leaf, clover
    [SerializeField]
    public int Number { get; set; } //1, 2, 3, 4.
    [SerializeField]
    public bool Out { get; set; }
    [SerializeField]
    public int StartNumOfLives { get; set; }
    [SerializeField]
    public int NumOfLivesLeft { get; set; } // depends on difficulty, if == 0 -> out = false
    [SerializeField]
    public string Color { get; set; }
    [SerializeField]
    public Vector2 StartPosition { get; set; }
    [SerializeField]
    public Vector2 Position { get; set; }
    [SerializeField]
    public int Spot { get; set; }
    [SerializeField]
    public int SpotIfFromPlayer { get; set; }

    internal void OutedPawn(float x, float y)
    {
        Position = new Vector2(x, y);
        Out = true;
    }

    internal bool UpdateLives()
    {
        NumOfLivesLeft--;

        if (NumOfLivesLeft <= 0)
        {
            Position = StartPosition;
            Spot = 0;
            Out = false;
            NumOfLivesLeft = StartNumOfLives;
            return true;
        }

        return false;
    }

    internal void UpdatePosition(float x, float y, int newSpot, int addToSpot = 0)
    {
        Position = new Vector2(Position.x + x, Position.y + y);
        Spot = newSpot;
        if (SpotIfFromPlayer + addToSpot > 54)
            SpotIfFromPlayer = SpotIfFromPlayer + addToSpot - 56;
        else
            SpotIfFromPlayer += addToSpot;
    }
}
