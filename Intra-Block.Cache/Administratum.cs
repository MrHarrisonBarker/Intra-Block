using System;

namespace Intra_Block.Cache
{
    public interface IAdministratum
    {
        Report GatherReport();
        void ReportInsertion(double timeToComplete);
        void ReportRetrieval(double timeToComplete);
        void ReportReap();
        void ReportRequest(double timeToComplete);
    }

    // analytics sink
    public class Administratum : IAdministratum
    {
        private readonly Averages Averages;

        private uint CurrentReapCount;
        private uint LastReapCount;
        private DateTime StartOfFrame;
        private DateTime EndOfFrame;

        public Administratum()
        {
            Averages = new Averages();
        }

        public Report GatherReport()
        {
            return new Report()
            {
                Averages = Averages,
            };
        }

        public void ReportInsertion(double timeToComplete)
        {
            Averages.TimeToInsert = (Averages.TimeToInsert + timeToComplete) / 2;
        }

        public void ReportRetrieval(double timeToComplete)
        {
            Averages.TimeToRetrieve = (Averages.TimeToRetrieve + timeToComplete) / 2;
        }

        // TODO: average checking should be done independently to the reporting otherwise if no more reports the average will be the last value forever
        public void ReportReap()
        {
            CurrentReapCount++;

            if (DateTime.Now > EndOfFrame)
            {
                StartOfFrame = DateTime.Now;
                EndOfFrame = StartOfFrame.AddMinutes(1);
                CurrentReapCount = 1;
            }

            Averages.ReapsPerMinute = (LastReapCount + CurrentReapCount) / 2;
        }

        public void ReportRequest(double timeToComplete)
        {
            throw new System.NotImplementedException();
        }
    }
}