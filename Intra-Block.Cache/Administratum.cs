using System;
using Microsoft.Extensions.Logging;

namespace Intra_Block.Cache
{
    public interface IAdministratum
    {
        Report GatherReport();
        void ReportInsertion(long timeToCompleteInMicro);
        void ReportRetrieval(long timeToCompleteInMicro);
        void ReportReap();
        void ReportRequest(long timeToCompleteInMicro);
    }

    // analytics sink
    public class Administratum : IAdministratum
    {
        private readonly Averages Averages;
        private readonly ILogger<Administratum> Logger;

        private uint CurrentReapCount;
        private uint LastReapCount;
        private DateTime StartOfFrame;
        private DateTime EndOfFrame;

        private uint CurrentRequestCount;
        private uint LastRequestCount;

        public Administratum(ILogger<Administratum> logger)
        {
            Logger = logger;
            Averages = new Averages();
        }

        public Report GatherReport()
        {
            return new Report
            {
                Averages = Averages,
            };
        }

        public void ReportInsertion(long timeToCompleteInMicro)
        {
            double timeToCompleteInMilli = timeToCompleteInMicro * 0.001;

            Logger.LogInformation($"Reporting an insertion {timeToCompleteInMilli}ms");

            if (Averages.TimeToInsert == 0) Averages.TimeToInsert = timeToCompleteInMilli;
            
            Averages.TimeToInsert = (Averages.TimeToInsert + timeToCompleteInMilli) / 2;
        }

        public void ReportRetrieval(long timeToCompleteInMicro)
        {
            double timeToCompleteInMilli = timeToCompleteInMicro * 0.001;

            Logger.LogInformation($"Reporting a retrieval {timeToCompleteInMilli}ms");
            
            if (Averages.TimeToRetrieve == 0) Averages.TimeToRetrieve = timeToCompleteInMilli;

            Averages.TimeToRetrieve = (Averages.TimeToRetrieve + timeToCompleteInMilli) / 2;
        }

        // TODO: average checking should be done independently to the reporting otherwise if no more reports the average will be the last value forever
        public void ReportReap()
        {
            CurrentReapCount++;

            if (DateTime.Now > EndOfFrame)
            {
                StartOfFrame = DateTime.Now;
                EndOfFrame = StartOfFrame.AddMinutes(1);
                LastReapCount = CurrentReapCount;
                CurrentReapCount = 1;
            }

            Averages.ReapsPerMinute = (double)(LastReapCount + CurrentReapCount) / 2;
        }

        public void ReportRequest(long timeToCompleteInMicro)
        {
            double timeToCompleteInMilli = timeToCompleteInMicro * 0.001;
            
            if (Averages.RequestCompletion == 0) Averages.RequestCompletion = timeToCompleteInMilli;
            
            Averages.RequestCompletion = (Averages.RequestCompletion + timeToCompleteInMilli) / 2;
            
            CurrentRequestCount++;

            if (DateTime.Now > EndOfFrame)
            {
                StartOfFrame = DateTime.Now;
                EndOfFrame = StartOfFrame.AddMinutes(1);
                LastReapCount = CurrentRequestCount;
                CurrentRequestCount = 1;
            }

            Averages.RequestsPerMinute = (double)(LastRequestCount + CurrentRequestCount) / 2;
        }
    }
}