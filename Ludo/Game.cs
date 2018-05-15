using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo
{
    // colors for the tokens and players
    public enum GameColor { Green, Blue, Red, Yellow};
    // States for the tokens
    public enum GameState { InPlay, Finished };
    public class Game
    {
        private GameState state;
        private Player[] players;
        private int numberOfPlayers;
        private int playerTurn = 1;

        private Dice dice = new Dice();
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
                    Console.WriteLine("Press T when you wanna throw your dice");
                }
                while (Console.ReadKey().KeyChar != 't');
                WriteLine("Your die landed on: " + dice.ThrowDice().ToString());
                ShowTurnOptions(myTurn.GetTokens());
                break;
            }

        }


        public void ShowTurnOptions(Token[] tokens)
        {
            int choice = 0;

            WriteLine("Here are your tokens:");
            foreach (Token tk in tokens)
            {

                Console.WriteLine("Brik #" + tk.GetTokenId() + ": er placeret: " + tk.GetState(), 1000);

                switch (tk.GetState())
                {
                    case TokenState.Home:
                        if (dice.GetValue() == 6)
                        {
                            Console.WriteLine(" <- Playable");
                            choice++;
                        }
                        else
                        {
                            Console.WriteLine(" <- Not playable");
                        }
                        break;
                    case TokenState.InPlay:
                        Console.WriteLine(" <- Playable");
                        choice++;
                        break;
                    case TokenState.Safe:
                        Console.WriteLine(" <- Playable");
                        choice++;
                        break;
                    case TokenState.Finished:
                        Console.WriteLine(" <- Not Playable");
                        choice++;
                        break;
                    case TokenState.HomeStretch:
                        Console.WriteLine(" <- Playable");
                        choice++;
                        break;
                }
                WriteLine("");
            }
            WriteLine("");
            WriteLine("You have " + choice.ToString() + " choices this turn", 2000);

            //No choices
            if (choice == 0)
            {
                this.ChangeTurn();
            }
            else
            {
                WriteLine("Choos which token you wanna move");

            }
        }

        private void ChangeTurn()
        {
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
