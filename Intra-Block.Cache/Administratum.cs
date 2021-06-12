namespace Intra_Block.Cache
{
    public interface IAdministratum
    {
        Report GatherReport();
        void ReportInsertion(double timeToComplete);
        void ReportRetrieval(double timeToComplete);
        void ReportReap();
    }

    // analytics sink
    public class Administratum : IAdministratum
    {
        public Report GatherReport()
        {
            throw new System.NotImplementedException();
        }

        public void ReportInsertion(double timeToComplete)
        {
            throw new System.NotImplementedException();
        }

        public void ReportRetrieval(double timeToComplete)
        {
            throw new System.NotImplementedException();
        }

        public void ReportReap()
        {
            throw new System.NotImplementedException();
        }
    }
}