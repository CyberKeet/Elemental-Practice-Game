// Name: Katie Straughn
// Date: 5/9/2017
// Program: Practice Game
// Purpose: A sort of rock-paper-scissors game, but using six different elements.

using System;
using System.Windows.Forms;
using System.Drawing;

namespace PracticeGame_v1._0
{
    public partial class practiceGameForm : Form
    {
        /* Declare variable to be used throughout the class */
        int playerCounter = 0; // Counts damage player has taken
        int compCounter = 0; // Counts damage comp has taken
        int roundCounter = 1; // Counts the round number, starts at round 1

        /* This contructor creates the game form and it's attributes */
        public practiceGameForm() {
            InitializeComponent();
            
            // Set the radio buttons click events
            foreach (RadioButton rButton in symbolsGBox.Controls)
            {
                rButton.Click += new EventHandler(rBtn_Click);
            } 

            resetBtn.Click += new EventHandler(reset_Click);
            
        }

        /* This button displays the games instructions */
        private void instructionsBtn_Click(object sender, EventArgs e)
        {
            if (instructionLbl.Visible == false)
            {
                instructionLbl.Text = "Instructions to be displayed here";
                instructionLbl.Visible = true;
                instructionsBtn.Text = "Hide Instructions";
            }
            else if (instructionLbl.Visible == true)
            {
                instructionLbl.Visible = false;
                instructionsBtn.Text = "Instructions";
            }
        }

        /* This method resets the game */
        public void reset_Click(object sender, System.EventArgs e)
        {
            playerSymbol1.BackColor = Color.White; playerSymbol2.BackColor = Color.White; playerSymbol3.BackColor = Color.White; // Set player symbols to blank
            compSymbol1.BackColor = Color.White; compSymbol2.BackColor = Color.White; compSymbol3.BackColor = Color.White; // Set comp images to blank

            // Remove points from HP bars
            if (compCounter <= compHPBar.Value)
                compHPBar.Value -= compCounter;
            else
                compHPBar.Value = 0;

            if (playerCounter <= playerHPBar.Value)
                playerHPBar.Value -= playerCounter;
            else
                playerHPBar.Value = 0;

            // Turn labels invisible
            symbol1CompareLbl.Visible = false;
            symbol2CompareLbl.Visible = false;
            symbol3CompareLbl.Visible = false;

            // Reset counters
            compCounter = 0; compPointsCount.Text = "0"; compPointsCount.ForeColor = Color.Black;
            playerCounter = 0; playerPointsCount.Text = "0"; playerPointsCount.ForeColor = Color.Black;

            roundCounter = 1; // Reset to round 1

            symbolsGBox.Enabled = true; // Disable radio buttons

            resetBtn.Visible = false;

            // Check if there is a winner
            if (isWinner(compHPBar) == true)
            {
                winLbl.Visible = true;
                symbolsPanel.Visible = false;
            }
            else if (isWinner(playerHPBar) == true)
            {
                winLbl.Text = "Sorry, you lost.";
                winLbl.Visible = true;
                symbolsPanel.Visible = false;
            }

        }

        /* This method checks if a HP bar is zero (which means the opposing side wins) */
        private Boolean isWinner(ProgressBar pbar)
        {
            if (pbar.Value == 0) // Check is bar is at 0
            {
                return true;
            }
            else
                return false;
        }

        /* This method handles what happens when a radio button is clicked */
        private void rBtn_Click(object sender, System.EventArgs e)
        {
            PictureBox pic; // Picture box to be altered

            // Determine which round it is and therefor which picture box is to be altered
            if (roundCounter == 1)
                pic = playerSymbol1;
            else if (roundCounter == 2)
                pic = playerSymbol2;
            else // If roundCounter is 3
                pic = playerSymbol3; 
            
            int num = 0; // Hold's the number correlatingwith the button selected

            // Determing which button is selected throughb if staements
            if (waterRBtn.Checked)
            {
                pic.BackColor = Color.Blue;
                num = 1;
            }
            else if (fireRBtn.Checked)
            {
                pic.BackColor = Color.Red;
                num = 2;
            }
            else if (metalRBtn.Checked)
            {
                pic.BackColor = Color.Gray;
                num = 3;
            }
            else if (woodRBtn.Checked)
            {
                pic.BackColor = Color.Green;
                num = 4;
            }
            else if (airRBtn.Checked)
            {
                pic.BackColor = Color.LightBlue;
                num = 5;
            }
            else if (lightningRBtn.Checked)
            {
                pic.BackColor = Color.Yellow;
                num = 6;
            }

            compare(num, pickCompElement()); // Method call compare the two elements
            roundCounter++;

            if (roundCounter > 3)
            {
                resetBtn.Visible = true;
                symbolsGBox.Enabled = false; // Disable radio buttons

                foreach (RadioButton rButton in symbolsGBox.Controls)
                {
                    rButton.Checked = false; // Uncheck radio buttons
                }
            }
        }

        /* This method handles randomizing an elemnt for the comp */
        private int pickCompElement()
        {
            PictureBox pic; // Picture box to be altered

            // Determine which round it is and therefor which picture box is to be altered
            if (roundCounter == 1)
                pic = compSymbol1;
            else if (roundCounter == 2)
                pic = compSymbol2;
            else // If roundCounter is 3
                pic = compSymbol3; 

            Random rand = new Random(); // Create a Random object
            int randInt = rand.Next(1, 7); // Picks a random value from 1-6, wih each int representing an element (1=water, 2=fire, etc.)
            if (randInt == 1)
            {
                pic.BackColor = Color.Blue;
            }
            else if (randInt == 2)
            {
                pic.BackColor = Color.Red;
            }
            else if (randInt == 3)
            {
                pic.BackColor = Color.Gray;
            }
            else if (randInt == 4)
            {
                pic.BackColor = Color.Green;
            }
            else if (randInt == 5)
            {
                pic.BackColor = Color.LightBlue;
            }
            else if (randInt == 6)
            {
                pic.BackColor = Color.Yellow;
            }
            return randInt; // Return the randomized int's value
        }

        /* This method compares the player's and the comp's elements */
        private void compare(int playerElement, int compElement)
        {
            /* Elemental type match-up rules:
             * The type match-ups are set up in a circle of 1->2->3->4->5->6->(back to 1). 
             * The digit clockwise one place to the right looses two HP. 
             * The digit clockwise two places to the right looses one HP.
             * The digit opposite on the circle causes no HP to be lost.
             * The digit clockwise four place to the right takes one HP from opponent. 
             * The digit clockwise five place to the right takes two HP from opponent.
             * 
             * For Example: Assume player plays a water element (number 1). Relative to the computers choices: 
             * 1 = 1    (no HP taken from comp or player)
             * 1 > 2    (2 HP taken from comp)
             * 1 > 3    (1 HP taken from comp)
             * 1 = 4    (no HP taken from comp or player)
             * 1 < 5    (1 HP taken from player)
             * 1 < 6    (2 HP taken from player)
             */

            int result = playerElement - compElement;

            Label lbl; // comparison Label to be altered

            // Determine which round it is and therefor which picture box is to be altered
            if (roundCounter == 1)
                lbl = symbol1CompareLbl;
            else if (roundCounter == 2)
                lbl = symbol2CompareLbl;
            else // If roundCounter is 3
                lbl = symbol3CompareLbl; 

            if (result == -1 || result == 5) // Two damage to comp
            {
                lbl.Text = "^";
                compCounter += 2;
                compPointsCount.ForeColor = Color.Red;
            }
            else if (result == -2 || result == 4) // One damage to comp
            {
                lbl.Text = "^";
                compCounter += 1;
                compPointsCount.ForeColor = Color.Red;
            }
            else if (result == -3 || result == 3 || result == 0) // No damage
            {
                lbl.Text = "||";
            }
            else if (result == -4 || result == 2) // One damage to player
            {
                lbl.Text = "v";
                playerCounter += 1;
                playerPointsCount.ForeColor = Color.Red;
            }
            else if (result == -5 || result == 1) // Two damage to player
            {
                lbl.Text = "v";
                playerCounter += 2;
                playerPointsCount.ForeColor = Color.Red;
            }

            lbl.Visible = true; // Make label visible

            // Add damage points to counters
            compPointsCount.Text = compCounter.ToString();
            playerPointsCount.Text = playerCounter.ToString();
        }
    }
}
