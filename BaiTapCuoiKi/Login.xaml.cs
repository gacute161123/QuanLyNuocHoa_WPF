using BaiTapCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        connect db = new connect();
        public Login()
        {
            InitializeComponent();
            tbl_dangky.MouseLeftButtonDown += dangky_Click;
        }
        private void dangky_Click(object sender, MouseButtonEventArgs e)
        {
            Register register = new Register();
            register.Show();
        }
        private void textAccount_MouseDown(object sender, MouseEventArgs e)
        {
            txtAccount.Focus();
        }
        private void txtAccount_textChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtAccount.Text) && txtAccount.Text.Length>0)  {
                textAccount.Visibility= Visibility.Collapsed;
            }
            else { textAccount.Visibility= Visibility.Visible;}
        }
        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else { textPassword.Visibility = Visibility.Visible; }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AllowLogin())// nếu hàm này trả ra false thì thoát ngay 
            {
                return;
            }

            string tdn = txtAccount.Text;
            string mk = txtPassword.Password;
            NGUOIDUNG nguoidung = db.NGUOIDUNG.FirstOrDefault(x => x.TenNguoiDung == tdn && x.MatKhau == mk);

            if (nguoidung != null)
            {
                int id = nguoidung.IDNguoiDung;
                MainWindow mainwindow = new MainWindow(id);
                mainwindow.Show();
                Close();
               /* if (nguoidung.VaiTro == "ADMIN")
                {
                    MessageBox.Show("Đăng nhập thành công ở vị trí admin!");
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
                else if (nguoidung.VaiTro == "NGUOIDUNG")
                {
                    MessageBox.Show("Đăng nhập thành công ở vị trí nguoidung!");
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.btn_khachhang.Visibility = Visibility.Collapsed;
                    

                    mainWindow.Show();
                    Close();
                }
*/

            }
            else
            {
                MessageBox.Show("Đăng nhập không thành công. Vui lòng kiểm tra tên đăng nhập và mật khẩu.");
            }


        }
        private bool AllowLogin()
        {
            if (txtAccount.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập dữ liệu Tài Khoản", "Cảnh Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAccount.Focus();
                return false;
            }
            if (txtPassword.Password.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập dữ liệu Mật Khẩu", "Cảnh Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return false;
            }
            return true;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát", "Cảnh Báo", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

    }
}
