using Sprout.Exam.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Common.Process
{
    public abstract class EmployeeSalaryFactory
    {
        public abstract EmployeeType GetEmployeeType  { get; }
        public abstract decimal AbsentDays  { get; set; }
        public abstract decimal WorkingDays { get; set; }
        public abstract decimal ComputeSalary();

    }

     public class RegularEmployee : EmployeeSalaryFactory
    {
       private readonly EmployeeType _employeeType;
       private readonly decimal _salary;
       private readonly decimal _taxDeduction;
       private decimal _absentDays;
       private decimal _workingDays;
       

        public RegularEmployee(EmployeeType employeeType, decimal absentDays, decimal workingDays,decimal salary, decimal taxDeduction)
        {
            _employeeType = EmployeeType.Regular;
            _absentDays = absentDays;
            _workingDays = workingDays;
            _salary = 25000;
            _taxDeduction = (decimal)0.12;
        }

        public override EmployeeType GetEmployeeType => _employeeType;

        public override decimal AbsentDays { get => _absentDays; set => _absentDays=value; }
        public override decimal WorkingDays { get => _workingDays; set => _workingDays=value; }

        public override decimal ComputeSalary()
        {
            return _salary - (_salary / (WorkingDays - AbsentDays)) - (_salary * _taxDeduction);
        }
    }

    public class ContractualEmployee : EmployeeSalaryFactory 
    {
        private readonly EmployeeType _employeeType;
        private readonly decimal _salary;
        private decimal _absentDays;
        private decimal _workingDays;

        public ContractualEmployee(EmployeeType employeeType, decimal absentDays, decimal workingDays, decimal salary)
        {
            _employeeType = EmployeeType.Regular;
            _absentDays = absentDays;
            _workingDays = workingDays;
            _salary = 20000;
        }

        public override EmployeeType GetEmployeeType => _employeeType;

        public override decimal AbsentDays { get => _absentDays; set => _absentDays = value; }
        public override decimal WorkingDays { get => _workingDays; set => _workingDays = value; }

        public override decimal ComputeSalary()
        {
            return _salary * WorkingDays;
        }
    }
}
