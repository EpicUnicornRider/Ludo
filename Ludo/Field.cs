using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo
{
    class Field
    {
        internal GameColor FieldColor { get; set; } = GameColor.None;
        internal List<Token> OccupyTokens = new List<Token>();
    }
}
