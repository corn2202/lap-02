using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Lab02_02
{
    public partial class Form2 : Form
    {
        private DataTable dtStudents;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();
            InitializeComboBox();

            // ➕ THÊM DỮ LIỆU MẪU TẠI ĐÂY
            AddSampleStudents();
        }

        private void AddSampleStudents()
        {
            dtStudents.Rows.Add("210101", "Nguyễn Văn A", "Nam", "QTKD");
            dtStudents.Rows.Add("210102", "Trần Thị B", "Nữ", "CNTT");
            dtStudents.Rows.Add("210103", "Lê Văn C", "Nam", "NNA");
        }

        private void InitializeDataGridView()
        {
            dtStudents = new DataTable();
            dtStudents.Columns.Add("MSSV", typeof(string));
            dtStudents.Columns.Add("Họ tên", typeof(string));
            dtStudents.Columns.Add("Giới tính", typeof(string));
            dtStudents.Columns.Add("Chuyên ngành", typeof(string));

            dgvStudents.DataSource = dtStudents;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.ReadOnly = true;
        }

        private void InitializeComboBox()
        {
            // ComboBox nhập liệu (dùng khi thêm/sửa)
            cmbChuyenNganh.Items.AddRange(new string[] { "QTKD", "CNTT", "NNA" });
            if (cmbChuyenNganh.Items.Count > 0)
                cmbChuyenNganh.SelectedIndex = 0;

            // ComboBox lọc chuyên ngành
            cmbLocChuyenNganh.Items.AddRange(new string[] { "Tất cả", "QTKD", "CNTT", "NNA" });
            cmbLocChuyenNganh.SelectedIndex = 0;
            cmbLocChuyenNganh.SelectedIndexChanged += CmbLocChuyenNganh_SelectedIndexChanged;
        }

        // =============== THÊM ===============
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMSSV.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập MSSV và Họ tên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string mssv = txtMSSV.Text.Trim();
            if (dtStudents.AsEnumerable().Any(row => row.Field<string>("MSSV") == mssv))
            {
                MessageBox.Show("MSSV đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hoTen = txtHoTen.Text.Trim();
            string gioiTinh = chkNam.Checked ? "Nam" : "Nữ";
            string chuyenNganh = cmbChuyenNganh.Text;

            dtStudents.Rows.Add(mssv, hoTen, gioiTinh, chuyenNganh);
            MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearInputs();
            ApplyFilter(); // Cập nhật lọc sau khi thêm
        }

        // =============== SỬA ===============
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMSSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string currentMSSV = dgvStudents.SelectedRows[0].Cells["MSSV"].Value.ToString();
            string newMSSV = txtMSSV.Text.Trim();

            // Nếu đổi MSSV → kiểm tra trùng
            if (newMSSV != currentMSSV && dtStudents.AsEnumerable().Any(r => r.Field<string>("MSSV") == newMSSV))
            {
                MessageBox.Show("MSSV mới đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật dòng dữ liệu
            var row = dtStudents.AsEnumerable().First(r => r.Field<string>("MSSV") == currentMSSV);
            row.SetField("MSSV", newMSSV);
            row.SetField("Họ tên", txtHoTen.Text.Trim());
            row.SetField("Giới tính", chkNam.Checked ? "Nam" : "Nữ");
            row.SetField("Chuyên ngành", cmbChuyenNganh.Text);

            MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearInputs();
            ApplyFilter();
        }

        // =============== XÓA ===============
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string mssv = dgvStudents.SelectedRows[0].Cells["MSSV"].Value.ToString();
            DialogResult result = MessageBox.Show($"Xác nhận xóa sinh viên có MSSV: {mssv}?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var row = dtStudents.AsEnumerable().First(r => r.Field<string>("MSSV") == mssv);
                dtStudents.Rows.Remove(row);
                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                ApplyFilter();
            }
        }

        // =============== LỌC ===============
        private void CmbLocChuyenNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string selected = cmbLocChuyenNganh.SelectedItem.ToString();
            if (selected == "Tất cả")
            {
                dgvStudents.DataSource = dtStudents;
            }
            else
            {
                var filtered = dtStudents.AsEnumerable()
                    .Where(row => row.Field<string>("Chuyên ngành") == selected)
                    .CopyToDataTable();

                dgvStudents.DataSource = filtered;
            }
        }

        // =============== HIỂN THỊ KHI CHỌN ===============
        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                var row = dgvStudents.SelectedRows[0];
                txtMSSV.Text = row.Cells["MSSV"].Value.ToString();
                txtHoTen.Text = row.Cells["Họ tên"].Value.ToString();
                chkNam.Checked = row.Cells["Giới tính"].Value.ToString() == "Nam";
                chkNu.Checked = !chkNam.Checked;
                cmbChuyenNganh.Text = row.Cells["Chuyên ngành"].Value.ToString();
            }
        }

        // =============== THOÁT ===============
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // =============== GIỚI TÍNH (chỉ chọn 1) ===============
        private void chkNam_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNam.Checked) chkNu.Checked = false;
        }

        private void chkNu_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNu.Checked) chkNam.Checked = false;
        }

        // =============== XÓA INPUT ===============
        private void ClearInputs()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            chkNam.Checked = true;
            chkNu.Checked = false;
            if (cmbChuyenNganh.Items.Count > 0)
                cmbChuyenNganh.SelectedIndex = 0;
            txtMSSV.Focus();
        }
    }
}