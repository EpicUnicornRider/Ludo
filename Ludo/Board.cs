using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo
{
    class Board
    {
        internal List<Field> AllFields = new List<Field>();

        public Board()
        {
            for(int i = 0; i <= 51; i++)
            {
                AllFields.Add(new Field());
            }

        }
        

    }

}

