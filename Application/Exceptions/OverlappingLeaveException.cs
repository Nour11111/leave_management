namespace LeaveManagement.Application.Exceptions
{
    public class OverlappingLeaveException : ApplicationException
    {
        public OverlappingLeaveException(string message) : base(message) { }
    }

}
