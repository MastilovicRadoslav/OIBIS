namespace Common
{
    public class Help
    {
        private static MarkChange markChange = new MarkChange();

        public static MarkChange HelpForChange
        {
            get { return markChange; }
        }
    }
}