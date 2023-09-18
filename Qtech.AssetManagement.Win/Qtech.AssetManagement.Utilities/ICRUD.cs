using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.Utilities
{
    public interface ICRUD
    {
        //Create new record
        void NewRecord();        

        int SaveRecords();
        //Save record

        void DeleteRecords();
        //Delete record

        void PrintRecords();
        //Print record
        
    }
}
