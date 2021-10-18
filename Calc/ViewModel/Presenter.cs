using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Calc.Models;

namespace Calc.ViewModel
{
    internal class Presenter : ObservableObject
    {
        private Calculator calc = new Calculator();

        private String expression;

        private String result;


        public String Input
        {
            get
            {
                return this.expression;
            }
            set
            {
                this.expression = value;
                this.RaisePropertyChangedEvent("Expression");
            }
        }

        public String Result
        {
            get
            {
                return this.result;
            }
            set
            {
                this.result = value;
                this.RaisePropertyChangedEvent("Result");
            }
        }

        public ICommand CalculateCommand
        {
            get { return new DelegateCommand(this.Calculate); }
        }


        private void Calculate()
        {
            var temp = 0.0d;

            try
            {
                temp = this.calc.Calculate(this.Input);
            }
            catch (Exception e)
            {
                return;
            }

            this.Result = temp.ToString();
        }
    }
}
