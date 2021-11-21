using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _15211TT0065
{
    public partial class TimKiemKhachHang : Form
    {
        //connection database
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");
        public TimKiemKhachHang()
        {
            InitializeComponent();
        }

        //Created by Vu Dinh khanh - 31/05/2018 : xác nhận và đóng form
        private void TimKiemKhachHang_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (Main.flag == false)
            {
                DialogResult dr = MessageBox.Show("Bạn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : tìm thông tin khách hàng
        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = null;
                SqlParameter para = null;

                //chọn phương pháp tìm kiếm
                if (rbtMa.Checked)
                {
                    cmd = new SqlCommand("sp_SearchClientByCode", con);
                    cmd.CommandText = "sp_SearchClientByCode";
                    para = new SqlParameter("@MaKH", txtText.Text);
                }
                else
                {
                    cmd = new SqlCommand("sp_SearchClientByName", con);
                    cmd.CommandText = "sp_SearchClientByName";
                    para = new SqlParameter("@TenKH", txtText.Text);
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(para);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                dgList.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Loi " + ex.Message, "Thong bao");
            }
            finally
            {
                con.Close(); // dong ket noi
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : load dữ liệu lên form
        private void TimKiemKhachHang_Load(object sender, EventArgs e)
        {
            dgList.DataSource = KhachHang.LayDSKH(con);
        }
    }
}
