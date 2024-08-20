namespace Acudir.Test.Apis.Extra.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException() { }

        public DatabaseException(string message) : base(message)
        {

        }
    }
}
