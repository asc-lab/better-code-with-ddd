using System;
using System.Collections.Generic;

namespace DebtorRegistryMock
{
    public class DebtorInfo
    {
        public string Pesel { get; set; }
        public List<Debt> Debts { get; set; }
    }

    public class Debt
    {
        public decimal Amount { get; set; }
    }
}