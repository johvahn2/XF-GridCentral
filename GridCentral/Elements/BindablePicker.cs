using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.Elements
{
    public class BindablePicker : Picker
    {
        public BindablePicker()
        {
            base.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(BindablePicker), null, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate(OnSelectedItemChanged), null, null, null);
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(BindablePicker), null, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate(OnItemsSourceChanged), null, null, null);
        public static readonly BindableProperty DisplayPropertyProperty = BindableProperty.Create("DisplayProperty", typeof(string), typeof(BindablePicker), null, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate(OnDisplayPropertyChanged), null, null, null);

        public IList ItemsSource
        {
            get { return (IList)base.GetValue(ItemsSourceProperty); }
            set { base.SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return base.GetValue(SelectedItemProperty); }
            set
            {
                base.SetValue(SelectedItemProperty, value);
                if (ItemsSource.Contains(SelectedItem))
                {
                    SelectedIndex = ItemsSource.IndexOf(SelectedItem);
                }
                else
                {
                    SelectedIndex = -1;
                }
            }
        }

        public string DisplayProperty
        {
            get { return (string)base.GetValue(DisplayPropertyProperty); }
            set { base.SetValue(DisplayPropertyProperty, value); }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedItem = ItemsSource[SelectedIndex];
        }


        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BindablePicker picker = (BindablePicker)bindable;
            picker.SelectedItem = newValue;
            if (picker.ItemsSource != null && picker.SelectedItem != null)
            {
                int count = 0;
                foreach (object obj in picker.ItemsSource)
                {
                    if (obj == picker.SelectedItem)
                    {
                        picker.SelectedIndex = count;
                        break;
                    }
                    count++;
                }
            }
        }

        private static void OnDisplayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BindablePicker picker = (BindablePicker)bindable;
            picker.DisplayProperty = (string)newValue;
            loadItemsAndSetSelected(bindable);

        }
        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BindablePicker picker = (BindablePicker)bindable;
            picker.ItemsSource = (IList)newValue;
            loadItemsAndSetSelected(bindable);
        }

        static void loadItemsAndSetSelected(BindableObject bindable)
        {
            BindablePicker picker = (BindablePicker)bindable;
            if (picker.ItemsSource as IEnumerable != null)
            {
                int count = 0;
                foreach (object obj in (IEnumerable)picker.ItemsSource)
                {
                    string value = string.Empty;
                    if (picker.DisplayProperty != null)
                    {
                        var prop = obj.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, picker.DisplayProperty, StringComparison.OrdinalIgnoreCase));
                        if (prop != null)
                        {
                            value = prop.GetValue(obj).ToString();
                        }
                    }
                    else
                    {
                        value = obj.ToString();
                    }
                    picker.Items.Add(value);
                    if (picker.SelectedItem != null)
                    {
                        if (picker.SelectedItem == obj)
                        {
                            picker.SelectedIndex = count;
                        }
                    }
                    count++;
                }
            }
        }
    }

    public static class EnumerableExtensions
    {

        public static int IndexOf(this IEnumerable self, object obj)
        {
            int index = -1;

            var enumerator = self.GetEnumerator();
            enumerator.Reset();
            int i = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Equals(obj))
                {
                    index = i;
                    break;
                }

                i++;
            }

            return index;
        }
    }
}
