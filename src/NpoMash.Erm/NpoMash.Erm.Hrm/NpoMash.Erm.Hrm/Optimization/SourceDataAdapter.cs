using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public abstract class SourceDataAdapter {
        public abstract void GetData();
        public abstract void CreateCriteria();
        public abstract void CreateRestrictions();
        public abstract void RunOptimisation();
        public abstract void AfterOptimisation();
        public abstract void ReturnData();
        public void Execute() {
            GetData();
            CreateCriteria();
            CreateRestrictions();
            RunOptimisation();
            AfterOptimisation();
            ReturnData();
        }
    }
}
