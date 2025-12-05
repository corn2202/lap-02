using System;
using System.Windows.Forms;

namespace Lab02_01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PerformCalculation(Func<float, float, float> operation, string operationName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNumber1.Text) || string.IsNullOrWhiteSpace(txtNumber2.Text))
                {
                    MessageBox.Show("Vui lòng nhập cả hai số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                float num1 = float.Parse(txtNumber1.Text);
                float num2 = float.Parse(txtNumber2.Text);

                if (operationName == "Chia" && num2 == 0)
                {
                    MessageBox.Show("Không thể chia cho 0!", "Lỗi toán học", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                float result = operation(num1, num2);
                txtAnswer.Text = result.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập số hợp lệ!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PerformCalculation((a, b) => a + b, "Cộng");
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            PerformCalculation((a, b) => a - b, "Trừ");
        }

        private void btnMul_Click(object sender, EventArgs e)
        {
            PerformCalculation((a, b) => a * b, "Nhân");
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            PerformCalculation((a, b) => a / b, "Chia");
        }
    }
}