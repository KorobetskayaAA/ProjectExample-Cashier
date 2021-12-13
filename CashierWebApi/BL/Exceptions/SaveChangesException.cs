using System;

namespace CashierWebApi.BL.Exceptions
{
    public class SaveChangesException : Exception
    {
        public SaveChangesException() : base("Произошла неизвестная ошибка при сохранении данных") { }
        public SaveChangesException(Exception innerException)
            : base("Произошла ошибка при сохранении данных", innerException) { }
    }
}
