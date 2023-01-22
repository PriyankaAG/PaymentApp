using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.Services
{
    public interface IAppPaymentService
    {
        string GooglePay(string pa, string pn, string amount);
        string PhonePay(string pa, string pn, string amount);
        string IciciPay(string amount);
        string BhimPay(string amount);
        string PayTm(string amount);
    }
}
