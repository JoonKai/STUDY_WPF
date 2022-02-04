using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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

namespace STUDY_WPF_01.Binding
{
    /// <summary>
    /// Window3.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
        }

        private void btnYear_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnPre_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = GetList();
            view.MoveCurrentToPrevious();
            if (view.IsCurrentBeforeFirst)
            {
                view.MoveCurrentToFirst();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = GetList();
            view.MoveCurrentToNext();
            if (view.IsCurrentAfterLast)
            {
                view.MoveCurrentToLast();
            }
        }

        private ICollectionView GetList()
        {
            CustList list = (CustList)this.FindResource("custList");

            return CollectionViewSource.GetDefaultView(list);
        }

        private void lstCust_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("선택 고객 코드: "+lstCust.SelectedValue.ToString());
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //틀린 예
            //MessageBox.Show(((Cust)lstCust.SelectedItem).Name + "을 삭제 합니다.", "알림");

            Cust c = ((Button)sender).DataContext as Cust;
            MessageBox.Show(c.Name + "을 삭제 합니다.", "알림");

        }
        private void btnAddCust_Click_OLD(object sender, RoutedEventArgs e)
        {
            CustList list = (CustList)this.FindResource("custList");

            list.Add(new Cust("세종대왕", 30));
        }
        private void btnAddCust_Click(object sender, RoutedEventArgs e)
        {
            DataSourceProvider provider = (DataSourceProvider)this.FindResource("custList");

            CustList list = provider.Data as CustList;

            list.Add(new Cust("세종대왕", 30));
        }
        private void btnSortName_Click(object sender, RoutedEventArgs e)
        {
            ListCollectionView list = (ListCollectionView)GetList();



            //if(list.SortDescriptions.Count == 0)
            //{
            //    list.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            //    list.SortDescriptions.Add(new SortDescription("AGE", ListSortDirection.Descending));
            //}
            //else
            //{
            //    list.SortDescriptions.Clear();
            //}

            if(list.CustomSort == null)
            {
                list.CustomSort = new CustSorter();
            }
            else
            {
                list.CustomSort = null;
            }
        }
        private void btnSortAge_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView list = GetList();
            if (list.SortDescriptions.Count == 0)
            {
                list.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Ascending));
                list.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
            }
            else
            {
                list.SortDescriptions.Clear();
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView list = GetList();
            if(list.Filter == null)
            {
                list.Filter = delegate (object item)
                {
                    return ((Cust)item).Age >= 20;
                };
            }
            else
            {
                list.Filter = null;
            }
        }

        private void btnGroupName_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView list = GetList();
            if(list.GroupDescriptions.Count == 0)
            {
                list.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
            }
            else
            {
                list.GroupDescriptions.Clear();
            }
        }

        private void btnAgeRangeGroup_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView list = GetList();
            if(list.GroupDescriptions.Count == 0)
            {
                list.GroupDescriptions.Add(
                    new PropertyGroupDescription("Age", new AgeToRangeConverter())
                    );
                list.GroupDescriptions.Add(
                    new PropertyGroupDescription("Name")
                    );
            }
            else
            {
                list.GroupDescriptions.Clear();
            }
        }
    }
    class CustSorter : IComparer
    {
        public int Compare(object obj1, object obj2)
        {
            //- : x
            //0 : 동점
            //+ : 바꾼다

            Cust c1 = (Cust)obj1;
            Cust c2 = (Cust)obj2;

            int r = c1.Name.CompareTo(c2.Name);
            Debug.WriteLine("C1:"+ c1.Name);
            Debug.WriteLine("C2:"+ c2.Name);
            Debug.WriteLine("r:"+ r);
            return r;
        }
    }
    class AgeToRangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value >= 20 ? "성년" : "미성년";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    class RemoteCustListLoader
    {
        public CustList LoadCustList(string min, string max)
        {
            CustList list = new CustList();
            Debug.WriteLine("min" + min);
            Debug.WriteLine("max" + max);
            list.Add(new Cust { Name = "홍길동1", Age = 17 });
            list.Add(new Cust { Name = "홍길동1", Age = 17 });
            list.Add(new Cust { Name = "홍길동1", Age = 27 });
            list.Add(new Cust { Name = "홍길동2", Age = 37 });
            list.Add(new Cust { Name = "홍길동3", Age = 47 });
            list.Add(new Cust { Name = "홍길동3", Age = 57 });
            list.Add(new Cust { Name = "홍길동4", Age = 67 });
            return list;
        }

    }
}
