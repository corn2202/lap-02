using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Lab02_04
{
    public partial class Form4 : Form
    {
        private List<Account> accounts = new List<Account>();

        public Form4()
        {
            InitializeComponent();
        }

        private void btnThemCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoTaiKhoan.Text) ||
                string.IsNullOrWhiteSpace(txtTenKhachHang.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtSoTien.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtSoTien.Text, out decimal soTien) || soTien < 0)
            {
                MessageBox.Show("Số tiền không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string soTaiKhoan = txtSoTaiKhoan.Text.Trim();
            string tenKhachHang = txtTenKhachHang.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            // Kiểm tra xem có tồn tại số tài khoản chưa
            int index = accounts.FindIndex(a => a.SoTaiKhoan == soTaiKhoan);

            if (index == -1)
            {
                // Thêm mới
                accounts.Add(new Account(soTaiKhoan, tenKhachHang, diaChi, soTien));
                AddItemToListView(soTaiKhoan, tenKhachHang, diaChi, soTien);
                MessageBox.Show("Thêm mới dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Cập nhật
                accounts[index].TenKhachHang = tenKhachHang;
                accounts[index].DiaChi = diaChi;
                accounts[index].SoTien = soTien;

                // Cập nhật lại ListView
                ListViewItem item = lvAccounts.Items[index];
                item.SubItems[1].Text = tenKhachHang;
                item.SubItems[2].Text = diaChi;
                item.SubItems[3].Text = soTien.ToString("N0");

                MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            UpdateTotalMoney();
            ClearInputs();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoTaiKhoan.Text))
            {
                MessageBox.Show("Vui lòng nhập số tài khoản cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string soTaiKhoan = txtSoTaiKhoan.Text.Trim();
            int index = accounts.FindIndex(a => a.SoTaiKhoan == soTaiKhoan);

            if (index == -1)
            {
                MessageBox.Show("Không tìm thấy số tài khoản cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa tài khoản {soTaiKhoan}?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                accounts.RemoveAt(index);
                lvAccounts.Items.RemoveAt(index);
                MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateTotalMoney();
                ClearInputs();
            }
        }

        private void btnRutTien_Click(object sender, EventArgs e)
        {
            if (lvAccounts.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để rút tiền!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ListViewItem selectedItem = lvAccounts.SelectedItems[0];
            int index = lvAccounts.SelectedIndices[0];
            Account acc = accounts[index];

            if (!decimal.TryParse(txtSoTien.Text, out decimal soTienRut) || soTienRut <= 0)
            {
                MessageBox.Show("Số tiền rút không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (soTienRut > acc.SoTien)
            {
                MessageBox.Show("Số tiền rút vượt quá số dư trong tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            acc.SoTien -= soTienRut;
            selectedItem.SubItems[3].Text = acc.SoTien.ToString("N0");
            MessageBox.Show($"Rút {soTienRut:N0}đ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateTotalMoney();
        }

        private void btnNapTien_Click(object sender, EventArgs e)
        {
            if (lvAccounts.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để nạp tiền!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ListViewItem selectedItem = lvAccounts.SelectedItems[0];
            int index = lvAccounts.SelectedIndices[0];
            Account acc = accounts[index];

            if (!decimal.TryParse(txtSoTien.Text, out decimal soTienNap) || soTienNap <= 0)
            {
                MessageBox.Show("Số tiền nạp không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            acc.SoTien += soTienNap;
            selectedItem.SubItems[3].Text = acc.SoTien.ToString("N0");
            MessageBox.Show($"Nạp {soTienNap:N0}đ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateTotalMoney();
        }

        private void lvAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAccounts.SelectedItems.Count > 0)
            {
                ListViewItem item = lvAccounts.SelectedItems[0];
                int index = lvAccounts.SelectedIndices[0];

                txtSoTaiKhoan.Text = item.SubItems[0].Text;
                txtTenKhachHang.Text = item.SubItems[1].Text;
                txtDiaChi.Text = item.SubItems[2].Text;
                txtSoTien.Text = item.SubItems[3].Text.Replace(",", "").Replace(".", "");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddItemToListView(string soTaiKhoan, string ten, string diaChi, decimal soTien)
        {
            ListViewItem item = new ListViewItem(soTaiKhoan);
            item.SubItems.Add(ten);
            item.SubItems.Add(diaChi);
            item.SubItems.Add(soTien.ToString("N0"));
            lvAccounts.Items.Add(item);
        }

        private void UpdateTotalMoney()
        {
            decimal total = accounts.Sum(a => a.SoTien);
            lblTongTien.Text = total.ToString("N0") + "đ";
        }

        private void ClearInputs()
        {
            txtSoTaiKhoan.Clear();
            txtTenKhachHang.Clear();
            txtDiaChi.Clear();
            txtSoTien.Clear();
            txtSoTaiKhoan.Focus();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // Khởi tạo cột cho ListView
            lvAccounts.View = View.Details;
            lvAccounts.FullRowSelect = true;
            lvAccounts.GridLines = true;

            lvAccounts.Columns.Add("STT", 50);
            lvAccounts.Columns.Add("Mã tài khoản", 120);
            lvAccounts.Columns.Add("Tên khách hàng", 150);
            lvAccounts.Columns.Add("Địa chỉ", 200);
            lvAccounts.Columns.Add("Số tiền", 100);

            // ➕ THÊM DỮ LIỆU MẪU
            AddSampleAccounts();
            UpdateTotalMoney();
        }

        private void AddSampleAccounts()
        {
            // Thêm vào List
            accounts.Add(new Account("001", "Nguyễn Văn A", "Hà Nội", 5000000));
            accounts.Add(new Account("002", "Trần Thị B", "TP.HCM", 10000000));
            accounts.Add(new Account("003", "Lê Văn C", "Đà Nẵng", 3000000));

            // Thêm vào ListView
            AddItemToListView("001", "Nguyễn Văn A", "Hà Nội", 5000000);
            AddItemToListView("002", "Trần Thị B", "TP.HCM", 10000000);
            AddItemToListView("003", "Lê Văn C", "Đà Nẵng", 3000000);
        }
    }

    public class Account
    {
        public string SoTaiKhoan { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public decimal SoTien { get; set; }

        public Account(string soTaiKhoan, string tenKhachHang, string diaChi, decimal soTien)
        {
            SoTaiKhoan = soTaiKhoan;
            TenKhachHang = tenKhachHang;
            DiaChi = diaChi;
            SoTien = soTien;
        }
    }
}