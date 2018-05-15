using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo
{
    public class Player
    {
        private readonly int playerId;
        private readonly string name;
        private readonly Token[] tokens;

        // Player Constructor
        public Player(int id, string playerName, Token[] tokens)
        {
            this.playerId = id;
            this.name = playerName;
            this.tokens = tokens;
            this.Color = this.tokens[0].GetColor();
        }

        //Gets a name
        public string GetName
        {
            get
            {
                return this.name;
            }
        }

        //Gets a color
        public GameColor Color
        {
            get;
        }
        
        
        public int GetPlayerId()
        {
            return this.playerId;
        }

        //Creates a description of the player
        public string GetDescription()
        {
            return this.GetName + " is player number: " + this.GetPlayerId() + " and is the color " + this.Color;
        }

        //Gets Tokens
        public Token[] GetTokens()
        {
            return this.tokens;
        }
    }
}
