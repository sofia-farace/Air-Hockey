using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;
namespace Air_Hockey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Rectangle player1 = new Rectangle(180, 30, 40, 40);
        Rectangle player2 = new Rectangle(180, 540, 40, 40);
        Rectangle ball = new Rectangle(190, 290, 20, 20);

        Rectangle goal1 = new Rectangle(130, 0, 140, 10);
        Rectangle goal2 = new Rectangle(130, 600, 140, 10);

        int player1Score = 0;
        int player2Score = 0;

        int playerSpeed = 6;
        int ballXSpeed = -7;
        int ballYSpeed = 7;

        bool wDown = false;
        bool sDown = false;
        bool dDown = false;
        bool aDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool rightArrowDown = false;
        bool leftArrowDown = false;

        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush purpleBrush = new SolidBrush(Color.MediumPurple);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Pen whitePen = new Pen(Color.White, 3);

        SoundPlayer soundPlayer = new SoundPlayer();

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }

        private void gameEngine_Tick(object sender, EventArgs e)
        {
            int player1X = player1.X;
            int player1Y = player1.Y;
            int player2X = player2.X;   
            int player2Y = player2.Y;
            int ballY = ball.Y;
            int ballX = ball.X;

            soundPlayer = new SoundPlayer(Properties.Resources.bounceSound);

            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;

            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
            }

            if (sDown == true && player1.Y < 600 - player1.Height)
            {
                player1.Y += playerSpeed;
            }

            if (aDown == true && player1.X > 0)
            {
                player1.X -= playerSpeed;
            }

            if (dDown == true && player1.X < 400 - player1.Width)
            {
                player1.X += playerSpeed;
            }

            if (upArrowDown == true && player2.Y > 0)
            {
                player2.Y -= playerSpeed;
            }

            if (downArrowDown == true && player2.Y < 600 - player2.Height)
            {
                player2.Y += playerSpeed;
            }

            if (leftArrowDown == true && player2.X > 0)
            {
                player2.X -= playerSpeed;
            }

            if (rightArrowDown == true && player2.X < 400 - player2.Width)
            {
                player2.X += playerSpeed;
            }

            if (ball.Y < 0 || ball.Y > 600 - ball.Height)
            {
                ballYSpeed *= -1;
                soundPlayer.Play();
            }
            if (ball.X < 0 || ball.X > 400 - ball.Width)
            {
                ballXSpeed *= -1;
                soundPlayer.Play();
            }

            Rectangle player1Top = new Rectangle(player1X, player1Y, ball.Width, 1);
            Rectangle player1Right = new Rectangle(player1X + 40, player1Y, 1, ball.Height);
            Rectangle player1Left = new Rectangle(player1X, player1Y, 1, ball.Height);
            Rectangle player1Bottom = new Rectangle(player1X, player1Y + 40, ball.Width, 1);

            Rectangle player2Top = new Rectangle(player2X, player2Y, player2.Width, 1);
            Rectangle player2Right = new Rectangle(player2X + 40, player2Y, 1, player2.Height);
            Rectangle player2Left = new Rectangle(player2X, player2Y, 1, player2.Height);
            Rectangle player2Bottom = new Rectangle(player2X, player2Y + 40, player2.Width, 1);

            if (player1Top.IntersectsWith(ball) || player1Bottom.IntersectsWith(ball) || player2Top.IntersectsWith(ball) || player2Bottom.IntersectsWith(ball))
            {
                ballYSpeed *= -1;
                soundPlayer.Play();
            }
            if (player1Right.IntersectsWith(ball) || player1Left.IntersectsWith(ball) || player2Right.IntersectsWith(ball) || player2Left.IntersectsWith(ball))
            {
                ballXSpeed *= -1;
                soundPlayer.Play();
            }

            if(goal1.IntersectsWith(ball))
            {
                Thread.Sleep(300);

                soundPlayer = new SoundPlayer(Properties.Resources.score);
                soundPlayer.Play();
                player2Score++;
                p2ScoreLabel.Text = $"{player2Score}";

                resetPosition();
            }

            if (goal2.IntersectsWith(ball))
            {
                Thread.Sleep(300);

                soundPlayer = new SoundPlayer(Properties.Resources.score);
                soundPlayer.Play();
                player1Score++;
                p1ScoreLabel.Text = $"{player1Score}";

                resetPosition();
            }

            if (player1Score == 3)
            {
                soundPlayer = new SoundPlayer(Properties.Resources.cheer);
                soundPlayer.Play();
                gameEngine.Enabled = false;
                winnerLabel.Text = $"player 1 wins";

                restartButton.Visible = true;
                restartButton.Enabled = true;
            }

            if (player2Score == 3)
            {
                soundPlayer = new SoundPlayer(Properties.Resources.cheer);
                soundPlayer.Play();
                gameEngine.Enabled = false;
                winnerLabel.Text = $"player 2 wins";

                restartButton.Visible = true;
                restartButton.Enabled = true;
            }

            Refresh();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(blueBrush, goal1);
            e.Graphics.FillRectangle(purpleBrush, goal2);

            e.Graphics.DrawLine(whitePen, 0, 298, 400, 298);
            e.Graphics.DrawArc(whitePen, 100, -100, 200, 200, 0, 180);
            e.Graphics.DrawArc(whitePen, 100, 510, 200, 200, 0, -180);
            e.Graphics.DrawEllipse(whitePen, 100, 200, 200, 200);

            e.Graphics.FillEllipse(blueBrush, player1);
            e.Graphics.FillEllipse(purpleBrush, player2);
            e.Graphics.FillRectangle(whiteBrush, ball);
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            player1Score = 0;
            p1ScoreLabel.Text = $"{player1Score}";
            player2Score = 0;
            p2ScoreLabel.Text = $"{player2Score}";
            gameEngine.Enabled = true;

            winnerLabel.Visible = false;
            restartButton.Visible = false;
            restartButton.Enabled = false;
        }

        private void resetPosition()
        {
            ball.X = 190;
            ball.Y = 290;

            player1.X = 180;
            player1.Y = 30;

            player2.X = 180;
            player2.Y = 540;
        }
    }
}
