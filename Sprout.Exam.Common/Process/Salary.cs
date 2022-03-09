using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Common.Process
{
    public abstract class Salary
    {
        public abstract decimal BaseSalary { get; }
        public abstract decimal ComputedSalary { get; }

    }

   public class RegularEmployee : Salary
    {
        private decimal _salary;
        private decimal _absentDays;
        private readonly decimal _workedDays;
        private decimal _taxDeduction;

        public RegularEmployee(decimal salary, decimal absentDays, decimal workedDays, decimal taxDeduction)
        {
            _salary = salary;
            _absentDays = absentDays;
            _workedDays = 22;
            _taxDeduction = taxDeduction;

        }

        public override decimal BaseSalary => _salary;

       public override decimal ComputedSalary =>decimal.Round((BaseSalary - ((BaseSalary/_workedDays)*_absentDays)) - (BaseSalary*_taxDeduction),2,MidpointRounding.AwayFromZero);
        //public override decimal ComputedSalary =>decimal.Round(BaseSalary/_workedDays*_absentDays,2,MidpointRounding.AwayFromZero);

    }

    public class ContractualEmploye : Salary 
    {
        private decimal _salary;
        private readonly decimal _workedDays;
    
        public ContractualEmploye(decimal salary, decimal workedDays)
        {
            _salary = salary;
            _workedDays = workedDays;
        }

        public override decimal BaseSalary => _salary;

        public override decimal ComputedSalary => decimal.Round((BaseSalary * _workedDays), 2, MidpointRounding.AwayFromZero);
    }
}
