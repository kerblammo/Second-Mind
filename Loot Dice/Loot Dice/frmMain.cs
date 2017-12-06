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
        const int MaxStrikes = 3;   //maximum number of strikes a player can have
        int playersIndex = 0;      //index of current player in lvwPlayers
        bool isNextPlayerPositive = false;      //indicates if play is moving up or down the list
        int round = -1;              //current round. 0 means game has not started
        
        

        private void ResetGame()
        {
            //Resets the form to its default state

            //hide dice
            HideRoom();
            HideWounds();
            HideLoot();

            //get player list
            GetPlayers();
            //Start first round
            StartTurn();
        }

        private void GameOver()
        {
            //STUBBED. Call when game is over
            MessageBox.Show("GAME OVER");
        }

        private void StartTurn()
        {
            //called when turn ends to start the next turn

            int totalPlayers = lvwPlayers.Items.Count;

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

            //Display Current Player
            lblPlayer.Text = lvwPlayers.Items[playersIndex].Text;

            //Find High Score and Leader
            int leaderIndex = FindHiScore();
            lblLeader.Text = lvwPlayers.Items[leaderIndex].Text;
            lblHiScore.Text = lvwPlayers.Items[leaderIndex].SubItems[1].Text;



        }

        private int FindHiScore()
        {
            //returns the index of the player with the highest score
            int index = 0, hiScore = Convert.ToInt32(lvwPlayers.Items[0].SubItems[1].Text);
            for (int i = 1; i < lvwPlayers.Items.Count; i++)
            {
                int num = Convert.ToInt32(lvwPlayers.Items[i].SubItems[1].Text);
                if (num > hiScore)
                {
                    hiScore = num;
                    index = i;
                }
            }
            return index;
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
                lvwPlayers.Items.Add(lvi);
            }
        }

        private void HideLoot()
        {
            //Make each picbox in grpLoot invisible
            foreach (Control pic in grpLoot.Controls.OfType<PictureBox>())
            {
                pic.Visible = false;
            }
        }

        private void HideWounds()
        {
            //Make each picbox in grpWounds invisible
            foreach (Control pic in grpWounds.Controls.OfType<PictureBox>())
            {
                pic.Visible = false;
            }
        }

        private void HideRoom()
        {
            //Make each picbox in grpRoll invisible
            foreach (Control pic in grpRoll.Controls.OfType<PictureBox>())
            {
                pic.Visible = false;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Set form to default state
            ResetGame();
        }

        private void btnBank_Click(object sender, EventArgs e)
        {
            //TODO temporarily just moves to next player
            StartTurn();
        }
    }
}
