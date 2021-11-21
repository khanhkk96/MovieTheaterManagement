using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using _15211TT0065.Model;
using System.Collections.ObjectModel;

namespace _15211TT0065
{
    public partial class LichChieuPhim : Form
    {
        //connection string and variable
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");
        static string id;
        bool flag;
        string code;
        bool? isEdit = null;
        TimeSpan timeCurrent;
        Collection<TimeFilm> collection;

        public LichChieuPhim()
        {
            InitializeComponent();
            collection = new Collection<TimeFilm>();
        }

        /// <summary>
        /// Lấy danh sách lịch chiếu phim
        /// </summary>
        /// <param name="con">connection to database</param>
        /// <returns>lịch chiếu phim</returns>
        /// Created by Vu Dinh Khanh - 01/06/2018 : Lấy dữ liệu lịch chiếu phim
        public static DataTable LayDSLich(SqlConnection con)
        {
            id = null;
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayLich", con);
                cmd.CommandText = "sp_LayLich";
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

        /// <summary>
        /// Lấy danh sách lịch chiếu phim theo mã phòng và ngày
        /// </summary>
        /// <param name="con">connection to database</param>
        /// <returns>none</returns>
        /// Created by Vu Dinh Khanh - 01/06/2018 : Lấy dữ liệu lịch chiếu phim
        private void LayDSLich2()
        {
            id = null;
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayLich2", con);
                cmd.CommandText = "sp_LayLich2";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = new SqlParameter("@MaPhong", cboPhong.SelectedValue);
                cmd.Parameters.Add(p);
                SqlParameter n = new SqlParameter("@Ngay", DateTime.Parse(mtbTime.Text));
                cmd.Parameters.Add(n);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                
                //nhận lại thời gian chiếu và thời lượng phim của danh sách lịch
                if (dt.Rows.Count > 0)
                {
                    collection = new Collection<TimeFilm>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i].ItemArray[0].ToString() == txtMa.Text)
                        {
                            //bỏ qua lịch có mã được hiển thị trên textbox
                            continue;
                        }
                        collection.Add(new TimeFilm(dt.Rows[i].ItemArray[0].ToString(), dt.Rows[i].ItemArray[1].ToString(), dt.Rows[i].ItemArray[2].ToString()));
                    }
                }
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

        //Created by Vu Dinh Khanh - 01/06/2018 : hiển thị dữ liệu theo dòng được chọn
        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            mtbTime.Clear();
            cboPhong.SelectedItem = null;
            cboPhim.SelectedItem = null;
            cboStaff.SelectedItem = null;
            try
            {
                int dong = dgvList.CurrentCell.RowIndex;
                txtMa.Text = dgvList.Rows[dong].Cells[0].Value.ToString();
                code = txtMa.Text;
                if (dong > dgvList.RowCount - 2)
                {
                    AutoCode code = new AutoCode("LC", id);
                    txtMa.Text = code.ToString();
                }

                string time = dgvList.Rows[dong].Cells[2].Value.ToString();
                DateTime date = new DateTime();
                date = Convert.ToDateTime(time);
                string str = date.ToString("dd/MM/yyyy HH:mm");
                mtbTime.Text = str;

                //kiểm tra có thể sửa đổi dòng dữ liệu
                if (date.CompareTo(DateTime.Now) <= 0)
                {
                    isEdit = false;
                }
                else
                {
                    isEdit = true;
                }

                //tìm selected value của cboPhim trong datasource
                string phim = dgvList.Rows[dong].Cells[1].Value.ToString();
                foreach (DataRow item in (cboPhim.DataSource as DataTable).Rows)
                {
                    if (item.ItemArray[1].ToString() == phim)
                    {
                        cboPhim.SelectedValue = item.ItemArray[0].ToString();
                        break;
                    }
                }

                //tìm selected value của cboStaff trong datasource
                string nv = dgvList.Rows[dong].Cells[3].Value.ToString();
                foreach (DataRow item in (cboStaff.DataSource as DataTable).Rows)
                {
                    if (item.ItemArray[1].ToString() == nv)
                    {
                        cboStaff.SelectedValue = item.ItemArray[0].ToString();
                        break;
                    }
                }

                //tìm selected value của cboPhong trong datasource
                string phong = dgvList.Rows[dong].Cells[4].Value.ToString();
                foreach (DataRow item in (cboPhong.DataSource as DataTable).Rows)
                {
                    if (item.ItemArray[1].ToString() == phong)
                    {
                        cboPhong.SelectedValue = item.ItemArray[0].ToString();
                        break;
                    }
                }

            }
            catch (Exception ex) { }
        }

        //Created by Vu Dinh Khanh - 01/06/2018 : kiểm tra trước khi thao tác csdl
        private void test()
        {
            //không bỏ trống trường nhập
            flag = true;
            if (txtMa.Text.Length == 0 || cboPhim.SelectedValue == null || cboPhong.SelectedValue == null || cboStaff.SelectedValue == null || mtbTime.TextLength == 0)
            {
                MessageBox.Show("Nhập đầy đủ thông tin!");
                flag = false;
                return;
            }

            //xác thực thời gian chiếu
            DateTime date;
            try
            {
                date = DateTime.Parse(mtbTime.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thời gian chiếu không hợp lệ!");
                flag = false;
                return;
            }

            if (date.CompareTo(DateTime.Now) <= 0)
            {
                MessageBox.Show("Thời gian chiếu chưa hợp lý!");
                flag = false;
                return;
            }

            //kiểm tra va chọn ra thời gian chiếu thích hợp cho phim
            LayDSLich2();

            if (collection.Count > 0)
            {
                bool isTrue = false;
                foreach (TimeFilm item in collection)
                {
                    DateTime start = DateTime.Parse(item.Start);
                    TimeSpan time = TimeSpan.Parse(item.Time);
                    if (date.CompareTo(start) == 0)
                    {
                        break;
                    }
                    else if (date.CompareTo(start) > 0)
                    {
                        if (date.Subtract(start) >= time)
                        {
                            int index = collection.IndexOf(item);

                            if ((index += 1) < collection.Count)
                            {
                                TimeFilm tf = collection[index];
                                DateTime start2 = DateTime.Parse(tf.Start);
                                TimeSpan time2 = TimeSpan.Parse(tf.Time);
                                if (start2.Subtract(date) >= timeCurrent)
                                {
                                    isTrue = true;
                                    break;
                                }
                            }
                            else
                            {
                                isTrue = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (start.Subtract(date) >= timeCurrent)
                        {
                            int index = collection.IndexOf(item);
                            TimeFilm tf = collection[index - 1];
                            if (tf != null)
                            {
                                DateTime start2 = DateTime.Parse(tf.Start);
                                TimeSpan time2 = TimeSpan.Parse(tf.Time);
                                if (date.Subtract(start2) >= time2)
                                {
                                    isTrue = true;
                                    break;
                                }
                            }
                            else
                            {
                                isTrue = true;
                                break;
                            }
                        }
                    }
                }

                if (!isTrue)
                {
                    MessageBox.Show("Thời gian chiếu chưa hợp lý!");
                    flag = false;
                }
            }
        }


        //Created by Vu Dinh Khanh - 01/06/2018 : Tạo thêm lịch chiếu
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int kq = 0;
            try
            {
                //nhận kết quả kiểm tra
                test();
                if (!flag)
                {
                    return;
                }

                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_themLich", con);
                cmd.CommandText = "sp_themLich";
                cmd.CommandType = CommandType.StoredProcedure;

                //Nhận các parameters cho procedure
                SqlParameter para_ma = new SqlParameter("@MaLich", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@MaPhim", cboPhim.SelectedValue);
                cmd.Parameters.Add(para_ten);
                SqlParameter para_tg = new SqlParameter("@BatDau", DateTime.Parse(mtbTime.Text));
                cmd.Parameters.Add(para_tg);
                SqlParameter para_loai = new SqlParameter("@MaNV", cboStaff.SelectedValue);
                cmd.Parameters.Add(para_loai);
                SqlParameter para_nd = new SqlParameter("@MaPhong", cboPhong.SelectedValue);
                cmd.Parameters.Add(para_nd);

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

            //kiểm tra va load dữ liệu lên view
            if (kq != 0)
            {
                dgvList.DataSource = LayDSLich(con);
                AutoCode code = new AutoCode("LC", id);
                txtMa.Text = code.ToString();
            }
        }

        //Create by Vu Dinh Khanh - 01/06/2018 : Sửa lịch chiếu phim
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //kiểm tra xem có thể chỉnh sửa hay k?
            if (isEdit == null)
            {
                MessageBox.Show("Chọn lịch chiếu phim cần chỉnh sửa!");
                return;
            }
            else if (isEdit == false)
            {
                MessageBox.Show("Lịch chiếu phim đã hết hạn chỉnh sửa!");
                return;
            }

            
            int kq = 0;
            try
            {
                //kiểm tra đủ điều kiện chỉnh sửa
                test();

                if (!flag)
                {
                    return;
                }

                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SuaLich", con);
                cmd.CommandText = "sp_SuaLich";
                cmd.CommandType = CommandType.StoredProcedure;

                //Nhận các parameters của procedure
                SqlParameter para_ma = new SqlParameter("@MaLich", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@MaPhim", cboPhim.SelectedValue);
                cmd.Parameters.Add(para_ten);
                SqlParameter para_tg = new SqlParameter("@BatDau", DateTime.Parse(mtbTime.Text));
                cmd.Parameters.Add(para_tg);
                SqlParameter para_loai = new SqlParameter("@MaNV", cboStaff.SelectedValue);
                cmd.Parameters.Add(para_loai);
                SqlParameter para_nd = new SqlParameter("@MaPhong", cboPhong.SelectedValue);
                cmd.Parameters.Add(para_nd);

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

            //kiểm tra kết quả và load dữ liệu
            if (kq != 0)
            {
                dgvList.DataSource = LayDSLich(con);
                AutoCode code = new AutoCode("LC", id);
                txtMa.Text = code.ToString();
            }

            this.code = null;
            isEdit = null;
        }

        //Created by Vu Dinh Khanh - 01/06/2018 : Xóa lịch chiếu phim
        private void btnDel_Click(object sender, EventArgs e)
        {
            //kiểm tra xem có đủ điện để xóa
            if (isEdit == null)
            {
                MessageBox.Show("Chọn lịch chiếu phim muốn xóa!");
                return;
            }

            //xác nhận yêu cầu người dùng
            DialogResult result = MessageBox.Show("Bạn muốn xóa lịch chiếu phim!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int kq = 0;
                try
                {
                    // mo ket noi
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_xoaLich", con);
                    cmd.CommandText = "sp_xoaLich";
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
                    //    SqlCommand cmd = new SqlCommand("sp_xoaLich2", con);
                    //    cmd.CommandText = "sp_xoaLich2";
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

                //kiểm tra kết quả là load dữ liệu
                if (kq != 0)
                {
                    dgvList.DataSource = LayDSLich(con);
                    AutoCode code = new AutoCode("LC", id);
                    txtMa.Text = code.ToString();
                }
            }

            this.code = null;
            isEdit = null;
        }


        //Created by Vu Dinh khanh - 01/06/2018 : load dữ liệu cho control trong form
        private void LichChieuPhim_Load(object sender, EventArgs e)
        {
            dgvList.DataSource = LayDSLich(con);
            cboStaff.DataSource = NhanVien.LayDSNV2(con);
            cboPhong.DataSource = Phong.LayDSPhong(con);
            cboPhim.DataSource = Phim.LayDSPhim(con);
            AutoCode code = new AutoCode("LC", id);
            txtMa.Text = code.ToString();
            cboPhong.SelectedItem = null;
            cboPhim.SelectedItem = null;
            cboStaff.SelectedItem = null;
        }

        //Created by Vu Dinh Khanh - 01/06/2018 : clear dữ liệu hiển thị trên các control
        private void clear()
        {
            txtMa.Clear();
            cboPhong.SelectedItem = null;
            cboPhim.SelectedItem = null;
            cboStaff.SelectedItem = null;
            mtbTime.Clear();
        }

        //Created by Vu Dinh Khanh - 01/06/2018 : xác nhận yêu cầu đóng form
        private void LichChieuPhim_FormClosing(object sender, FormClosingEventArgs e)
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

        private void cboPhim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPhim.SelectedValue != null)
            {
                foreach (DataRow item in (cboPhim.DataSource as DataTable).Rows)
                {
                    if (item.ItemArray[0].ToString() == cboPhim.SelectedValue.ToString())
                    {
                        timeCurrent = TimeSpan.Parse(item.ItemArray[2].ToString());
                        break;
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            AutoCode code = new AutoCode("LC", id);
            txtMa.Text = code.ToString();
        }
    }


    public class TimeFilm
    {
        private string code, start, time;

        public TimeFilm(string code, string start, string time)
        {
            this.code = code;
            this.start = start;
            this.time = time;
        }

        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
            }
        }

        public string Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }

        public string Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }
    }
}
