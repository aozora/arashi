using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Core;

namespace TestBase.Log4Net
{
   public class TestAppender : AppenderSkeleton
   {
      #region Public Instance Constructors

      /// <summary>
      /// This is the list of logs
      /// </summary>
      [ThreadStatic]
      private static List<LoggingEvent> Logs;

      public static IList<LoggingEvent> GetLogs()
      {
         return Logs.AsReadOnly();
      }

      private void AddLog(LoggingEvent log)
      {
         if (Logs == null)
            Logs = new List<LoggingEvent>();
         Logs.Add(log);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="TestAppender" /> class.
      /// </summary>
      /// <remarks>
      /// The default constructor start the ServiceHost to listen for registration.
      /// </remarks>
      public TestAppender()
      {

      }

      #endregion Public Instance Constructors

      #region Override implementation of AppenderSkeleton

      /// <summary>
      /// This method is called by the <see cref="AppenderSkeleton.DoAppend(LoggingEvent)"/> method.
      /// It calls static method <see cref="AppenderService.Append"/> that is used to notify the log
      /// to all registered clients.
      /// </summary>
      /// <param name="loggingEvent">The event to log.</param>
      /// <remarks>
      /// <para>
      /// Send the event to all registered listener.
      /// </para>
      /// <para>
      /// Exceptions are passed to the <see cref="AppenderSkeleton.ErrorHandler"/>.
      /// </para>
      /// </remarks>
      protected override void Append(LoggingEvent loggingEvent)
      {
         AddLog(loggingEvent);
      }

      /// <summary>
      /// This appender requires a <see cref="log4net.Layout"/> to be set.
      /// </summary>
      /// <value><c>true</c></value>
      /// <remarks>
      /// <para>
      /// This appender requires a <see cref="log4net.Layout"/> to be set.
      /// </para>
      /// </remarks>
      override protected bool RequiresLayout
      {
         get
         {
            return true;
         }
      }

      /// <summary>
      /// Close the appender
      /// </summary>
      protected override void OnClose()
      {

      }

      #endregion Override implementation of AppenderSkeleton

   }
}
