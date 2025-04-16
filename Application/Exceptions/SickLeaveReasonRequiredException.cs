namespace LeaveManagement.Application.Exceptions
{
    public class SickLeaveReasonRequiredException : ApplicationException
    {
        public SickLeaveReasonRequiredException(string message) : base(message) { }
    }
}
