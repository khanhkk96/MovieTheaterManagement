using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _15211TT0065
{
    public partial class TimKiemVe : Form
    {
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");

        public TimKiemVe()
        {
            InitializeComponent();
        }

        //Created by Vu Dinh khanh - 31/05/2018 : xác nhận và đóng form
        private void TimKiemVe_FormClosing(object sender, FormClosingEventArgs e)
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

        //Created by Vu Dinh khanh - 31/05/2018 : load dữ liệu lên form
        private void TimKiemVe_Load(object sender, EventArgs e)
        {
            dgList.DataSource = Ve.LayDSVe(con);
        }

        //Created by Vu Dinh khanh - 31/05/2018 : tìm kiếm vé đa bán ra
        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = null;
                SqlParameter para = null;

                //chọn phương pháp tìm kiếm
                if (rbtMaKH.Checked)
                {
                    cmd = new SqlCommand("sp_SearchTicketByClient", con);
                    cmd.CommandText = "sp_SearchTicketByClient";
                    para = new SqlParameter("@KH", txtText.Text);
                }
                else
                {
                    cmd = new SqlCommand("sp_SearchTicketByCode", con);
                    cmd.CommandText = "sp_SearchTicketByCode";
                    para = new SqlParameter("@MaVe", txtText.Text);
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
    }
}
