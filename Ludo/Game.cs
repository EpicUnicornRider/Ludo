using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo
{
    // colors for the tokens and players
    public enum GameColor { Green, Blue, Red, Yellow, None };
    // States for the tokens
    public enum GameState { InPlay, Finished };
    public class Game
    {
        GameState state;
        Player[] players;
        int numberOfPlayers;
        int playerTurn = 1;
        int throws = 0;
        int chosen = 0;
        Token token;
        bool movable = false;
        Board board = new Board();
        Dice dice = new Dice();
        //Game constructor
        public Game()
        {
            Console.WriteLine("Hello and welcome to this Ludo game");
            SetNumberOfPlayers();
            NamePlayers();
            ShowPlayers();
            TakeTurns();
        }
        private void Space()
        {
            Console.WriteLine();
        }
        private void WriteLine(string txt = "", int dl = 0)
        {
            System.Threading.Thread.Sleep(dl);
            Console.WriteLine(txt);
        }
        private void SetNumberOfPlayers()
        {
            Space();
            Console.WriteLine("How many players are playing? (2-4) ");

            while (numberOfPlayers < 2 || numberOfPlayers > 4)
            {
                if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out this.numberOfPlayers))
                {
                    Space();
                    Console.WriteLine("Please pick a whole number between 2 and 4");
                }
            }
        }
        private void NamePlayers()
        {
            Console.Clear();
            this.players = new Player[this.numberOfPlayers];

            for (int i = 0; i < this.numberOfPlayers; i++)
            {
                Console.WriteLine("What is the name of player #" + (i + 1) + "? ");
                string Name = Console.ReadLine();

                Token[] tkns = AssignTokens(i);
                players[i] = new Player((i + 1), Name, tkns);
            }
        }
        private Token[] AssignTokens(int colorIndex)
        {

            Token[] tokens = new Token[4];

            for (int i = 0; i <= 3; i++)
            {
                switch (colorIndex)
                {
                    case 0:
                        tokens[i] = new Token((i + 1), GameColor.Green);
                        break;
                    case 1:
                        tokens[i] = new Token((i + 1), GameColor.Yellow);
                        break;
                    case 2:
                        tokens[i] = new Token((i + 1), GameColor.Blue);
                        break;
                    case 3:
                        tokens[i] = new Token((i + 1), GameColor.Red);
                        break;
                }
            }
            return tokens;
        }
        private void ShowPlayers()
        {
            Console.Clear();
            Console.WriteLine("Here are your players:", 500);
            foreach (Player pl in this.players)
            {
                WriteLine(pl.GetDescription(), 1000);
            }
            WriteLine("", 2000);
        }
        //players thow dice
        private void TakeTurns()
        {

            while (this.state == GameState.InPlay)
            {

                Console.Clear();
                Player myTurn = players[(playerTurn - 1)];
                Console.WriteLine(myTurn.GetName + "'s turn");
                WriteLine("It is " + myTurn.GetDescription() + "'s turn");
                do
                {
                    Console.WriteLine("Press t when you wanna throw your die");
                }
                while (Console.ReadKey().KeyChar != 't');
                WriteLine("Your die landed on: " + dice.ThrowDice().ToString());
                throws++;
                ShowTurnOptions(myTurn.GetTokens(), token);
                break;
            }

        }
        private void AddOnField(Token token)
        {
            var getField = board.AllFields[token.position.Value];
            if (getField.FieldColor == GameColor.None)
            {
                getField.FieldColor = token.GetColor();
                getField.OccupyTokens.Add(token); 
            }
            else if (getField.FieldColor == token.GetColor())
            {
                getField.OccupyTokens.Add(token);
                getField.OccupyTokens[0].state = TokenState.Safe;
                token.state = TokenState.Safe;
            }
            else if (getField.OccupyTokens[0].state == TokenState.Safe)
            {
                WriteLine("You hit a enemy token that is safe. So your token has been moved home");
                token.position = null;
                token.fieldsLeft = 56;
                token.state = TokenState.Home;
            }
            else
            {
                WriteLine("You have hit a enemy token which now has been moved home");
                getField.OccupyTokens[0].fieldsLeft = 56;
                getField.OccupyTokens[0].position = null;
                getField.OccupyTokens[0].state = TokenState.Home;
                getField.OccupyTokens.RemoveAt(0);
                getField.OccupyTokens.Add(token);
                getField.FieldColor = token.GetColor();
            }
        }
        private void RemoveFromField(Token token)
        {
            var getField = board.AllFields[token.position.Value];
            if (token.state == TokenState.Safe)
            {
                if (getField.OccupyTokens.Count > 2)
                {
                    token.state = TokenState.InPlay;
                    getField.OccupyTokens.Remove(token);
                }
                else
                {
                    token.state = TokenState.InPlay;
                    getField.OccupyTokens.Remove(token);
                    getField.OccupyTokens[0].state = TokenState.InPlay;
                }
            }
            else
            {
                getField.OccupyTokens.Remove(token);
                getField.FieldColor = GameColor.None;
            }
        }
        public void ShowTurnOptions(Token[] tokens, Token token)
        {
            int choice = 0;

            WriteLine("Here are your tokens:");
            foreach (Token tk in tokens)
            {

                Console.WriteLine("Token #" + tk.GetTokenId() + ": is placed: " + tk.state, 1000);

                switch (tk.state)
                {
                    case TokenState.Home:
                        if (dice.GetValue() == 6)
                        {
                            Console.WriteLine(" <- Playable. Ready to move out.");
                            choice++;
                        }
                        else
                        {
                            Console.WriteLine(" <- Not playable");
                        }
                        break;
                    case TokenState.InPlay:
                        Console.WriteLine(" <- Playable. This token stands on tile number {0:D} and has {1:D] tiles left.", token.position, token.fieldsLeft);
                        choice++;
                        break;
                    case TokenState.Safe:
                        Console.WriteLine("  <- Playable. This token stands on tile number {0:D} and has {1:D] tiles left.", token.position, token.fieldsLeft);
                        choice++;
                        break;
                    case TokenState.Finished:
                        Console.WriteLine(" <- Not Playable. This token i already done");
                        choice++;
                        break;
                    case TokenState.HomeStretch:
                        Console.WriteLine(" <- Playable. This token only has {0:D} tiles left", token.fieldsLeft);
                        choice++;
                        break;
                }
                WriteLine("");
            }
            WriteLine("");
            WriteLine("You have " + choice.ToString() + " choices this turn", 2000);

            //No choices and rolled 3 times
            if (choice == 0 && throws >= 3)
            {
                this.ChangeTurn();
            }

            //no choices but hasnt rolled 3 times
            if (choice == 0)
            {
                this.TakeTurns();
            }

            //1-4 choices
            else
            {
                
                ChooseToken(tokens);

            }
        }
        private void ChooseToken(Token[] tokens)
        {
            movable = false;
            while (movable == false)
            {
                WriteLine("Choose which token you want to move");

                chosen = Convert.ToInt16(Console.ReadLine());
                token = tokens[chosen - 1];

                if (token.state == TokenState.Home && dice.GetValue() == 6)
                {
                    TokenStart();
                }
                else if (token.state == TokenState.Finished)
                {
                    WriteLine("This token has already finished please choose another one");
                }
                else
                {
                    MoveToken(token);
                }
            }
        }
        private void MoveToken(Token token)
        {
            //moves a token into the homestretch
            if (token.position != null)
            {
                if (token.fieldsLeft - dice.GetValue() < 6)
                {
                    RemoveFromField(token);
                    token.state = TokenState.HomeStretch;
                    token.position = null;                }
            }
            //finishes a token at the end
            if(token.fieldsLeft - dice.GetValue() == 0)
            {
                token.state = TokenState.Finished;
                token.fieldsLeft = 0;
            }
            else if (token.fieldsLeft - dice.GetValue() < 0)
            {
                token.fieldsLeft = ((token.fieldsLeft - dice.GetValue()) * -1);
                
            }
            else
            {
                token.fieldsLeft = token.fieldsLeft - dice.GetValue();
            }
            if (token.position != null)
            {
                RemoveFromField(token);
                if (token.position + dice.GetValue() > 51)
                {
                    for (int i = 0; i < dice.GetValue(); i++)
                    {
                        if (token.position + 1 > 51)
                        {
                            token.position = 0;
                        }
                        else
                        {
                            token.position++;
                        }
                    }
                }
                else
                {
                    token.position = token.position - dice.GetValue();
                }
                AddOnField(token);
            }
            movable = true;
            CheckSix();
        }
        private void TokenStart()
        {
            token.state = TokenState.InPlay;
            
            switch (token.GetColor())
            {
                case GameColor.Green:
                    token.position = 2;
                    break;
                case GameColor.Blue:
                    token.position = 15;
                    break;
                case GameColor.Red:
                    token.position = 28;
                    break;
                case GameColor.Yellow:
                    token.position = 41;
                    break;
            }
            AddOnField(token);
            this.TakeTurns();

        }
        //go around the board
        /* private void RoundBoard(Token token)
        {
            if(token.position + dice.GetValue() > 51)
            {
                for (int i = 0; i < dice.GetValue(); i++)
                {
                    if(token.position + 1 > 51 )
                    {
                        token.position = 0;
                    }
                    else
                    {
                        token.position++;
                    }
                }
            }
        }
        */
        private void CheckSix()
        {
            if (dice.GetValue() == 6)
            {
                this.TakeTurns();
            }

            else
            {
                this.ChangeTurn();
            }
        }
        private void ChangeTurn()
        {

            throws = 0;

            WriteLine("", 1000);
            if (playerTurn == numberOfPlayers)
            {
                playerTurn = 1;
            }

            else
            {
                playerTurn++;
            }

            Console.WriteLine("Changing players ", 1000);
            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine(" " + i.ToString() + " ", 1000);
            }
            TakeTurns();
        }
    }
}