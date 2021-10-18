﻿using CashierModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCashier
{
    static class MocksFabric
    {
        public static IEnumerable<Product> MockProducts => new List<Product>()
        {
            new Product("4634567890098", "Батон горчичный", 37.12m ),
            new Product("8001234567891", "Спагетти Италия 450г", 89.9m),
            new Product("4609876541212", "Вода негаз. 0,5л", 15.6m),
            new Product("5345738573637", "Томаты вес.", 90.5m),
            new Product("1234567891234", "Огурцы вес.", 75.25m),
         };
    }
}
