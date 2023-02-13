using Web_Triolingo.Model;

namespace Web_Triolingo.DBContext
{
    public class DataProvider
    {
        private static DataProvider _ins;
        public static DataProvider Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new DataProvider();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public TriolingoDBContext DB { get; set; }
        public DataProvider()
        {
            DB = new TriolingoDBContext();
        }
    }
}
