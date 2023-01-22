using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.ViewModels
{
    public class ScannerViewResult
    {
        public ScannerViewResult(string text) : this(text, null)
        {
        }

        private ScannerViewResult(string text, object state)
        {
            this.Text = text;
        }

        public string Text { get; }
    }
}
