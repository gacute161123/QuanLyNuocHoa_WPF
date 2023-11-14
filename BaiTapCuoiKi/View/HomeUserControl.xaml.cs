using BaiTapCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public partial class HomeUserControl : UserControl
    {
        private connect db= new connect();
        public HomeUserControl()
        {
            InitializeComponent();
            tongdoanhthu();
            soluongkhachhang();
            soluongdonhang();
        }
        public void getdata()
        {
            
        }
        public void tongdoanhthu()
        {
            decimal tongdoanhthu = db.HOADON.Sum(c => (decimal)c.Donhang_tonggiatri);
            string formattedText = tongdoanhthu.ToString("#,0.##");
            tbl_tongdoanhthu.Text = formattedText;
        }
        public void soluongkhachhang()
        {
            tbl_soluongkhachhang.Text = db.KHACHHANG.Count().ToString();
        }
        public void soluongdonhang()
        {
            tbl_sodonhang.Text = db.HOADON.Count().ToString();
        }
    }
}
