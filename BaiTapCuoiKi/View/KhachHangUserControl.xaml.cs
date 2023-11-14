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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BaiTapCuoiKi.View
{
    /// <summary>
    /// Interaction logic for KhachHangUserControl.xaml
    /// </summary>
    public partial class KhachHangUserControl : UserControl
    {
        connect db = new connect();
        public KhachHangUserControl()
        {
            InitializeComponent();
            getdata();
        }
        public void getdata()
        {
            connect db = new connect();
            dgKhachHang.ItemsSource=db.KHACHHANG.ToList();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            InsertKhachHang insertKhachHang = new InsertKhachHang();
            insertKhachHang.DataChanged += InsertWindow_DataChanged;
            insertKhachHang.ShowDialog();
        }
        public void InsertWindow_DataChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void tbl_Search_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }


        private void btn_xoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Bạn có chắc chắn xóa dữ liệu?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    KHACHHANG khahhangselected = (KHACHHANG)dgKhachHang.SelectedItem;
                    int idkhachhang = khahhangselected.Khachhang_ID;

                    KHACHHANG khachhang = db.KHACHHANG.Find(idkhachhang);

                    var hoadons = db.HOADON.Where(ct => ct.Khachhang_ID == idkhachhang).ToList();
                    foreach( var hoadon in hoadons)
                    {
                        var cthd = db.CHITIETHOADON.Where(ct=>ct.Donhang_ID==hoadon.Donhang_ID).ToList();
                        db.CHITIETHOADON.RemoveRange(cthd);
                        db.HOADON.Remove(hoadon);
                    }
                   
                    db.HOADON.RemoveRange(hoadons);
                    db.KHACHHANG.Remove(khachhang);
                    db.SaveChanges();
                    MessageBox.Show("Xóa thành công");
                    getdata();


                   
                }
                else if (result == MessageBoxResult.No)
                {
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex, "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void btn_sua_Click(object sender, RoutedEventArgs e)
        {
            KHACHHANG khahhangselected = (KHACHHANG)dgKhachHang.SelectedItem;
            int idkhachhang = khahhangselected.Khachhang_ID;
            InsertKhachHang insertKhachHang = new InsertKhachHang(idkhachhang);
            insertKhachHang.DataChanged += InsertWindow_DataChanged;
            insertKhachHang.ShowDialog();

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text.Length > 0)
            {
                tbl_Search.Visibility = Visibility.Collapsed;
            }
            else { tbl_Search.Visibility = Visibility.Visible; }
            TimKiem();
        }
        private void TimKiem()
        {  
            string noiDungTimKiem = txtSearch.Text.ToLower();
            var ketQua = db.KHACHHANG.Where(kh => kh.Khachhang_ten.ToString().Contains(noiDungTimKiem)).ToList();
            dgKhachHang.ItemsSource = ketQua;
        }
    }
}
