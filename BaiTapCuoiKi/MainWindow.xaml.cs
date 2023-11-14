using BaiTapCuoiKi.Model;
using BaiTapCuoiKi.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BaiTapCuoiKi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button activeButton;
        int idnguoidung = -1;
        int vaitronguoidung = -1;
        connect db = new connect();

        public MainWindow(int id)
        {
           
            InitializeComponent();
            activeButton = btn_home;
            kiemtravaitro(id);
        }
        public void kiemtravaitro(int id)
        {
            idnguoidung = id;
            var nguoidungdangnhap = db.NGUOIDUNG.Find(id);
            if(nguoidungdangnhap.VaiTro== "NHANVIEN")
            {
                vaitronguoidung = id;
                btn_khachhang.Visibility = Visibility.Collapsed;
                
            }
        }
        
      /*  private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.Focus();
                this.DragMove();
            }
        }*/
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void ActivateButton(Button button)
        {
            if (activeButton != null)
            {
                activeButton.Style = (Style)FindResource("menuButton"); 
            }

            activeButton = button;
            activeButton.Style = (Style)FindResource("menuButtonActive"); 
        }

        private void btn_sanpham_Click(object sender, RoutedEventArgs e)
        {
           
            if (vaitronguoidung==-1)
            {
                SanPhamUserControl sanPhamUser = new SanPhamUserControl();
                Maincontent.Content = sanPhamUser;
                ActivateButton(btn_sanpham);
            }
            else 
            {
                SanPhamUserControl sanPhamUser = new SanPhamUserControl();
                Maincontent.Content = sanPhamUser;
                sanPhamUser.btnThem.Visibility = Visibility.Collapsed;
                ActivateButton(btn_sanpham);
            }
        }

        private void btn_donhang_Click(object sender, RoutedEventArgs e)
        {
            Maincontent.Content = new DonHangUserControl();
            ActivateButton(btn_donhang);

        }

        private void btn_khachhang_Click(object sender, RoutedEventArgs e)
        {
            Maincontent.Content = new KhachHangUserControl();
            ActivateButton(btn_khachhang);
        }

      /*  private void btn_caidat_Click(object sender, RoutedEventArgs e)
        {
            Maincontent.Content = new TaiKhoanUserControl();
            ActivateButton(btn_caidat);
        }*/

        private void btn_home_Click(object sender, RoutedEventArgs e)
        {
            Maincontent.Content = new HomeUserControl();
            ActivateButton(btn_home);
        }

        private void btn_dangxuat_Click(object sender, RoutedEventArgs e)
        {
            idnguoidung = -1;
            vaitronguoidung = -1;
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
    }
}
