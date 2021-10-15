using System;
using System.Collections.Generic;
using System.Text;

namespace CashierModel
{
    public struct Barcode
    {
        private const int Length = 13;
        public static bool IsCorrect(string barcode)
        {
            return barcode.Length == Length && ulong.TryParse(barcode, out _);
        }

        readonly ulong barcode;

        public Barcode(string barcode)
        {
            if (!IsCorrect(barcode))
                throw new ArgumentException("Формат строки штрих-кода некорректен");
            this.barcode = ulong.Parse(barcode);
        }

        public override string ToString()
        {
            return string.Format("{0:" + new string('0', Length) + "}", barcode);
        }

        public override int GetHashCode()
        {
            return (int)barcode;
        }

        public override bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        public static bool operator ==(Barcode barcode1, Barcode barcode2) =>
            barcode1.barcode == barcode2.barcode;
        public static bool operator !=(Barcode barcode1, Barcode barcode2) =>
            barcode1.barcode != barcode2.barcode;

        public static bool operator ==(Barcode barcode1, object barcode2) =>
            barcode1.ToString() == barcode2.ToString();
        public static bool operator !=(Barcode barcode1, object barcode2) =>
            barcode1.ToString() != barcode2.ToString();

        public static implicit operator string(Barcode barcode)
        {
            return barcode.ToString();
        }

        public static implicit operator Barcode(string barcode)
        {
            return new Barcode(barcode);
        }

        public static implicit operator ulong(Barcode barcode)
        {
            return barcode.barcode;
        }
    }
}
