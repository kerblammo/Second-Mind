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
        int strikes;
        const int MaxStrikes = 3;
        private void ResetGame()
        {
            //Resets the form to its default state

            //hide dice
            HideRoom();
            HideWounds();
            HideLoot();

            //get player list
            GetPlayers();
            //Debug message to see if list is readable
            MessageBox.Show("It is now " + lvwPlayers.Items[0].Text + "'s turn.");
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
    }
}
