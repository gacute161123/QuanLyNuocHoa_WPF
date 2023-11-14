using BaiTapCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BaiTapCuoiKi.View
{
    /// <summary>
    /// Interaction logic for InsertKhachHang.xaml
    /// </summary>
    public partial class InsertKhachHang : Window
    {
        connect db = new connect();
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;
        private int id = -1;
        public InsertKhachHang()
        {
            InitializeComponent();
        }
        public InsertKhachHang(int idkhachhang) : this()
        {
            tb_dangkykhachhang.Text = "Thông Tin Khách Hàng";
            id = idkhachhang;
            HienThiDuLieuSua();
        }
        public void HienThiDuLieuSua(){
            var khachhangSelected = db.KHACHHANG.Find(id);
            if (khachhangSelected != null)
            {
                txtten.Text = khachhangSelected.Khachhang_ten.ToString();
                txtdiachi.Text = khachhangSelected.Khachhang_diachi.ToString();
                txtsdt.Text = khachhangSelected.Khachhang_sdt.ToString();
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void txtchangedten(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtten.Text) && txtten.Text.Length > 0)
            {
                tbl_ten.Visibility = Visibility.Collapsed;
            }
            else { tbl_ten.Visibility = Visibility.Visible; }
        }
        private void txtchangeddiachi(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtdiachi.Text) && txtdiachi.Text.Length > 0)
            {
                tbl_diachi.Visibility = Visibility.Collapsed;
            }
            else { tbl_diachi.Visibility = Visibility.Visible; }
        }
        private void txtchangedsdt(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtsdt.Text) && txtsdt.Text.Length > 0)
            {
                tbl_sdt.Visibility = Visibility.Collapsed;
            }
            else { tbl_sdt.Visibility = Visibility.Visible; }
        }


        private void tbl_ten_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtten.Focus();
        }

        private void tbl_diachi_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtdiachi.Focus();
        }

        private void tbl_sdt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtsdt.Focus();
        }

        private void them_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string tenkhachhang = txtten.Text;
                string diachi = txtdiachi.Text;
                string sdt = txtsdt.Text;
                if (id == -1)
                {
                    var khachhang = new KHACHHANG();
                    khachhang.Khachhang_ten = tenkhachhang;
                    khachhang.Khachhang_diachi = diachi;
                    khachhang.Khachhang_sdt = sdt;
                    db.KHACHHANG.Add(khachhang);
                    db.SaveChanges();
                    MessageBox.Show("Thêm Khách Hàng Thành Công", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    this.Close();

                }
                else
                {
                    var khedit = db.KHACHHANG.Find(id);
                    khedit.Khachhang_ten = tenkhachhang;
                    khedit.Khachhang_diachi = diachi;
                    khedit.Khachhang_sdt = sdt;
                    db.SaveChanges();
                    MessageBox.Show("Sửa dữ liệu thành công!", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    this.Close();
                }
                DataChangedEventHandler handler = DataChanged;
                if (handler != null)
                {
                    handler(this, new EventArgs());
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra vui lòng kiểm tra lại!", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
