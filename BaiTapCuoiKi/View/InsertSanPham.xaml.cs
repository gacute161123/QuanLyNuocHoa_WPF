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

namespace BaiTapCuoiKi.View
{
    /// <summary>
    /// Interaction logic for InsertSanPham.xaml
    /// </summary>
    public partial class InsertSanPham : Window
    {
        public InsertSanPham()
        {
            InitializeComponent();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btn_upload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_thoat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_inhoadon_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
