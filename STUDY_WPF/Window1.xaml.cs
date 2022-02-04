using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace STUDY_WPF_01.Binding
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window1 : Window
    {
        Cust c1;
        public Window1()
        {
            InitializeComponent();
            c1 = GetData();
            //grdMain.DataContext = c1;
            //Validation.AddErrorHandler(this.txtAge, txtAge_ValidationErrorHandler);
        }

        private void txtAge_ValidationErrorHandler(object sender, ValidationErrorEventArgs e)
        {
            //MessageBox.Show(e.Error.ErrorContent.ToString(), "유효성 검사 에러");
            txtAge.ToolTip = e.Error.ErrorContent.ToString();
        }

        private Cust GetData()
        {
            //return new Cust("이순신", 22);
            Cust c=(Cust)this.FindResource("c2");
            return c;
        }

        private void btnYear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddAge_Click(object sender, RoutedEventArgs e)
        {
            c1.Age++;

        }
        //16 <-> 10진수 변환
        

        //~19 : Green
        //20 ~40 : Blue
        //41~ : Red
        
    }
    //Validation Rule
    //나이 : 0 ~125
    public class AgeRangeRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int n;
            if(!int.TryParse((string)value,out n))
            {
                return new ValidationResult(false, "잘못된 숫자입니다.");
            }
            if(n>=Min && n <= Max)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, string.Format("나이의 범위는 {0} ~ {1} 사이의 숫자 이어야 합니다.", Min, Max));
            }
        }
    }
    //16 <-> 10진수 변환
    [ValueConversion(typeof(int), typeof(string))]
    public class Int32ToHex : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value).ToString("X"); //10 -> A . 10 -> a
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return int.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
                return null;
            }
        }
    }
    [ValueConversion(typeof(int), typeof(Brush))]
    public class Int32ToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                return null;
            }
            int age = Int32.Parse(value.ToString());
            if (age < 20)
            {
                return Brushes.Green;
            }
            else if (age <= 40)
            {
                return Brushes.Blue;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
