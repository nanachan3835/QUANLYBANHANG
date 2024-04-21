using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;     // Sử dụng đối tượng MessageBox


namespace QLBanHang.Class
{
    internal class Function
    {
        public static SqlConnection Con;  //Khai báo đối tượng kết nối        

        public static void Connect()
        {
            Con = new SqlConnection();   //Khởi tạo đối tượng
            //Con.ConnectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=" + Application.StartupPath + @"\Quanlybanhang.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            Con.ConnectionString = Properties.Settings.Default.QLBanHangConnectionString;   
                              //Mở kết nối
            //Kiểm tra kết nối
            if (Con.State != ConnectionState.Open)
            {
                Con.Open();
                MessageBox.Show("Kết nối thành công");
            }
            else
            {
                MessageBox.Show("Không thể kết nối với dữ liệu");
            }
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

        //phuong thuc thuc thi cau lenh select lay du lieu
        public static DataTable GetDataToTable(String sql)
        {
           DataTable table  = new DataTable();
           SqlDataAdapter dap = new SqlDataAdapter(sql,Con);
           dap.Fill(table);
           return table;
        }

    }

}
