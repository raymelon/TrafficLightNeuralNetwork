using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TrafficLightsEyEnEn
{
	
	public class TrafficLightControllerGUI : Form
	{
		#region Windows Forms Methods
		public TrafficLightControllerGUI()
		{
			 InitializeComponent();	
		}

		private void InitializeComponent()
		{
			try
			{
				TimerUI = new System.Windows.Forms.Timer();

				TimerUI.Interval = 1000;
				TimerUI.Tick += new EventHandler(TimerUI_Tick);

				this.Text = "Traffic Light Controller using ANN";
				this.Size = new Size(900, 600);
				this.StartPosition = FormStartPosition.CenterScreen;
				this.BackColor = Color.FromArgb(255, 255, 255);

				PanelRoad = new Panel();
				PanelRoad.SetBounds(0, 0, 700, 600);

				LabelDashBoard = new Label[6];

				int yPos = 0;
				for(int i = 0; i < LabelDashBoard.Length; i++)
				{
					LabelDashBoard[i] = new Label();
					LabelDashBoard[i].SetBounds(701, yPos, 200, 100);
					LabelDashBoard[i].BackColor = Color.Black;
					LabelDashBoard[i].ForeColor = Color.White;

					LabelDashBoard[i].Font = new Font("Consolas", 30);
					this.Controls.Add(LabelDashBoard[i]);

					yPos += 100;
				}

				LabelDashBoard[0].Text = "[R G]";
				LabelDashBoard[1].Text = "[R A]";
				LabelDashBoard[2].Text = "[R R]";
				LabelDashBoard[3].Text = "[G R]";
				LabelDashBoard[4].Text = "[A R]";
				LabelDashBoard[5].Text = "[R R]";

				foreach(var LabelDash in LabelDashBoard)
					LabelDash.Refresh();

				// initial state of traffic lights
				PanelRoad.BackgroundImage = Image.FromFile(@"../img/crossroadRG.jpg");
				PrevIndex = 6;
				InputIndex = 1;
				OutputIndex = 2;
				LabelDashBoard[InputIndex - 1].BackColor = Color.Yellow;
				LabelDashBoard[InputIndex - 1].ForeColor = Color.Black;

				ButtonControl = new Button();
				ButtonControl.Text = "Start!";
				ButtonControl.Click += new EventHandler(ButtonControl_Clicked);
				ButtonControl.BackColor = Color.Green;
				ButtonControl.FlatStyle = FlatStyle.Flat;
				ButtonControl.ForeColor = Color.White;
				ButtonControl.SetBounds(30, 30, 160, 90);

				this.Controls.Add(ButtonControl);
				this.Controls.Add(PanelRoad);
				//this.Controls.Add(LabelDashBoard);

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message); 
			}
		}

		private void TimerUI_Tick(object sender, EventArgs e)
		{
			ChangeLight();
		}

		private void ButtonControl_Clicked(Object src, EventArgs evt)
		{
			if(TimerUI.Enabled)
			{
				TimerUI.Stop();
				ButtonControl.Text = "Start!";
				ButtonControl.BackColor = Color.Green;
			}
			else
			{
				TimerUI.Start();
				ButtonControl.Text = "Stop!";
				ButtonControl.BackColor = Color.Red;
			}
        }
        #endregion Windows Forms Methods

		public void ChangeLight()
		{
			// make sure the network is trained huh
			var TrafficNN = new NeuralNetwork();

            TrafficNN.HiddenLayers = 1;

            TrafficNN.AllocateNetwork();
            TrafficNN.LoadNetworkFrom(@"../data/weights.csv");

            Console.WriteLine("\nPrevious: {0}", PrevIndex);
            Console.WriteLine("Current: {0}", InputIndex);

            // interpolate inputs before feeding to Network
            double PrevIndexDbl = (double) PrevIndex / 10;
            double InputIndexDbl = (double) InputIndex / 10;

            // no need to interpolate again since the interpolated value will be returned
            OutputIndex = Convert.ToInt32(TrafficNN.FeedToNetwork(PrevIndexDbl, InputIndexDbl));

            // using the Network's output... for real
			switch(OutputIndex)
			{
				case 1: PanelRoad.BackgroundImage = Image.FromFile(@"../img/crossroadRG.jpg"); break;
				case 2: PanelRoad.BackgroundImage = Image.FromFile(@"../img/crossroadRA.jpg"); break;
				case 3: PanelRoad.BackgroundImage = Image.FromFile(@"../img/crossroadRR.jpg"); break;
				case 4: PanelRoad.BackgroundImage = Image.FromFile(@"../img/crossroadGR.jpg"); break;
				case 5: PanelRoad.BackgroundImage = Image.FromFile(@"../img/crossroadAR.jpg"); break;
				case 6: PanelRoad.BackgroundImage = Image.FromFile(@"../img/crossroadRR.jpg"); break;
				
				default: PanelRoad.BackgroundImage = Image.FromFile(@"../img/crossroadRR.jpg"); break;
			}

			LabelDashBoard[InputIndex - 1].ForeColor = Color.White;
			LabelDashBoard[InputIndex - 1].BackColor = Color.Black;

			LabelDashBoard[OutputIndex - 1].BackColor = Color.Yellow;
			LabelDashBoard[OutputIndex - 1].ForeColor = Color.Black;

			// shifting of values
			PrevIndex = InputIndex;
			InputIndex = OutputIndex;

			Console.WriteLine("Next: {0}", OutputIndex);
		}

		public static void Main()
        {
        	Application.Run(new TrafficLightControllerGUI());
        }

        private Panel PanelRoad;
        private Label[] LabelDashBoard;
		private Button ButtonControl;
		private System.Windows.Forms.Timer TimerUI;

		private int PrevIndex;
		private int InputIndex;
		private int OutputIndex;
		
	}

}


        
       
      
       
