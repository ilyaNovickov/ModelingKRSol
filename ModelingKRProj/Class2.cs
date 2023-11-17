using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingKRProj
{
    internal class Class2 : DoubleConverter//TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return base.CanConvertFrom(context, sourceType);
            //bool asd = sourceType == typeof(string);
            //return sourceType == typeof(double);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str)
            {
                if (double.TryParse(str, out double doubleValue))
                {
                    if (doubleValue <= 0)
                        throw new Exception("Значение не может быть равно 0 или быть отрицательным");
                    return base.ConvertFrom(context, culture, value);
                }
            }
            throw new Exception("В свойство можно установить только число");
        }
    }
}
