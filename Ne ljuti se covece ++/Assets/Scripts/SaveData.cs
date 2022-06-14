using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class SaveData
    {
        public List<int> controlledPawnsNumOfLives = new List<int>();
        public List<int> AIPawnsNumOfLives = new List<int>();

        public List<int> controlledPawnsSpot = new List<int>();
        public List<int> AIPawnsSpot = new List<int>();
        public List<int> AIPawnsSpotIfFromPlayer = new List<int>();

        public List<Vector2> controlledPawnsPositions = new List<Vector2>();
        public List<Vector2> AIPawnsPositions = new List<Vector2>();

        public List<Vector2> controlledPawnsStartPositions = new List<Vector2>();
        public List<Vector2> AIPawnsStartPositions = new List<Vector2>();

        public List<bool> controlledPawnsOut = new List<bool>();
        public List<bool> AIPawnsOut = new List<bool>();
    }
}
