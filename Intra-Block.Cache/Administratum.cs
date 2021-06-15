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

        public Administratum(ILogger<Administratum> logger)
        {
            Logger = logger;
            Averages = new Averages();
        }

        public Report GatherReport()
        {
            return new Report()
            {
                Averages = Averages,
            };
        }

        public void ReportInsertion(long timeToCompleteInMicro)
        {
            double timeToCompleteInMilli = timeToCompleteInMicro * 0.001;
            
            Logger.LogInformation($"Reporting an insertion {timeToCompleteInMilli}ms");
            
            Averages.TimeToInsert = (Averages.TimeToInsert + timeToCompleteInMilli) / 2;
        }

        public void ReportRetrieval(long timeToCompleteInMicro)
        {
            double timeToCompleteInMilli = timeToCompleteInMicro * 0.001;
            
            Logger.LogInformation($"Reporting a retrieval {timeToCompleteInMilli}ms");
            
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

            Averages.ReapsPerMinute = (LastReapCount + CurrentReapCount) / 2;
        }

        public void ReportRequest(long timeToCompleteInMicro)
        {
            throw new System.NotImplementedException();
        }
    }
}