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

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Add a player to the player list
            string name = txtAddPlayer.Text;
            if (name == "")     //empty field
            {
                MessageBox.Show("Please enter a player name.", "Missing Name");
                txtAddPlayer.Focus();
            }
            else if (cboPlayers.Items.Contains(name))   //duplicate id
            {
                MessageBox.Show("Player \"" + name + "\" already exists. \nPlease enter a unique name.", "Duplicate Player");
                txtAddPlayer.Focus();
            }
            else    //Add player to list
            {
                cboPlayers.Items.Add(name);
                txtAddPlayer.Clear();
                txtAddPlayer.Focus();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Clear entire player table
            cboPlayers.Items.Clear();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //Remove checked players
            for (int i = cboPlayers.Items.Count - 1; i >= 0; i--)
            {
                if (cboPlayers.GetItemChecked(i))
                {
                    cboPlayers.Items.RemoveAt(i);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //opens new instance of game
            frmMain Game = new frmMain();
            Game.Show();
            //create new event handler for when game closes
            Game.FormClosed += new FormClosedEventHandler(Game_FormClosed);
            Hide();
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Close application
            Close();
        }

        bool isHelpOpen = false;
        private void btnHelp_Click(object sender, EventArgs e)
        {
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
            isHelpOpen = false;
        }

        bool isCreditsOpen = false;
        private void btnCredits_Click(object sender, EventArgs e)
        {
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
            isCreditsOpen = false;
        }
    }
}
