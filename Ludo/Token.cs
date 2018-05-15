using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo
{
    public enum TokenState { Home, InPlay, Safe, HomeStretch, Finished };

    public class Token
    {
        private int tokenId;
        private GameColor color;
        private TokenState state;

        // token constructor
        public Token(int id, GameColor clr)
        {
            this.tokenId = id;
            this.color = clr;
            this.state = TokenState.Home;
        }

        public int GetTokenId()
        {
            return this.tokenId;
        }

        public GameColor GetColor()
        {
            return this.color;
        }

        public TokenState GetState()
        {
            return this.state;
        }
    }
}