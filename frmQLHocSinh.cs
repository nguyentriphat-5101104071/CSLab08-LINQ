using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace CSLab08_LINQ
{
    public class frmQLHocSinh : Form
    {
        private readonly QLHSDataContext db = new QLHSDataContext();
        private TextBox txtMaHS, txtTenHS, txtDiaChi, txtDiemTB, txtTimTen;
        private ComboBox cboLop;
        private DateTimePicker dtpNgaySinh;
        private DataGridView dgvHocSinh;

        public frmQLHocSinh()
        {
            InitializeForm();
            Load += frmQLHocSinh_Load;
        }

        private void InitializeForm()
        {
            Text = "Quản lý học sinh - LINQ";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(850, 580);
            Size = new Size(980, 660);
            Font = new Font("Segoe UI", 10);

            var title = new Label { Text = "THÔNG TIN HỌC SINH", Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.Firebrick, AutoSize = true, Location = new Point(300, 15) };
            Controls.Add(title);

            AddLabel("Mã học sinh", 25, 75); txtMaHS = AddTextBox(150, 72, 180);
            AddLabel("Lớp", 370, 75);
            cboLop = new ComboBox { Location = new Point(430, 72), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cboLop);

            AddLabel("Tên học sinh", 25, 120); txtTenHS = AddTextBox(150, 117, 480);
            AddLabel("Ngày sinh", 25, 165);
            dtpNgaySinh = new DateTimePicker { Location = new Point(150, 162), Width = 180, Format = DateTimePickerFormat.Short };
            Controls.Add(dtpNgaySinh);

            AddLabel("Địa chỉ", 25, 210); txtDiaChi = AddTextBox(150, 207, 480);
            AddLabel("Điểm trung bình", 25, 255); txtDiemTB = AddTextBox(150, 252, 180);

            var group = new GroupBox { Text = "Chức năng", Location = new Point(680, 65), Size = new Size(240, 250) };
            Controls.Add(group);
            AddButton(group, "Thêm", 25, 30, btnThem_Click);
            AddButton(group, "Sửa", 25, 75, btnSua_Click);
            AddButton(group, "Xóa", 25, 120, btnXoa_Click);
            AddButton(group, "Làm mới", 25, 165, (s, e) => ClearInputs());

            AddLabel("Tìm theo tên", 25, 325);
            txtTimTen = AddTextBox(150, 322, 300);
            var btnTim = new Button { Text = "Tìm", Location = new Point(465, 320), Width = 85 };
            btnTim.Click += (s, e) => FillHocSinhByLop();
            Controls.Add(btnTim);

            dgvHocSinh = new DataGridView {
                Location = new Point(25, 365), Size = new Size(895, 230), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false, RowHeadersVisible = false
            };
            dgvHocSinh.SelectionChanged += dgvHocSinh_SelectionChanged;
            Controls.Add(dgvHocSinh);
        }

        private void AddLabel(string text, int x, int y)
        {
            Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true });
        }

        private TextBox AddTextBox(int x, int y, int width)
        {
            var box = new TextBox { Location = new Point(x, y), Width = width };
            Controls.Add(box);
            return box;
        }

        private void AddButton(Control parent, string text, int x, int y, EventHandler handler)
        {
            var button = new Button { Text = text, Location = new Point(x, y), Width = 175, Height = 34 };
            button.Click += handler;
            parent.Controls.Add(button);
        }

        private void frmQLHocSinh_Load(object sender, EventArgs e)
        {
            try
            {
                cboLop.DataSource = db.Lops.ToList();
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";
                cboLop.SelectedIndexChanged += (s, args) => FillHocSinhByLop();
                FillHocSinhByLop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu. Hãy kiểm tra SQL Server và chuỗi kết nối.\n" + ex.Message,
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillHocSinhByLop()
        {
            if (cboLop.SelectedValue == null || !(cboLop.SelectedValue is string))
                return;

            string maLop = cboLop.SelectedValue.ToString();
            string ten = txtTimTen.Text.Trim();
            var data = db.HocSinhs
                .Where(h => h.MaLop == maLop && h.TenHS.Contains(ten))
                .Select(h => new { h.MaHS, h.TenHS, h.NgaySinh, h.DiaChi, h.DTB, h.MaLop })
                .ToList();
            dgvHocSinh.DataSource = data;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(true)) return;
            if (db.HocSinhs.Any(h => h.MaHS == txtMaHS.Text.Trim()))
            {
                MessageBox.Show("Mã học sinh đã tồn tại.");
                return;
            }

            var hs = new HocSinh {
                MaHS = txtMaHS.Text.Trim(),
                TenHS = txtTenHS.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value.Date,
                DiaChi = txtDiaChi.Text.Trim(),
                DTB = float.Parse(txtDiemTB.Text.Trim()),
                MaLop = cboLop.SelectedValue.ToString()
            };
            db.HocSinhs.InsertOnSubmit(hs);
            db.SubmitChanges();
            FillHocSinhByLop();
            ClearInputs();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(false)) return;
            var hs = db.HocSinhs.SingleOrDefault(h => h.MaHS == txtMaHS.Text.Trim());
            if (hs == null) { MessageBox.Show("Không tìm thấy học sinh."); return; }

            hs.TenHS = txtTenHS.Text.Trim();
            hs.NgaySinh = dtpNgaySinh.Value.Date;
            hs.DiaChi = txtDiaChi.Text.Trim();
            hs.DTB = float.Parse(txtDiemTB.Text.Trim());
            hs.MaLop = cboLop.SelectedValue.ToString();
            db.SubmitChanges();
            FillHocSinhByLop();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var hs = db.HocSinhs.SingleOrDefault(h => h.MaHS == txtMaHS.Text.Trim());
            if (hs == null) { MessageBox.Show("Không tìm thấy học sinh."); return; }
            if (MessageBox.Show("Bạn có chắc muốn xóa học sinh này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            db.HocSinhs.DeleteOnSubmit(hs);
            db.SubmitChanges();
            FillHocSinhByLop();
            ClearInputs();
        }

        private bool ValidateInputs(bool requireId)
        {
            if ((requireId && string.IsNullOrWhiteSpace(txtMaHS.Text)) ||
                string.IsNullOrWhiteSpace(txtTenHS.Text) || cboLop.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập mã (khi thêm), tên học sinh và chọn lớp.");
                return false;
            }
            float score;
            if (!float.TryParse(txtDiemTB.Text.Trim(), out score) || score < 0 || score > 10)
            {
                MessageBox.Show("Điểm trung bình phải là số từ 0 đến 10.");
                return false;
            }
            return true;
        }

        private void dgvHocSinh_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHocSinh.CurrentRow == null || dgvHocSinh.CurrentRow.IsNewRow) return;
            var row = dgvHocSinh.CurrentRow;
            if (row.Cells["MaHS"].Value == null) return;
            txtMaHS.Text = Convert.ToString(row.Cells["MaHS"].Value);
            txtTenHS.Text = Convert.ToString(row.Cells["TenHS"].Value);
            if (row.Cells["NgaySinh"].Value != null)
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
            txtDiaChi.Text = Convert.ToString(row.Cells["DiaChi"].Value);
            txtDiemTB.Text = Convert.ToString(row.Cells["DTB"].Value);
            if (row.Cells["MaLop"].Value != null)
                cboLop.SelectedValue = Convert.ToString(row.Cells["MaLop"].Value);
        }

        private void ClearInputs()
        {
            txtMaHS.Clear();
            txtTenHS.Clear();
            txtDiaChi.Clear();
            txtDiemTB.Clear();
            dtpNgaySinh.Value = DateTime.Today;
            txtMaHS.Focus();
        }
    }
}
