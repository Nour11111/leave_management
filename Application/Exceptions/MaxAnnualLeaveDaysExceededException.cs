namespace LeaveManagement.Application.Exceptions
{
    public class MaxAnnualLeaveDaysExceededException : ApplicationException
    {
        public MaxAnnualLeaveDaysExceededException(string message) : base(message) { }
    }

}
