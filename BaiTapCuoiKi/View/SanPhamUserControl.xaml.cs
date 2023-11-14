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
    /// Interaction logic for SanPhamUserControl.xaml
    /// </summary>
    public partial class SanPhamUserControl : UserControl
    {
        connect db = new connect();
        int idsanphamSelected = 0;
        public SanPhamUserControl()
        {
            InitializeComponent();
            getData();
        }
       
        public void getData()
        {
            connect db = new connect();
            dgSanPham.ItemsSource = db.SANPHAM.ToList();
        }   

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text.Length > 0)
            {
                tbl_Search.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbl_Search.Visibility = Visibility.Visible;
            }
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
                    var ketQua = db.SANPHAM.Where(sanpham => sanpham.Sanpham_ten.ToString().Contains(noiDungTimKiem)).ToList();
                    dgSanPham.ItemsSource = ketQua;
                }
                else if (luaChon == 1)
                {
                    var ketQua = db.SANPHAM.Where(sanpham => sanpham.Sanpham_gia.ToString().Contains(noiDungTimKiem)).ToList();
                    dgSanPham.ItemsSource = ketQua;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void dgSanPham_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ThongTinSanPham insertSanPham = new ThongTinSanPham(idsanphamSelected);
            insertSanPham.DataChangedform1 += InsertWindow_DataChanged;
            insertSanPham.ShowDialog();
        }

        // xác định phần tử người dùng nhập vào
        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Xác định Border mà người dùng đã nhấp vào
            Border clickedBorder = sender as Border;

            if (clickedBorder != null)
            {
                // Lấy ra đối tượng dữ liệu 
                SANPHAM item = clickedBorder.DataContext as SANPHAM;

                if (item != null)
                {
                    int productID = item.Sanpham_ID; // ID của sản phẩm
                    idsanphamSelected = productID;
                }
            }
        }


        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            ThemSanPham insertSanPham = new ThemSanPham();
            insertSanPham.DataChanged += InsertWindow_DataChanged;
            insertSanPham.ShowDialog();
        }
        public void InsertWindow_DataChanged(object sender, EventArgs e)
        {
            getData();
        }

        private void tbl_Search_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }
    }
}
