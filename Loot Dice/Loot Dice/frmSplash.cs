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
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();
        }

        /*
         * Author: Peter Adam
         * Dec 2, 2017
         * Loot Die is a dice game for 2+ players playing locally
         * This new game window allows the user to open a new game with a number of players
         * of their choosing. It also includes navigation to Help and Credits pages.
         */

        //list of players to be in game
        public static List<string> players = new List<string>();

        private void EnableNewGame()
        {
            //checks if there are at least two players, and if there are, enables the start button
            if (lstPlayers.Items.Count <= 1)
            {
                btnStart.Enabled = false;
            }
            else
            {
                btnStart.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            //Add a player to the player list
            string name = txtAddPlayer.Text;
            if (name == "")     //empty field
            {
                MessageBox.Show("Please enter a player name.", "Missing Name");
                txtAddPlayer.Focus();
            }
            else if (lstPlayers.Items.Contains(name))   //duplicate id
            {
                MessageBox.Show("Player \"" + name + "\" already exists. \nPlease enter a unique name.", "Duplicate Player");
                txtAddPlayer.Focus();
            }
            else    //Add player to list
            {
                lstPlayers.Items.Add(name);
                txtAddPlayer.Clear();
                txtAddPlayer.Focus();
                EnableNewGame(); //if there are 2+ players, game is playable
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Clear entire player table
            lstPlayers.Items.Clear();
            EnableNewGame(); //if there are not 2+ players, game is unplayable
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //Remove checked players
            for (int i = lstPlayers.Items.Count - 1; i >= 0; i--)
            {
                if (lstPlayers.GetItemChecked(i))
                {
                    lstPlayers.Items.RemoveAt(i);
                }
            }
            EnableNewGame(); //if there are not 2+ players, game is unplayable
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            //Send player list to game
            
            foreach (string item in lstPlayers.Items)
            {
                players.Add(item);
            }

            //opens new instance of game
            frmMain Game = new frmMain(players);

            

            Game.Show();
            //create new event handler for when game closes
            Game.FormClosed += new FormClosedEventHandler(Game_FormClosed);
            Hide();
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Show application
            players.Clear();
            Show();
        }

        bool isHelpOpen = false;
        private void btnHelp_Click(object sender, EventArgs e)
        {
            //open the help page, if the help page isn't open
            if (!isHelpOpen)
            {
                frmHelp Help = new frmHelp();
                Help.FormClosed += new FormClosedEventHandler(Help_FormClosed);
                Help.Show();
                isHelpOpen = true;
            } 
        }

        private void Help_FormClosed(object sender, FormClosedEventArgs e)
        {
            //runs when Help is clsoed. Allows Help to be opened again
            isHelpOpen = false;
        }

        bool isCreditsOpen = false;
        private void btnCredits_Click(object sender, EventArgs e)
        {
            //opens the credits page, if the credits page isn't open
            if (!isCreditsOpen)
            {
                frmCredits Credits = new frmCredits();
                Credits.FormClosed += new FormClosedEventHandler(Credits_FormClosed);
                Credits.Show();
                isCreditsOpen = true;
            }
            
        }

        private void Credits_FormClosed(object sender, FormClosedEventArgs e)
        {
            //runs when Credits is closed. Allows Credits to be opened again
            isCreditsOpen = false;
        }
        
    }
}
