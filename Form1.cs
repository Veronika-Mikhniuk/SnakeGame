using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
	public partial class Form1 : Form
	{
		Point[] snake;
		Point apple;
		Random r = new Random();
		SolidBrush colorOfGameField;
		SolidBrush colorOfSnake;
		SolidBrush colorOfApple;
		string direction;
		int lenghtOfSnake;
		int widthOfPlayingField;
		int heightOfPlayingField;

		private void InstanceSnake()
		{
			snake = new Point[10000];
			lenghtOfSnake = 5;
			direction = "Up";
			lbScore.Text = "0";
			timer1.Interval = 100;

			for (int i = 0; i < 5; i++)
			{
				snake[i].X = widthOfPlayingField / 2;
				snake[i].Y = heightOfPlayingField / 2+i;
			}

			apple.X = r.Next(0, widthOfPlayingField - 1);
			apple.Y = r.Next(0, heightOfPlayingField - 1);
		}
		public Form1()//Конструктор
		{
			InitializeComponent();
			pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
			widthOfPlayingField = pictureBox1.Width / 10;
			heightOfPlayingField = pictureBox1.Height / 10;
			
			InstanceSnake();



			colorOfGameField = new SolidBrush(Color.Black);
			colorOfSnake = new SolidBrush(Color.DarkRed);
			colorOfApple = new SolidBrush(Color.Green);
			//apple.X = r.Next(0, widthOfPlayingField-1);
			//apple.Y = r.Next(0, heightOfPlayingField - 1);

		}

		private void CheckGameOver()
		{
			for (int i = 1; i < lenghtOfSnake; i++)
			{
				if (snake[0].X == snake[i].X && snake[0].Y == snake[i].Y)
				{
					timer1.Stop();
					panel1.Visible = true;
				}

			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			Graphics gamePanel = Graphics.FromImage(pictureBox1.Image);
			gamePanel.FillRectangle(colorOfGameField, 0, 0, pictureBox1.Width, pictureBox1.Height);
			
			CheckGameOver();



			if (lenghtOfSnake > 10000 - 3) lenghtOfSnake = 10000 - 3;//Поверка выхода за пределы массива
			for (int i = lenghtOfSnake; i >= 0; i--)
			{
				snake[i + 1].X = snake[i].X;//Предыдущую координату записываем в следующую
				snake[i + 1].Y = snake[i].Y;
			}


			if (direction == "Up") snake[0].Y -= 1;
			if (direction == "Down") snake[0].Y += 1;
			if (direction == "Right") snake[0].X += 1;
			if (direction == "Left") snake[0].X -= 1;

			for (int i = 0; i < lenghtOfSnake; i++)
			{
				//Проход сквозь стены	
				if (snake[i].X < 0) snake[i].X = snake[i].X + widthOfPlayingField;
				if (snake[i].X > widthOfPlayingField) snake[i].X = 0;
				if (snake[i].Y < 0) snake[i].Y = snake[i].Y + heightOfPlayingField;
				if (snake[i].Y > heightOfPlayingField) snake[i].Y = 0;

				gamePanel.FillEllipse(colorOfSnake, snake[i].X * 10, snake[i].Y * 10, 10, 10);
				if (apple.X == snake[0].X && apple.Y == snake[0].Y)//Рос змеи при стедани яблока
				{
					apple.X = r.Next(0, widthOfPlayingField - 1);
					apple.Y = r.Next(0, heightOfPlayingField - 1);
					lenghtOfSnake++;
					timer1.Interval = timer1.Interval == 30 ? timer1.Interval : timer1.Interval - 1;
					lbScore.Text = (int.Parse(lbScore.Text) + 1).ToString();
				}
			}
			for (int i = 1; i < lenghtOfSnake; i++)
			{

				if (apple.X == snake[i].X && apple.Y == snake[i].Y)//Проверка появления яблока на теле змеи
				{
					apple.X = r.Next(0, widthOfPlayingField - 1);
					apple.Y = r.Next(0, heightOfPlayingField - 1);
				}
			}

			gamePanel.FillEllipse(colorOfApple, apple.X * 10, apple.Y * 10, 10, 10);



			//if (lenghtOfSnake < 6) lenghtOfSnake++;

			pictureBox1.Invalidate();

		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			direction = e.KeyCode == Keys.Left && direction != "Right" ? "Left" : direction;
			direction = e.KeyCode == Keys.Right && direction != "Left" ? "Right" : direction;
			direction = e.KeyCode == Keys.Up && direction != "Down" ? "Up" : direction;
			direction = e.KeyCode == Keys.Down && direction != "Up" ? "Down" : direction;
		}

		private void btnNewGame_Click(object sender, EventArgs e)
		{
			panel1.Visible = false;
			InstanceSnake();
			timer1.Start();
		}
	}
}
