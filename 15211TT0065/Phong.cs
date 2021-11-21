using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using _15211TT0065.Model;

namespace _15211TT0065
{
    public partial class Phong : Form
    {
        //connection database and variable
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");
        static string id;
        bool flag;
        public Phong()
        {
            InitializeComponent();
        }

        //Created by Vu Dinh khanh - 30/05/2018 : xóa dữ liệu đang hiển thị trên các control
        private void clear()
        {
            txtMa.Clear();
            txtSoGhe.Clear();
            txtTen.Clear();
            txtGia.Clear();
            cboType.SelectedItem = null;
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Lấy danh sách phòng chiếu
        public static DataTable LayDSPhong(SqlConnection con)
        {
            id = null;
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayDSPhong", con);
                cmd.CommandText = "sp_LayDSPhong";
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

        //Created by Vu Dinh khanh - 30/05/2018 : Load dữ liệu lên form
        private void Form1_Load(object sender, EventArgs e)
        {
            dgvList.DataSource = LayDSPhong(con);
            AutoCode code = new AutoCode("PG", id);
            txtMa.Text = code.ToString();
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Kiểm tra dữ liệu đang hiển thị trên các control
        private void test()
        {
            flag = true;
            if (txtMa.Text.Length == 0 | txtSoGhe.Text.Length == 0 | txtTen.Text.Length == 0 | txtGia.Text.Length == 0 | cboType.Text.Length == 0)
            {
                MessageBox.Show("Nhập đầy đủ thông tin!");
                flag = false;
                return;
            }
            
            if (!txtSoGhe.Text.Trim().IsInterger())
            {
                MessageBox.Show("Nhập số ghế trong phòng chưa đúng!");
                flag = false;
                return;
            }

            double m = 0;
            bool flag2 = double.TryParse(txtGia.Text.Trim(), out m);
            if (!flag2)
            {
                MessageBox.Show("Nhập giá vé của phòng chưa đúng!");
                flag = false;
                return;
            }

            if (m < 10000)
            {
                MessageBox.Show("Nhập giá vé của phòng không hợp lệ!");
                flag = false;
                return;
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Thêm phòng chiếu mới
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int kq = 0;
            try
            {
                test();
                if (!flag)
                {
                    return;
                }

                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_themPhong", con);
                cmd.CommandText = "sp_themPhong";
                cmd.CommandType = CommandType.StoredProcedure;

                //Các parameters cho store procedure
                SqlParameter para_ma = new SqlParameter("@MaPhong", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@TenPhong", txtTen.Text.Trim());
                cmd.Parameters.Add(para_ten);
                SqlParameter para_loai = new SqlParameter("@LoaiPhong", cboType.Text);
                cmd.Parameters.Add(para_loai);
                SqlParameter para_gia = new SqlParameter("@Gia", double.Parse(txtGia.Text.Trim()));
                cmd.Parameters.Add(para_gia);
                SqlParameter para_sg = new SqlParameter("@SoGhe", int.Parse(txtSoGhe.Text.Trim()));
                cmd.Parameters.Add(para_sg);

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

            //kiểm tra kết quả và load lai dữ liệu
            if (kq != 0)
            {
                dgvList.DataSource = LayDSPhong(con);
                AutoCode code = new AutoCode("PG", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Sửa thông tin phòng chiếu
        private void btnEdit_Click(object sender, EventArgs e)
        {
            int kq = 0;
            try
            {
                test();
                if (!flag)
                {
                    return;
                }

                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_suaPhong", con);
                cmd.CommandText = "sp_suaPhong";
                cmd.CommandType = CommandType.StoredProcedure;

                //cac parameters cho store procedure
                SqlParameter para_ma = new SqlParameter("@MaPhong", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@TenPhong", txtTen.Text.Trim());
                cmd.Parameters.Add(para_ten);
                SqlParameter para_loai = new SqlParameter("@LoaiPhong", cboType.Text);
                cmd.Parameters.Add(para_loai);
                SqlParameter para_gia = new SqlParameter("@Gia", double.Parse(txtGia.Text.Trim()));
                cmd.Parameters.Add(para_gia);
                SqlParameter para_sg = new SqlParameter("@SoGhe", int.Parse(txtSoGhe.Text.Trim()));
                cmd.Parameters.Add(para_sg);

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
                dgvList.DataSource = LayDSPhong(con);
                AutoCode code = new AutoCode("PG", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : xóa phòng chiếu phim
        private void btnDel_Click(object sender, EventArgs e)
        {
            //xác nhận yêu cầu vầ xóa
            DialogResult result = MessageBox.Show("Bạn muốn xóa phòng!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int kq = 0;
                try
                {
                    // mo ket noi
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_xoaPhong", con);
                    cmd.CommandText = "sp_xoaPhong";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                    //    SqlCommand cmd = new SqlCommand("sp_xoaPhong2", con);
                    //    cmd.CommandText = "sp_xoaPhong2";
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
                    dgvList.DataSource = LayDSPhong(con);
                    AutoCode code = new AutoCode("PG", id);
                    txtMa.Text = code.ToString();
                }
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Load dữ liệu lên các control theo dòng dữ liệu đã chọn
        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cboType.SelectedItem = null;
            int dong = dgvList.CurrentCell.RowIndex;
            txtMa.Text = dgvList.Rows[dong].Cells[0].Value.ToString();
            txtTen.Text = dgvList.Rows[dong].Cells[1].Value.ToString();
            cboType.Text = dgvList.Rows[dong].Cells[2].Value.ToString();
            txtGia.Text = dgvList.Rows[dong].Cells[3].Value.ToString();
            txtSoGhe.Text = dgvList.Rows[dong].Cells[4].Value.ToString();
            if (dong > dgvList.RowCount - 2)
            {
                AutoCode code = new AutoCode("PG", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : đóng form
        private void Phong_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            AutoCode code = new AutoCode("PG", id);
            txtMa.Text = code.ToString();
        }
    }
}
