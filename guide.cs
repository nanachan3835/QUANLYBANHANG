// đây là code thử nghiệm của mình, không phải là code chính thức

/// <summary>
/// Lớp Functions chứa các hàm kết nối và thao tác với cơ sở dữ liệu
/// dùng đề chứa các phương thức dùng chung
/// </summary>

using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;  



namespace QuanLyBanHang.Class
{
    class Functions
    {
        public static SqlConnection Con;  //Khai báo đối tượng kết nối        

        public static void Connect()
        {
            Con = new SqlConnection();   //Khởi tạo đối tượng
            Con.ConnectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=" + Application.StartupPath + @"\Quanlybanhang.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            Con.Open();                  //Mở kết nối
            //Kiểm tra kết nối
            if (Con.State == ConnectionState.Open)
                MessageBox.Show("Kết nối thành công");
            else MessageBox.Show("Không thể kết nối với dữ liệu");

        }
        public static void Disconnect()
        {
            if (Con.State == ConnectionState.Open)
            {
                Con.Close();   	//Đóng kết nối
                Con.Dispose(); 	//Giải phóng tài nguyên
                Con = null;
            }
        }
}




//////////////////////////////////////// FORM MAIN ////////////////////////////////////////
/// <summary>
///     Lớp FormMain chứa các sự kiện và phương thức xử lý chính của chương trình ở form chính
///     dùng để gọi các form con
///     dùng để xử lý các sự kiện của form chính
/// </summary>


//form main load//


private void frmMain_Load(object sender, EventArgs e)
        {
            Class.Functions.Connect(); //Mở kết nối
        }
//sự kiên menu thoát click

private void mnuThoat_Click(object sender, EventArgs e)
        {
            Class.Functions.Disconnect(); //Đóng kết nối
            Application.Exit(); //Thoát
        }


// hiển thị các form khác với cú pháp
/*
<Tên lớp form> <Tên đối tượng> = new <Tên lớp form>(); //Khởi tạo đối tượng

<Tên đối tượng>.ShowDialog(); //Hiển thị dưới dạng hộp thoại


Hoặc <Tên đối tượng>.Show(); //Hiện thị dạng thông thường
*/

//hiển thị form chất liệu


private void mnuChatLieu_Click(object sender, EventArgs e)
        {
            frmDMChatLieu frmChatLieu = new frmDMChatLieu(); //Khởi tạo đối tượng
            frmChatLieu.ShowDialog(); //Hiển thị
        }


///////////////////////////////////////////// FORM DANH MỤC CHẤT LIỆU ///////////////////////////////////////
/// form chất liệu /// 
/// <summary>
///    Lớp FormDMChatLieu chứa các sự kiện và phương thức xử lý chính của chương trình ở form danh mục chất liệu
///    
/// </summary>

//khai báo 

using System.Data.SqlClient; //Sử dụng thư viện để làm việc SQL server
using QuanLyBanHang.Class; //Sử dụng class Functions.cs


DataTable tblCL; //Chứa dữ liệu bảng Chất liệu

// sự kiện form chất liệu


private void frmDMChatLieu_Load(object sender, EventArgs e)
        {
            txtMaChatLieu.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            LoadDataGridView(); //Hiển thị bảng tblChatLieu
        }


//phương thức load data grid view

private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT MaChatLieu, TenChatLieu FROM tblChatLieu";
            tblCL = Class.Functions.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            dgvChatLieu.DataSource = tblCL; //Nguồn dữ liệu            
            dgvChatLieu.Columns[0].HeaderText = "Mã chất liệu";
            dgvChatLieu.Columns[1].HeaderText = "Mã chất liệu";
            dgvChatLieu.Columns[0].Width = 100;
            dgvChatLieu.Columns[1].Width = 300;
            dgvChatLieu.AllowUserToAddRows = false; //Không cho người dùng thêm dữ liệu trực tiếp
            dgvChatLieu.EditMode = DataGridViewEditMode.EditProgrammatically; //Không cho sửa dữ liệu trực tiếp
        }
// phương thức getdatatoTable


//Lấy dữ liệu vào bảng
//cách1
        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            //Tạo đối tượng thuộc lớp SqlCommand
            dap.SelectCommand = new SqlCommand();
            dap.SelectCommand.Connection = Functions.Con; //Kết nối cơ sở dữ liệu
            dap.SelectCommand.CommandText = sql; //Lệnh SQL
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            dap.Fill(table);
            return table;
        }
//cách 2

//Lấy dữ liệu vào bảng
        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, Con); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            dap.Fill(table); //Đổ kết quả từ câu lệnh sql vào table
            return table;
        }
//phương thức dgv chất liệu click

private void dgvChatLieu_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaChatLieu.Focus();
                return;
            }
            if (tblCL.Rows.Count == 0) //Nếu không có dữ liệu
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtMaChatLieu.Text = dgvChatLieu.CurrentRow.Cells["MaChatLieu"].Value.ToString();
            txtTenChatLieu.Text = dgvChatLieu.CurrentRow.Cells["TenChatLieu"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }


//Phương thức btnThem_Click

    private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValue(); //Xoá trắng các textbox
            txtMaChatLieu.Enabled = true; //cho phép nhập mới
            txtMaChatLieu.Focus();
        }

//phương thức reset value
private void ResetValue()
        {
            txtMaChatLieu.Text = "";
            txtTenChatLieu.Text = "";
        }
//phương thức btnLuu_Click

 private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql; //Lưu lệnh sql
            if (txtMaChatLieu.Text.Trim().Length == 0) //Nếu chưa nhập mã chất liệu
            {
                MessageBox.Show("Bạn phải nhập mã chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaChatLieu.Focus();
                return;
            }
            if(txtTenChatLieu.Text.Trim().Length==0) //Nếu chưa nhập tên chất liệu
            {
                MessageBox.Show("Bạn phải nhập tên chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenChatLieu.Focus();
                return;
            }
            sql = "Select MaChatLieu From tblChatLieu where MaChatLieu=N'" + txtMaChatLieu.Text.Trim() + "'";
            if (Class.Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã chất liệu này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaChatLieu.Focus();                
                return;
            }

            sql = "INSERT INTO tblChatLieu VALUES(N'" + 
                txtMaChatLieu.Text + "',N'" + txtTenChatLieu.Text +"')";
            Class.Functions.RunSQL(sql); //Thực hiện câu lệnh sql
            LoadDataGridView(); //Nạp lại DataGridView
            ResetValue();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaChatLieu.Enabled = false;
        }


// hàm checkkey

//Hàm kiểm tra khoá trùng
        public static bool CheckKey(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql,Con);
            DataTable table = new DataTable();
            dap.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }


// phương thức runsql


//Hàm thực hiện câu lệnh SQL
        public static void RunSQL(string sql)
        {
            SqlCommand cmd; //Đối tượng thuộc lớp SqlCommand
            cmd = new SqlCommand();
            cmd.Connection = Con; //Gán kết nối
            cmd.CommandText = sql; //Gán lệnh SQL
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();//Giải phóng bộ nhớ
            cmd = null;
        }
//phương thức btnSua_Click

private void btnSua_Click(object sender, EventArgs e)
        {
            string sql; //Lưu câu lệnh sql
            if (tblCL.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaChatLieu.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenChatLieu.Text.Trim().Length==0) //nếu chưa nhập tên chất liệu
            {
                MessageBox.Show("Bạn chưa nhập tên chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sql = "UPDATE tblChatLieu SET TenChatLieu=N'" + 
                txtTenChatLieu.Text.ToString() + 
                "' WHERE MaChatLieu=N'" + txtMaChatLieu.Text + "'";
            Class.Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValue();

            btnBoQua.Enabled = false;
        }
//phương thức btnXoa_Click

 private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblCL.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaChatLieu.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE tblChatLieu WHERE MaChatLieu=N'" + txtMaChatLieu.Text + "'";
                Class.Functions.RunSqlDel(sql);
                LoadDataGridView();
                ResetValue();
            }
        }

//phương thức runsqldel

//Hàm thực hiện câu lệnh SQL
        public static void RunSqlDel(string sql)
        {
            SqlCommand cmd = new SqlCommand(); //Đối tượng thuộc lớp SqlCommand
            cmd.Connection = Functions.Con; //Gán kết nối
            cmd.CommandText = sql; //Gán lệnh SQL
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dữ liệu đang được dùng, không thể xóa...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose(); //Giải phóng bộ nhớ
            cmd = null;
        }

//phương thức btnBoQua_Click

private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValue();
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            txtMaChatLieu.Enabled = false;
        }


//phương thức dùng enter thay cho phím tab

private void txtMaChatLieu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

//phương thức btnDong_Click
 
 private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

/////////////////////////////////////////////////FOEM DANH MỤC NHÂN VIÊN///////////////////////////////////////
///
/// <summary>
///   Lớp FormDMNhanVien chứa các sự kiện và phương thức xử lý chính của chương trình ở form danh mục nhân viên
///   
/// </summary>
/// 

//khai báo


using System.Data;
using System.Data.SqlClient;
using QuanLyBanHang.Class;

DataTable tblNV; //Lưu dữ liệu bảng nhân viên


//phương thức frmDMNhanVien_Load


private void frmDMNhanvien_Load(object sender, EventArgs e)
        {
            txtMaNhanVien.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            LoadDataGridView();
        }


//phương thức load data grid view

public void LoadDataGridView()
        {
            string sql;
            sql = "SELECT MaNhanVien,TenNhanVien,GioiTinh,DiaChi,DienThoai,NgaySinh FROm tblNhanVien";
            tblNV = Functions.GetDataToTable(sql); //lấy dữ liệu
            dgvNhanVien.DataSource = tblNV;
            dgvNhanVien.Columns[0].HeaderText = "Mã nhân viên";
            dgvNhanVien.Columns[1].HeaderText = "Tên nhân viên";
            dgvNhanVien.Columns[2].HeaderText = "Giới tính";
            dgvNhanVien.Columns[3].HeaderText = "Địa chỉ";
            dgvNhanVien.Columns[4].HeaderText = "Điện thoại";
            dgvNhanVien.Columns[5].HeaderText = "Ngày sinh";
            dgvNhanVien.Columns[0].Width = 100;
            dgvNhanVien.Columns[1].Width = 150;
            dgvNhanVien.Columns[2].Width = 100;
            dgvNhanVien.Columns[3].Width = 150;
            dgvNhanVien.Columns[4].Width = 100;
            dgvNhanVien.Columns[5].Width = 100;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.EditMode = DataGridViewEditMode.EditProgrammatically;
        }


//phương thức dgvNhanVien_Click

private void dgvNhanVien_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo",  MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNhanVien.Focus();
                return;
            }
            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK,  MessageBoxIcon.Information);
                return;
            }
            txtMaNhanVien.Text = dgvNhanVien.CurrentRow.Cells["MaNhanVien"].Value.ToString();
            txtTenNhanVien.Text = dgvNhanVien.CurrentRow.Cells["TenNhanVien"].Value.ToString();
            if (dgvNhanVien.CurrentRow.Cells["GioiTinh"].Value.ToString() == "Nam") chkGioiTinh.Checked = true;
            else chkGioiTinh.Checked = false;
            txtDiaChi.Text = dgvNhanVien.CurrentRow.Cells["DiaChi"].Value.ToString();
            mtbDienThoai.Text = dgvNhanVien.CurrentRow.Cells["DienThoai"].Value.ToString();
            mskNgaySinh.Text = dgvNhanVien.CurrentRow.Cells["NgaySinh"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnXoa.Enabled = true;
        }
// phương thức btnThem_Click


private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValues();
            txtMaNhanVien.Enabled = true;
            txtMaNhanVien.Focus();
        }


//phương thức reset values

private void ResetValues()
        {
            txtMaNhanVien.Text = "";
            txtTenNhanVien.Text = "";
            chkGioiTinh.Checked = false;
            txtDiaChi.Text = "";
            mskNgaySinh.Text = "";
            mtbDienThoai.Text = "";
        }
// phương thức btnLuu_Click

private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql,gt;
            if (txtMaNhanVien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhanVien.Focus();
                return;
            }
            if (txtTenNhanVien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhanVien.Focus();
                return ;
            }
            if (txtDiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return ;
            }
            if (mtbDienThoai.Text == "(   )     -")
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbDienThoai.Focus();
                return ;
            }
            if (mskNgaySinh.Text == "  /  /")
            {
                MessageBox.Show("Bạn phải nhập ngày sinh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskNgaySinh.Focus();
                return ;
            }
            if (!Functions.IsDate(mskNgaySinh.Text))
            {
                MessageBox.Show("Bạn phải nhập lại ngày sinh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               // mskNgaySinh.Text = "";
                mskNgaySinh.Focus();
                return ;
            }
            if (chkGioiTinh.Checked == true)
                gt = "Nam";
            else
                gt = "Nữ";
            sql = "SELECT MaNhanVien FROM tblNhanVien WHERE MaNhanVien=N'" + txtMaNhanVien.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã nhân viên này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhanVien.Focus();
                txtMaNhanVien.Text = "";
                return ;
            }
            sql = "INSERT INTO tblNhanVien(MaNhanVien,TenNhanVien,GioiTinh, DiaChi,DienThoai, NgaySinh) VALUES (N'" + txtMaNhanVien.Text.Trim() + "',N'" + txtTenNhanVien.Text.Trim() + "',N'" + gt + "',N'" + txtDiaChi.Text.Trim() + "','" + mtbDienThoai.Text + "','" + Functions.ConvertDateTime(mskNgaySinh.Text) + "')";
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaNhanVien.Enabled = false;
        }

//hàm is date


public static bool IsDate(string date)
        {
            string[] elements = date.Split('/');
            if ((Convert.ToInt32(elements[0]) >= 1) && (Convert.ToInt32(elements[0]) <= 31) && (Convert.ToInt32(elements[1]) >= 1) && (Convert.ToInt32(elements[1]) <= 12) && (Convert.ToInt32(elements[2]) >= 1900))
                return true;
            else return false;
        }

//hàm convert date time

public static string ConvertDateTime(string date)
        {
            string[] elements = date.Split('/');
            string dt = string.Format("{0}/{1}/{2}", elements[0], elements[1], elements[2]);
            return dt;
        }
//phương thức btnSua_Click

private void btnSua_Click(object sender, EventArgs e)
        {
            string sql, gt;
            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (txtMaNhanVien.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenNhanVien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhanVien.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return;
            }
            if (mtbDienThoai.Text == "(   )     -")
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbDienThoai.Focus();
                return;
            }
            if (mskNgaySinh.Text == "  /  /")
            {
                MessageBox.Show("Bạn phải nhập ngày sinh", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskNgaySinh.Focus();
                return;
            }
            if (!Functions.IsDate(mskNgaySinh.Text))
            {
                MessageBox.Show("Bạn phải nhập lại ngày sinh", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskNgaySinh.Text = "";
                mskNgaySinh.Focus();
                return;
            }
            if (chkGioiTinh.Checked == true)
                gt = "Nam";
            else
                gt = "Nữ";
            sql = "UPDATE tblNhanVien SET  TenNhanVien=N'" +txtTenNhanVien.Text.Trim().ToString() +
                    "',DiaChi=N'" + txtDiaChi.Text.Trim().ToString() +
                    "',DienThoai='" + mtbDienThoai.Text.ToString() + "',GioiTinh=N'" + gt +
                    "',NgaySinh='" + Functions.ConvertDateTime(mskNgaySinh.Text) +
                    "' WHERE MaNhanVien=N'" + txtMaNhanVien.Text + "'";
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnBoQua.Enabled = false;
        }


//phuong thức btnXoa_Click 
private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (txtMaNhanVien.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo",MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "DELETE tblNhanVien WHERE MaNhanVien=N'" + txtMaNhanVien.Text + "'";
                Functions.RunSqlDel(sql);
                LoadDataGridView();
                ResetValues();
            }
        }

//phương thức btnBoQua_Click


private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaNhanVien.Enabled = false;
        }

//phương thức dùng enter thay tab

private void txtMaNhanVien_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }


//phương thức btn đóng 


private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }


////////////////////////////////////////// FORM DANH MỤC KHÁCH HÀNG ////////////////////////////////////////
/// 
/// <summary>
///  Lớp FormDMKhachHang chứa các sự kiện và phương thức xử lý chính của chương trình ở form danh mục khách hàng
///  
/// </summary>
/// 

//khai báo

DataTable tblKH; //Bảng khách hàng

//phương thức frmDMKhachHang_Load
private void frmDMKhachHang_Load(object sender, EventArgs e)
        {
            txtMaKhach.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            LoadDataGridView();
        }

//phương thức load data grid view


private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * from tblKhach";
            tblKH = Functions.GetDataToTable(sql); //Lấy dữ liệu từ bảng
            dgvKhachHang.DataSource = tblKH; //Hiển thị vào dataGridView
            dgvKhachHang.Columns[0].HeaderText = "Mã khách";
            dgvKhachHang.Columns[1].HeaderText = "Tên khách";
            dgvKhachHang.Columns[2].HeaderText = "Địa chỉ";
            dgvKhachHang.Columns[3].HeaderText = "Điện thoại";
            dgvKhachHang.Columns[0].Width = 100;
            dgvKhachHang.Columns[1].Width = 150;
            dgvKhachHang.Columns[2].Width = 150;
            dgvKhachHang.Columns[3].Width = 150;
            dgvKhachHang.AllowUserToAddRows = false;
            dgvKhachHang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
//phương thức dgvKhachHang_Click


private void dgvKhachHang_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKhach.Focus();
                return;
            }
            if (tblKH.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtMaKhach.Text = dgvKhachHang.CurrentRow.Cells["MaKhach"].Value.ToString();
            txtTenKhach.Text = dgvKhachHang.CurrentRow.Cells["TenKhach"].Value.ToString();
            txtDiaChi.Text = dgvKhachHang.CurrentRow.Cells["DiaChi"].Value.ToString();
            mtbDienThoai.Text = dgvKhachHang.CurrentRow.Cells["DienThoai"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }
// phương thức btnThem_Click


private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValues();
            txtMaKhach.Enabled = true;
            txtMaKhach.Focus();
        }

//phương thức reset values

private void ResetValues()
        {
            txtMaKhach.Text = "";
            txtTenKhach.Text  = "";
            txtDiaChi.Text = "";
            mtbDienThoai.Text = "";
        }


// phương thức btnLuu_Click

private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaKhach.Text.Trim().Length  == 0)
            {
                MessageBox.Show("Bạn phải nhập mã khách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKhach.Focus();
                return;
            }
            if (txtTenKhach.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên khách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenKhach.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDiaChi.Focus();
                return;
            }
            if (mtbDienThoai.Text == "(  )    -")
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mtbDienThoai.Focus();
                return;
            }
            //Kiểm tra đã tồn tại mã khách chưa
            sql = "SELECT MaKhach FROM tblKhach WHERE MaKhach=N'" + txtMaKhach.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã khách này đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKhach.Focus();
                return;                
            }
            //Chèn thêm
            sql = "INSERT INTO tblKhach VALUES (N'" + txtMaKhach.Text.Trim() +
                "',N'" + txtTenKhach.Text.Trim() + "',N'" + txtDiaChi.Text.Trim() + "','" + mtbDienThoai.Text + "')";
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();

            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaKhach.Enabled = false;
        }
    

//phương thức btnSua_Click

private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblKH.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaKhach.Text == "")
            {
                MessageBox.Show("Bạn phải chọn bản ghi cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenKhach.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên khách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenKhach.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDiaChi.Focus();
                return;
            }
            if (mtbDienThoai.Text=="(  )    -")
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mtbDienThoai.Focus();
                return;
            }
            sql = "UPDATE tblKhach SET TenKhach=N'" + txtTenKhach.Text.Trim().ToString() + "',DiaChi=N'" +
                txtDiaChi.Text.Trim().ToString() + "',DienThoai='" + mtbDienThoai.Text.ToString() +
                "' WHERE MaKhach=N'" + txtMaKhach.Text + "'";
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnBoQua.Enabled = false;
        }


//phương thức btnXoa_Click

private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblKH.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaKhach.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá bản ghi này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE tblKhach WHERE MaKhach=N'" + txtMaKhach.Text + "'";
                Functions.RunSqlDel(sql);
                LoadDataGridView();
                ResetValues();
            }
        }
// phương thức btnBoQua_Click


private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            txtMaKhach.Enabled = false;
        }

//phương thức dùng enter thay tab


private void txtMaKhach_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }


//Phương thức btnDong_Click


private void btnDong_Click(object sender, EventArgs e)
{
this.Close();
}