namespace BackTest.Loggers
{
    /// <summary>
    /// These types use when writing log in file
    /// </summary>
    public enum TypeLog
    {
        /// <summary>
        /// Use for error loging
        /// </summary>
        Error,
        /// <summary>
        /// Use for info loging
        /// </summary>
        Info,
        /// <summary>
        /// Use for warning loging
        /// </summary>
        Warning
    }

    /// <summary>
    /// This class contains methods for work with logging
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Write log in file.
        /// </summary>
        /// <param name="messageLog"></param>
        /// <param name="typeLog"></param>
        public async static Task Log(string messageLog, TypeLog typeLog)
        {
            using (StreamWriter writer = new StreamWriter("Log.txt", true))
            {
                await writer.WriteLineAsync($"{typeLog}");
                await writer.WriteLineAsync("-----");
                await writer.WriteLineAsync($"{DateTime.Now}");
                await writer.WriteLineAsync($"{messageLog}");
                await writer.WriteLineAsync("-----------------------------------");
            }
        }
    }
}
