using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn
{
    public Sprite typeSprite; //heart, diamond, leaf, clover
    public int Number { get; set; } //1, 2, 3, 4
    public bool Out { get; set; }
    public bool Eaten { get; set; }
    public int NumOfLivesLeft { get; set; } // depends on difficulty, if == 0 -> eaten = true
    public string Color { get; set; }
    public Vector2 StartPosition { get; set; }
    public Vector2 Position { get; set; }
    public int Spot { get; set; }
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
            Eaten = true;
            Position = StartPosition;
            Spot = 0;
            return true;
        }

        return false;
    }

    internal void UpdatePosition(float x, float y, int newSpot, int addToSpot = 0)
    {
        Position = new Vector2(Position.x + x, Position.y + y);
        Spot = newSpot;
        if (SpotIfFromPlayer + addToSpot > 55)
            SpotIfFromPlayer = addToSpot - 55 - SpotIfFromPlayer;
        else
            SpotIfFromPlayer += addToSpot;
    }
}
