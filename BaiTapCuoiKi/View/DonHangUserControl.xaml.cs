using BaiTapCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
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
    /// Interaction logic for DonHangUserControl.xaml
    /// </summary>
    public partial class DonHangUserControl : UserControl
    {
        connect db = new connect();
        public DonHangUserControl()
        {
            InitializeComponent();
            getData();
        }
        public void getData()
        {
            connect db = new connect();
            dgDonHang.ItemsSource = db.HOADON.ToList();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            InsertDonHang insertWindow = new InsertDonHang();
            insertWindow.DataChanged += InsertWindow_DataChanged;
            insertWindow.ShowDialog();
        }
        public void InsertWindow_DataChanged(object sender, EventArgs e)
        {
            getData();
        }

        private void tbl_Search_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // nội dung thay đổi thì ẩn tbl đi 
            if (!string.IsNullOrEmpty(tbl_Search.Text) && tbl_Search.Text.Length > 0)
            {
                tbl_Search.Visibility = Visibility.Collapsed;
             
            }
            else { tbl_Search.Visibility = Visibility.Visible; }
            TimKiem();
           
        }

        private void TimKiem()
        {
            try
            {
                int luaChon = cbbSearchType.SelectedIndex;
                string noiDungTimKiem = txtSearch.Text.ToLower();

                if (luaChon == 0)
                {
                    var ketQua = db.HOADON.Where(donhang => donhang.Donhang_ID.ToString().Contains(noiDungTimKiem)).ToList();
                    dgDonHang.ItemsSource = ketQua;
                }
                else if (luaChon == 1)
                {
                    var ketQua = db.HOADON.Where(donhang =>donhang.KHACHHANG.Khachhang_ten.ToLower().Contains(noiDungTimKiem)).ToList();
                    dgDonHang.ItemsSource = ketQua;
                }
                else if (luaChon == 2)
                {
                    var ketQua = db.HOADON.Where(donhang => donhang.Donhang_ngaydathang.ToLower().Contains(noiDungTimKiem)).ToList();
                    dgDonHang.ItemsSource = ketQua;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int countSelecteditem = 0;
                countSelecteditem = dgDonHang.SelectedItems.Count;
                if (countSelecteditem == 0)
                {
                    MessageBox.Show("Bạn chưa chọn item cần sửa");
                }
                else
                {
                    HOADON hoadonSelected = (HOADON)dgDonHang.SelectedItem;
                    int idHoaDonSelected = hoadonSelected.Donhang_ID;
                    InsertDonHang insertDonHang = new InsertDonHang(idHoaDonSelected);
                    insertDonHang.DataChanged += InsertWindow_DataChanged;
                    insertDonHang.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex, "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }


        private void XoaHD_Click(object sender, RoutedEventArgs e)
        {
            try
            {    
                var result = MessageBox.Show("Bạn có chắc chắn xóa dữ liệu?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    HOADON hoadonSelected = (HOADON)dgDonHang.SelectedItem;
                    int idhoadonSelected = hoadonSelected.Donhang_ID;
                    HOADON hd = db.HOADON.Find(idhoadonSelected);

                    var chiTietHoadons = db.CHITIETHOADON.Where(ct => ct.Donhang_ID == idhoadonSelected).ToList();
                    db.CHITIETHOADON.RemoveRange(chiTietHoadons);
                    db.HOADON.Remove(hd);
                    db.SaveChanges();
                    MessageBox.Show("Xóa thành công");
                    getData();
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

       
        private void dgDonHang_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                HOADON hoadonSelected = (HOADON)dgDonHang.SelectedItem;
                int idHoaDonSelected = hoadonSelected.Donhang_ID;

                // Mở cửa sổ chi tiết thông tin đơn hàng
                ChiTietDonHangWindow chiTietDonHangWindow = new ChiTietDonHangWindow(idHoaDonSelected);
                chiTietDonHangWindow.ShowDialog();

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
