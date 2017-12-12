using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loot_Dice
{
    public partial class frmMain : Form
    {
        
        public frmMain(object players)
        {
            InitializeComponent();
        }

        /*
         * Author: Peter Adam
         * Dec 5, 2017
         * This is the main game window for Loot Die.
         * The program iterates through a list of players allowing them to take actions. It simulates
         * the rolling of three random dice (from a pool of three random categories). After rolling, the player may decide if
         * they wish to roll a new hand or bank their points. The player does not score until they bank, which passes the turn to
         * the next player. If a player earns three strikes, they lose their turn and score no points. Play proceeds
         * for four rounds, and then the player with the highest score is considered the winner.
         */

        frmSplash splash = new frmSplash();
        int strikes;    //current number of strikes current player has
        int points;
        const int MaxStrikes = 3;   //maximum number of strikes a player can have
        int playersIndex = 0;      //index of current player in lvwPlayers
        bool isNextPlayerPositive = false;      //indicates if play is moving up or down the list
        bool isBagEmpty = false;    //indicates if the dice bag is currently empty
        int round = -1;              //current round. 0 means game has not started
        int kicks;  //counts how many times the player kicks in the door
        const int MaxKicks = 4;     //max number of times a player can kick in the  door
        Bitmap bmpEmpty = Properties.Resources.Empty;
        Bitmap bmpGreenRoll = Properties.Resources.GreenRoll;
        Bitmap bmpGreenPoint = Properties.Resources.GreenPoint;
        Bitmap bmpGreenStrike = Properties.Resources.GreenStrike;
        Bitmap bmpYellowRoll = Properties.Resources.YellowRoll;
        Bitmap bmpYellowPoint = Properties.Resources.YellowPoint;
        Bitmap bmpYellowStrike = Properties.Resources.YellowStrike;
        Bitmap bmpRedRoll = Properties.Resources.RedRoll;
        Bitmap bmpRedPoint = Properties.Resources.RedPoint;
        Bitmap bmpRedStrike = Properties.Resources.RedStrike;

        //DO NOT MODIFY, this is the total collection of dice
        //6 green, 4 yellow, 3 red
        List<string> FreshDiceBag = new List<string> { "",  "GREEN", "GREEN", "GREEN", "GREEN", "GREEN", "YELLOW", "YELLOW", "YELLOW", "YELLOW", "RED", "RED", "RED" };
        List<string> CurrentDiceBag = new List<string> { "",  "GREEN", "GREEN", "GREEN", "GREEN", "GREEN", "YELLOW", "YELLOW", "YELLOW", "YELLOW", "RED", "RED", "RED" };

        private void ResetGame()
        {
            //Resets the form to its default state
            
            //set dice
            SetDice();
            //hide dice
            ShowAllDice(false);      

            //get player list
            GetPlayers();
            //Start first round
            StartTurn();
        }

        bool isGameOver = false;
        private void GameOver()
        {
            //Call when the game is over, displays winner and closes game
            if (lblLeader.Text == "")
            {
                MessageBox.Show("The day ends and the dungeon disappears once more. \nNobody has secured any loot. You may wish to consider a new line of work.", "Game Over");
            }
            else
            {
                MessageBox.Show("The day ends and the dungeon disappears once more. \n" + lblLeader.Text + " has secured the most loot, and is victorious!", "Game Over");
            }
            isGameOver = true;
            Close();
        }

        private string GetScoreLeader()
        {
            //returns the name of the player with the highest score
            string ScoreLeader = lvwPlayers.Items[0].Text;
            int hiscore = Convert.ToInt32(lvwPlayers.Items[lvwPlayers.Items.Count - 1].SubItems[1].Text);
            for (int i = lvwPlayers.Items.Count - 2; i >= 0; i--)
            {
                int score = Convert.ToInt32(lvwPlayers.Items[i].SubItems[1].Text);
                if (score > hiscore)
                {
                    hiscore = score;
                    ScoreLeader = lvwPlayers.Items[i].Text;
                }
            }
            return ScoreLeader;
            
        }

        private void SetDice()
        {
            foreach (PictureBox pic in grpRoll.Controls.OfType<PictureBox>())
            {
                pic.Image = bmpEmpty;
                pic.Refresh();
            }
        }

        private void RefreshDice()
        {
            isBagEmpty = false;
            CurrentDiceBag.Clear();
            int green = 4, yellow = 8; 
            //This function sets the current dice bag to its initial state
            for (int i = 0; i < FreshDiceBag.Count; i++)
            {
                if (i == 0) { CurrentDiceBag.Add(""); }
                if (i <= green) { CurrentDiceBag.Add("GREEN"); }
                else if (i <= yellow) { CurrentDiceBag.Add("YELLOW"); }
                else { CurrentDiceBag.Add("RED"); }
            }

        }

        private void UpdateScoreboard()
        {
            int currentPlayer = lvwPlayers.Items.IndexOfKey(lblPlayer.Text);
            if (currentPlayer > -1)
            {
            int score = Convert.ToInt32(lvwPlayers.Items[currentPlayer].SubItems[1].Text.ToString());
            score += points;
            lvwPlayers.Items[currentPlayer].SubItems[1].Text = score.ToString();
            }
        }

        private void StartTurn()
        {
            //called when turn ends to start the next turn


            //update scoreboard
            //UpdateScoreboard();

            btnRoll.Enabled = true;
            //Get a fresh dice bag
            RefreshDice();

            int totalPlayers = lvwPlayers.Items.Count;


            //hide old players' dice
            ShowAllDice(false);

            //Determine next player

            if (isNextPlayerPositive)   //play proceeds down list
            {
                playersIndex++;
                if (playersIndex == totalPlayers)   //all out of players
                {
                    playersIndex--;
                    StartRound();
                }
            }
            else    //play proceeds up list
            {
                playersIndex--;
                if (playersIndex == -1)     //all out of players
                {
                    playersIndex++;
                    StartRound();
                }
            }
            if (!isGameOver)
            {
                //Display Current Player
                lblPlayer.Text = lvwPlayers.Items[playersIndex].Text;
                if (playersIndex != 0
                || round != 0)
                {
                    MessageBox.Show("It is now " + lvwPlayers.Items[playersIndex].Text + "'s turn.", "Next Turn");
                }
                //Find High Score and Leader
                //int leaderIndex = FindHiScore();
                //lblLeader.Text = lvwPlayers.Items[leaderIndex].Text;
                //lblHiScore.Text = lvwPlayers.Items[leaderIndex].SubItems[1].Text;
                
            }
           



        }

        private void FindHiScore()
        {
            //checks if the current player's score is higher 
            //if it is, update the hiscore

            int currentPlayer = lvwPlayers.Items.IndexOfKey(lblPlayer.Text);
            int score = 0;
            if (currentPlayer > -1) {score = Convert.ToInt32(lvwPlayers.Items[currentPlayer].SubItems[1].Text); }

            int hiscore = Convert.ToInt32(lblHiScore.Text);
            if (score > hiscore)
            {
                lblLeader.Text = lblPlayer.Text;
                lblHiScore.Text = score.ToString();
            }

        }

        private void StartRound()
        {
            //start the  next round of the game
            //stops game if there are no more rounds remaining
            string[] roundNames = new string[]{ "Dawn", "Day", "Dusk", "Dark" };
            const int MaxRounds = 3;         //maximum number of rounds in game
            round++;
            if (round <= MaxRounds) 
            {
                isNextPlayerPositive = !isNextPlayerPositive; //toggle rotation of play
                lblRound.Text = roundNames[round];
            }
            else
            {
                //UpdateScoreboard();
                GameOver();
            }
        }

        private void GetPlayers()
        {
            //empty list first
            lvwPlayers.Items.Clear();
            

            //add players to game from collection
            foreach(string name in frmSplash.players)
            {
                //create a listview item. Give each item a subitem with a default score
                //Text to display is the player's name. Finally, add the item
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add("0");
                lvi.Text = name;
                lvi.Name = name;
                lvwPlayers.Items.Add(lvi);
            }
        }

        private void ShowLoot(bool show)
        {
            //Make each picbox in grpLoot invisible
            foreach (Control pic in grpLoot.Controls.OfType<PictureBox>())
            {
                pic.Visible = show;
            }
        }

        private void ShowWounds(bool show)
        {
            //Make each picbox in grpWounds invisible
            foreach (Control pic in grpWounds.Controls.OfType<PictureBox>())
            {
                pic.Visible = show;
            }
        }

        private void ShowRoom(bool show)
        {
            //Make each picbox in grpRoll invisible
            foreach (Control pic in grpRoll.Controls.OfType<PictureBox>())
            {
                pic.Visible = show;
            }
        }

        private void ShowAllDice(bool show)
        {
            //toggles visibility of all dice, where show is the visibility
            ShowRoom(show);
            ShowWounds(show);
            ShowLoot(show);
        }

        private void CheckLoot()
        {
            //Check each image to see if it's a point. If it is, call SendLoot, sending the die's current image
            //Loot dice are reset to empty emages
            foreach (PictureBox pic in grpRoll.Controls.OfType<PictureBox>())
            {
                bool isSent = false;
                if (pic.Image == bmpGreenPoint)    //Green die sent
                {
                    SendPoint(bmpGreenPoint);
                    isSent = true;
                }
                else if (pic.Image == bmpYellowPoint)  //Yellow die sent
                {
                    SendPoint(bmpYellowPoint);
                    isSent = true;
                }
                else if (pic.Image == bmpRedPoint)     //Red die sent
                {
                    SendPoint(bmpRedPoint);
                    isSent = true;
                }
                if (isSent)     //a point has been sent, refresh the die
                {
                    pic.Image = bmpEmpty;
                    pic.Refresh();
                }
            }
        }

        private void CheckWounds()
        {
            //Check each image to see if it's a strike. If it is, call SendStrike, sending the die's current image
            //Strike dice are reset to empty images
            foreach (PictureBox pic in grpRoll.Controls.OfType<PictureBox>())
            {
                bool isSent = false;
                if (pic.Image == bmpGreenStrike)    //Green die sent
                {
                    SendStrike(bmpGreenStrike);
                    isSent = true;
                }
                else if (pic.Image == bmpYellowStrike)  //Yellow die sent
                {
                    SendStrike(bmpYellowStrike);
                    isSent = true;
                }
                else if (pic.Image == bmpRedStrike)     //Red die sent
                {
                    SendStrike(bmpRedStrike);
                    isSent = true;
                }
                if (isSent)     //a strike has been sent, refresh the die
                {
                    pic.Image = bmpEmpty;
                    pic.Refresh();
                }
            }
        }

        private void SendStrike(Bitmap strikeImage)
        {
            //Go through each empty spot in Wounds and place the wounded die there
            foreach (PictureBox pic in grpWounds.Controls.OfType<PictureBox>())
            {
                if (pic.Image == bmpEmpty
                    || pic.Visible == false)
                {
                    pic.Image = strikeImage;
                    pic.Visible = true;
                    pic.Refresh();
                    break;
                }
            }
        }

        private void SendPoint(Bitmap pointImage)
        {
            //Go through each empty spot in Loot and place the loot die there
            foreach (PictureBox pic in grpLoot.Controls.OfType<PictureBox>())
            {
                if (pic.Image == bmpEmpty
                    || pic.Visible == false)
                {
                    pic.Image = pointImage;
                    pic.Visible = true;
                    pic.Refresh();
                    break;
                }
            }
        }

        private void RollRoom()
        {
            //create random generator
            Random rand = new Random();
            const int MinRoll = 1;
            const int MaxRoll = 6;

            

            //Move wounds to wound area
            CheckWounds();

            //Move points to loot area
            CheckLoot();

            


                if (!isBagEmpty)
                {

                    //Rolls the dice in the room
                    foreach (PictureBox die in grpRoll.Controls.OfType<PictureBox>())
                    {


                        int totalDice = CurrentDiceBag.Count;
                        if (totalDice > 2)

                        {
                            //get a new die
                            int index;
                            if (totalDice >= 2) { index = rand.Next(1, totalDice - 1); }
                            else { index = 1; }

                            totalDice--;
                            string colour = CurrentDiceBag[index];
                            CurrentDiceBag.RemoveAt(index);

                            switch (colour)
                            {
                                case "GREEN":
                                    die.Image = bmpGreenRoll;
                                    break;
                                case "YELLOW":
                                    die.Image = bmpYellowRoll;
                                    break;
                                case "RED":
                                    die.Image = bmpRedRoll;
                                    break;
                            }
                            die.Refresh();
                        }
                        
                        //die is not/no longer empty, so we can roll it
                        if (die.Image == bmpGreenRoll)    //Green
                        {
                            int roll = rand.Next(MinRoll, MaxRoll + 1);
                            switch (roll)
                            {
                                case 1:
                                case 2:
                                    die.Image = bmpGreenRoll;
                                    break;
                                case 3:
                                case 4:
                                case 5:
                                    die.Image = bmpGreenPoint;
                                    points++;
                                    break;
                                case 6:
                                    die.Image = bmpGreenStrike;
                                    strikes++;
                                    break;
                            }
                        }
                        else if (die.Image == bmpYellowRoll) //Yellow
                        {
                            int roll = rand.Next(MinRoll, MaxRoll + 1);
                            switch (roll)
                            {
                                case 1:
                                case 2:
                                    die.Image = bmpYellowRoll;
                                    break;
                                case 3:
                                case 4:
                                    die.Image = bmpYellowPoint;
                                    points++;
                                    break;
                                case 5:
                                case 6:
                                    die.Image = bmpYellowStrike;
                                    strikes++;
                                    break;
                            }
                        }
                        else if (die.Image == bmpRedRoll)     //red
                        {
                            int roll = rand.Next(MinRoll, MaxRoll + 1);
                            switch (roll)
                            {
                                case 1:
                                case 2:
                                    die.Image = bmpRedRoll;
                                    break;
                                case 3:
                                    die.Image = bmpRedPoint;
                                    points++;
                                    break;
                                case 4:
                                case 5:
                                case 6:
                                    die.Image = bmpRedStrike;
                                    strikes++;
                                    break;
                            }
                        }
                        die.Refresh();
                    }
                }

            kicks++;
            if (!CheckStrikes())    //check if there are strikes remaining
            {
                
                if (kicks >= MaxKicks)
                { //There are no more dice 

                    //Display end of dice bag, and prevent player from drawing more dice
                    MessageBox.Show("There are no more dice to draw!", "Out of Dice");
                    isBagEmpty = true;
                    btnRoll.Enabled = false;
                    CheckWounds();
                    CheckLoot();
                    

                }
            }
            
        }

        private bool CheckStrikes()
        {
            //Checks if player is all out of wounds. If it does, it forces them to pass their turn and score no points
            bool isBust = false;
            if (strikes >= MaxStrikes)
            {
                MessageBox.Show("You have no remaining wounds. \nThe monsters steal your loot and you must return to the tavern in disgrace!", "Bust!");
                points = 0;
                CheckWounds();
                CheckLoot();
                btnRoll.Enabled = false;
                isBust = true;
            }
            return isBust;
        }

        private void ResetStrikePoint()
        {
            //Resets strikes and points to 0
            strikes = 0;
            points = 0;
            kicks = 0;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Set form to default state
            ResetGame();
        }

        private void btnBank_Click(object sender, EventArgs e)
        {
            //Tabulates points and passes control to the next player

            UpdateScoreboard();
            FindHiScore();
            SetDice();
            StartTurn();
            ResetStrikePoint();
            

        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            //move current room's dice to their appropriate areas. Fill dice, and roll them
            ShowRoom(true);
            RollRoom();
        }
    }
}
