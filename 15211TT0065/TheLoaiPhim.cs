using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using _15211TT0065.Model;

namespace _15211TT0065
{
    public partial class TheLoaiPhim : Form
    {
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");
        static string id;
        public TheLoaiPhim()
        {
            InitializeComponent();
        }

        //Created by Vu Dinh khanh - 31/05/2018 : lấy danh sách danh mục phim
        public static DataTable LayDSTL(SqlConnection con)
        {
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayDSTheLoai", con);
                cmd.CommandText = "sp_LayDSTheLoai";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[dt.Rows.Count - 1].ItemArray[0].ToString();
                }
                return dt; // tra ve bang
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Loi " + ex.Message, "Thong bao");
                return null;
            }
            finally
            {
                con.Close(); // dong ket noi
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : thêm danh mục phim mới
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int kq = 0;
            try
            {
                //kiểm tra dữ liệu hiển thị trên textbox
                if (txtMa.Text.Length == 0 || txtTen.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Nhập đầy đủ thông tin!");
                    return;
                }

                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_themTheLoai", con);
                cmd.CommandText = "sp_themTheLoai";
                cmd.CommandType = CommandType.StoredProcedure;

                //parameters cho store procedure
                SqlParameter para_ma = new SqlParameter("@MaTheLoai", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@TenTheLoaiPhim", txtTen.Text.Trim());
                cmd.Parameters.Add(para_ten);

                // thuc thi thanh cong cong hay khong?
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Thêm thông tin thành công!", "Thông báo");
                    clear();
                    kq++;
                }
                else
                {
                    MessageBox.Show("Thêm thông tin không thành công!", "Thông báo");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra! \n" + ex.Message, "Thông báo");
            }
            finally
            {
                // dong ket noi
                con.Close();
            }

            //kiểm tra kết quả và load lại dữ liệu
            if (kq != 0)
            {
                dgvList.DataSource = LayDSTL(con);
                AutoCode code = new AutoCode("TL", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : xóa danh mục phim
        private void btnDel_Click(object sender, EventArgs e)
        {
            //xác nhận yêu cầu và xóa
            DialogResult result = MessageBox.Show("Bạn muốn xóa thể loại phim này?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int kq = 0;
                try
                {
                    // mo ket noi
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_xoaTheLoai", con);
                    cmd.CommandText = "sp_xoaTheLoai";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //tham số truyền cho store procedure
                    SqlParameter para_ma = new SqlParameter("@ma", txtMa.Text);
                    cmd.Parameters.Add(para_ma);

                    // thuc thi thanh cong cong hay khong?
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo");
                        clear();
                        kq++;
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công!", "Thông báo");
                    }
                }
                catch (Exception ex)
                {
                    //try
                    //{
                    //    SqlCommand cmd = new SqlCommand("sp_xoaTL2", con);
                    //    cmd.CommandText = "sp_xoaTL2";
                    //    cmd.CommandType = CommandType.StoredProcedure;

                    //    SqlParameter para_ma = new SqlParameter("@ma", txtMa.Text);
                    //    cmd.Parameters.Add(para_ma);

                    //    // thuc thi thanh cong cong hay khong?
                    //    if (cmd.ExecuteNonQuery() > 0)
                    //    {
                    //        MessageBox.Show("Xóa thành công!", "Thông báo");
                    //        clear();
                    //        kq++;
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Xóa không thành công!", "Thông báo");
                    //    }
                    //}
                    //catch (Exception ex2)
                    //{
                        MessageBox.Show("Có lỗi xảy ra! \n" + ex.Message, "Thông báo");
                    //}
                }
                finally
                {
                    // dong ket noi
                    con.Close();
                }

                //kiểm tra kết quả và load lại dữ liệu
                if (kq != 0)
                {
                    dgvList.DataSource = LayDSTL(con);
                    AutoCode code = new AutoCode("TL", id);
                    txtMa.Text = code.ToString();
                }
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : sửa tên danh mục phim
        private void btnEdit_Click(object sender, EventArgs e)
        {
            int kq = 0;
            try
            {
                if (txtMa.Text.Length == 0 || txtTen.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Nhập đầy đủ thông tin!");
                    return;
                }

                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SuaTheLoai", con);
                cmd.CommandText = "sp_SuaTheLoai";
                cmd.CommandType = CommandType.StoredProcedure;

                //tham số truyền cho store procedure
                SqlParameter para_ma = new SqlParameter("@MaTheLoai", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@TenTheLoaiPhim", txtTen.Text.Trim());
                cmd.Parameters.Add(para_ten);

                // thuc thi thanh cong cong hay khong?
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Sửa thông tin thành công!", "Thông báo");
                    clear();
                    kq++;
                }
                else
                {
                    MessageBox.Show("Sửa thông tin không thành công!", "Thông báo");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra! \n" + ex.Message, "Thông báo");
            }
            finally
            {
                // dong ket noi
                con.Close();
            }

            //kiểm tra kết quả và load lại dữ liệu
            if (kq != 0)
            {
                dgvList.DataSource = LayDSTL(con);
                AutoCode code = new AutoCode("TL", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : xóa dữ liệu đang hiển thị trên textbox
        private void clear()
        {
            txtMa.Clear();
            txtTen.Clear();
        }

        //Created by Vu Dinh khanh - 31/05/2018 : load dữ liệu lên textbox tương ứng dữ liệu với dòng đã chọn
        private void dgList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int dong = dgvList.CurrentCell.RowIndex;
            txtMa.Text = dgvList.Rows[dong].Cells[0].Value.ToString();
            txtTen.Text = dgvList.Rows[dong].Cells[1].Value.ToString();
            if (dong > dgvList.RowCount - 2)
            {
                AutoCode code = new AutoCode("TL", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : load dữ liệu lên form
        private void TheLoaiPhim_Load(object sender, EventArgs e)
        {
            dgvList.DataSource = LayDSTL(con);
            AutoCode code = new AutoCode("TL", id);
            txtMa.Text = code.ToString();
        }

        //Created by Vu Dinh khanh - 31/05/2018 : xác nhận và đóng form
        private void TheLoaiPhim_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
