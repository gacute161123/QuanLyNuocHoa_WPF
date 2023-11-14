using BaiTapCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BaiTapCuoiKi
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        connect db = new connect();
        public Register()
        {
            InitializeComponent();

            DateTime currentTime = DateTime.Now;

            // Đặt thời gian vào DatePicker
            tb_thoigiandat.SelectedDate = currentTime;
        }

        private void btn_them_Click(object sender, RoutedEventArgs e)
        {
            string tenguoidung = tb_tennd.Text;
            string email = tb_email.Text;
            string matkhau = tb_matkhau.Password;
            string thoigiandat = tb_thoigiandat.Text;
            string vaitro = cbb_vaitro.SelectedValue.ToString();
            var nguoidung = new NGUOIDUNG();
            nguoidung.TenNguoiDung = tenguoidung;
            nguoidung.Email=email;
            nguoidung.MatKhau = matkhau;
            nguoidung.NgayDangKy = thoigiandat;
            nguoidung.VaiTro=vaitro;
            db.NGUOIDUNG.Add(nguoidung);
            db.SaveChanges();
            MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            this.Close();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void ConfirmPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string password = tb_matkhau.Password;
            string confirmPassword = tb_nhaplaimatkhau.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("Bạn nhập lại mật khẩu không trùng khớp với mật khẩu ");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
