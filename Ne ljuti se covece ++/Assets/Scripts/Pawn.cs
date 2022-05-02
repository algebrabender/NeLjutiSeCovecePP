using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn
{
    public int Number { get; set; } //1, 2, 3, 4
    public bool Eaten { get; set; }
    public int NumOfLivesLeft { get; set; } // depends on difficulty, if == 0 -> eaten = true
    public string Color { get; set; }

    internal void UpdateLives()
    {
        NumOfLivesLeft--;

        if (NumOfLivesLeft <= 0)
            Eaten = true;
    }
}
