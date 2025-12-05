using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab02_03
{
    public partial class Form3 : Form
    {
        private Button[,] seats = new Button[4, 5];
        private bool[,] isSold = new bool[4, 5];
        private bool[,] isSelected = new bool[4, 5];

        public Form3()
        {
            InitializeComponent();
            InitializeSeats();
        }

        private void InitializeSeats()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    int seatNumber = row * 5 + col + 1;
                    Button btn = new Button
                    {
                        Text = seatNumber.ToString(),
                        Width = 50,
                        Height = 50,
                        Location = new Point(50 + col * 60, 80 + row * 60)
                    };
                    btn.Click += Seat_Click;
                    this.Controls.Add(btn);
                    seats[row, col] = btn;
                    isSold[row, col] = false;
                    isSelected[row, col] = false;
                    UpdateSeatColor(row, col);
                }
            }
        }

        private void Seat_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            int row = -1, col = -1;
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (seats[r, c] == clickedButton)
                    {
                        row = r;
                        col = c;
                        break;
                    }
                }
                if (row != -1) break;
            }

            if (isSold[row, col])
            {
                MessageBox.Show("Ghế này đã được bán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isSelected[row, col] = !isSelected[row, col];
            UpdateSeatColor(row, col);
            UpdateTotalPrice();
        }

        private void UpdateSeatColor(int row, int col)
        {
            Button btn = seats[row, col];
            if (isSold[row, col])
                btn.BackColor = Color.Yellow;
            else if (isSelected[row, col])
                btn.BackColor = Color.LightBlue;
            else
                btn.BackColor = Color.White;
        }

        private void UpdateTotalPrice()
        {
            int totalPrice = 0;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (isSelected[row, col])
                    {
                        totalPrice += GetSeatPrice(row);
                    }
                }
            }
            lblTotalPrice.Text = "Thành tiền: " + totalPrice.ToString("N0") + "đ";
        }

        private int GetSeatPrice(int row)
        {
            switch (row)
            {
                case 0: return 30000;
                case 1: return 40000;
                case 2: return 50000;
                case 3: return 80000;
                default: return 0;
            }
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (isSelected[row, col])
                    {
                        isSold[row, col] = true;
                        isSelected[row, col] = false;
                        UpdateSeatColor(row, col);
                    }
                }
            }
            UpdateTotalPrice(); // Sẽ hiển thị 0 sau khi mua xong
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (isSelected[row, col])
                    {
                        isSelected[row, col] = false;
                        UpdateSeatColor(row, col);
                    }
                }
            }
            lblTotalPrice.Text = "Thành tiền: 0đ";
        }

        private void btnKetThuc_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}