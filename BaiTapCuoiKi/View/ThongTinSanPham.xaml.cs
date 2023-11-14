using BaiTapCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace BaiTapCuoiKi.View
{
    /// <summary>
    /// Interaction logic for ThongTinSanPham.xaml
    /// </summary>
    public partial class ThongTinSanPham : Window
    {
        connect db = new connect();
   
        private int id = -1;
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChangedform1;
        public ThongTinSanPham()
        {
            InitializeComponent();
        }
        public ThongTinSanPham(int idSanPhamSelected) : this()
        {
            id = idSanPhamSelected;
            hienThiDuLieu();
        }
      
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        public void hienThiDuLieu()
        {
            connect db = new connect();
            string pathNoImage = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Image/anhchuanhap.jpg";
            var sanphamSelected = db.SANPHAM.Find(id);
            if (sanphamSelected != null)
            {
                string pathImageSach = (sanphamSelected.Sanpham_anh == "" || sanphamSelected.Sanpham_anh == null) ? pathNoImage : sanphamSelected.Sanpham_anh;
                imgsanpham.Source = new BitmapImage(new Uri(pathImageSach));
                pathImage.Text = sanphamSelected.Sanpham_anh;
                tb_ten.Text = sanphamSelected.Sanpham_ten;
                tb_moba.Text = sanphamSelected.Sanpham_mota;
                tb_gia.Text = sanphamSelected.Sanpham_gia.ToString();
                tb_soluong.Text = sanphamSelected.Sanpham_soluong.ToString();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_clicksua(object sender, RoutedEventArgs e)
        {
           
            ThemSanPham themSanPham = new ThemSanPham(id);
            themSanPham.DataChanged += InsertWindow_DataChanged;
            themSanPham.ShowDialog();
            DataChangedEventHandler handler = DataChangedform1;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
            
        }
        public void InsertWindow_DataChanged(object sender, EventArgs e)
        {
            hienThiDuLieu();
        }
    }
}
