using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using _15211TT0065.Model;

namespace _15211TT0065
{
    public partial class Phim : Form
    {
        //connection database and variable
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");
        static string id;
        bool flag;
        public Phim()
        {
            InitializeComponent();
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Lấy danh sách phim từ database
        public static DataTable LayDSPhim(SqlConnection con)
        {
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayDSPhim", con);
                cmd.CommandText = "sp_LayDSPhim";
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

        //Created by Vu Dinh khanh - 30/05/2018 : load dữ liệu lên form
        private void Phim_Load(object sender, EventArgs e)
        {
            dgvList.DataSource = LayDSPhim(con);
            cboTheLoai.DataSource =  TheLoaiPhim.LayDSTL(con);
            cboTheLoai.DisplayMember = "Tên thể loại";
            cboTheLoai.ValueMember = "Mã thể loại";
            cboTheLoai.SelectedItem = null;

            //auto code
            AutoCode code = new AutoCode("PM", id);
            txtMa.Text = code.ToString();

            DataGridViewImageColumn imageCol = (DataGridViewImageColumn)dgvList.Columns[5];
            imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom; // will do the trick
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Kiểm tra dữ liệu hiển thị trên các trường nhập
        private void test()
        {
            flag = true;
            if (pbImage.Image == null || txtMa.Text.Length == 0 || txtTen.Text.Length == 0 || cboTheLoai.SelectedValue == null || rtbNoiDung.Text.Length == 0 || txtThoiLuong.Text.Length == 0)
            {
                MessageBox.Show("Nhập đầy đủ thông tin!");
                flag = false;
                return;
            }

            try
            {
                TimeSpan time = TimeSpan.Parse(txtThoiLuong.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kiểm tra thời lượng phim!");
                flag = false;
                return;
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Thêm một bộ phim mới
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
                SqlCommand cmd = new SqlCommand("sp_themPhim", con);
                cmd.CommandText = "sp_themPhim";
                cmd.CommandType = CommandType.StoredProcedure;

                //các parameters cho store procedure
                SqlParameter para_ma = new SqlParameter("@MaPhim", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@TenPhim", txtTen.Text.Trim());
                cmd.Parameters.Add(para_ten);
                SqlParameter para_tg = new SqlParameter("@ThoiLuong", TimeSpan.Parse(txtThoiLuong.Text));
                cmd.Parameters.Add(para_tg);
                SqlParameter para_loai = new SqlParameter("@TheLoai", cboTheLoai.SelectedValue);
                cmd.Parameters.Add(para_loai);
                SqlParameter para_nd = new SqlParameter("@NoiDung", rtbNoiDung.Text.Trim());
                cmd.Parameters.Add(para_nd);
                SqlParameter para_ha = new SqlParameter("@HinhAnh", pbImage.Image.ImageToByteArray());
                cmd.Parameters.Add(para_ha);

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
                dgvList.DataSource = LayDSPhim(con);
                AutoCode code = new AutoCode("PM", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Sửa thông tin của phim
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
                SqlCommand cmd = new SqlCommand("sp_SuaPhim", con);
                cmd.CommandText = "sp_SuaPhim";
                cmd.CommandType = CommandType.StoredProcedure;

                //các parameters cho store procedure
                SqlParameter para_ma = new SqlParameter("@MaPhim", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@TenPhim", txtTen.Text.Trim());
                cmd.Parameters.Add(para_ten);
                SqlParameter para_tg = new SqlParameter("@ThoiLuong", TimeSpan.Parse(txtThoiLuong.Text));
                cmd.Parameters.Add(para_tg);
                SqlParameter para_loai = new SqlParameter("@TheLoai", cboTheLoai.SelectedValue);
                cmd.Parameters.Add(para_loai);
                SqlParameter para_nd = new SqlParameter("@NoiDung", rtbNoiDung.Text.Trim());
                cmd.Parameters.Add(para_nd);
                SqlParameter para_ha = new SqlParameter("@HinhAnh", pbImage.Image.ImageToByteArray());
                cmd.Parameters.Add(para_ha);

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

            //kiểm tra kết quả và load lại dữ  liệu
            if (kq != 0)
            {
                dgvList.DataSource = LayDSPhim(con);
                AutoCode code = new AutoCode("PM", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : xóa một bộ phim
        private void btnDel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn xóa phim!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int kq = 0;
                try
                {
                    // mo ket noi
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_xoaPhim", con);
                    cmd.CommandText = "sp_xoaPhim";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //tham số truyền cho store
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
                    //    SqlCommand cmd = new SqlCommand("sp_xoaPhim2", con);
                    //    cmd.CommandText = "sp_xoaPhim2";
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
                    dgvList.DataSource = LayDSPhim(con);
                    AutoCode code = new AutoCode("PM", id);
                    txtMa.Text = code.ToString();
                }
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Load dữ liệu lên các control theo dòng dữ liệu đã chọn
        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cboTheLoai.SelectedItem = null;
            int dong = dgvList.CurrentCell.RowIndex;
            txtMa.Text = dgvList.Rows[dong].Cells[0].Value.ToString();
            txtTen.Text = dgvList.Rows[dong].Cells[1].Value.ToString();
            txtThoiLuong.Text = dgvList.Rows[dong].Cells[2].Value.ToString();

            string s = dgvList.Rows[dong].Cells[3].Value.ToString();
            foreach (DataRow item in (cboTheLoai.DataSource as DataTable).Rows)
            {
                if (item.ItemArray[1].ToString() == s)
                {
                    cboTheLoai.SelectedValue = item.ItemArray[0].ToString();
                }
            }
            rtbNoiDung.Text = dgvList.Rows[dong].Cells[4].Value.ToString();


            //hình ảnh
            if (dgvList.Rows[dong].Cells[5].Value.ToString().Length > 0)
            {
                byte[] img = (byte[])dgvList.Rows[dong].Cells[5].Value;
                    pbImage.Image = Test.ByteArrayToImage(img);
            }
            else
            {
                pbImage.Image = null;
            }

            //auto code
            if (dong > dgvList.RowCount - 2)
            {
                AutoCode code = new AutoCode("PM", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : xóa dữ liệu hiển thị trên các control
        private void clear()
        {
            txtMa.Clear();
            txtTen.Clear();
            txtThoiLuong.Clear();
            cboTheLoai.SelectedItem = null;
            rtbNoiDung.Clear();
            pbImage.Image = null;
        }

        //Created by Vu Dinh khanh - 30/05/2018 : xác nhận và đóng form
        private void Phim_FormClosing(object sender, FormClosingEventArgs e)
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

        //Created by Vu Dinh khanh - 30/05/2018 : chọn ảnh cho bộ phim
        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "Image File|*.png; *.bmp; *.gif; *.jpg; *.jpeg; ";
            op.ShowDialog();
            if (op.FileName.Length > 0)
            {
                pbImage.Image = Image.FromFile(op.FileName);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            AutoCode code = new AutoCode("PM", id);
            txtMa.Text = code.ToString();
        }
        
    }
}
