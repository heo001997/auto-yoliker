using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoYolikes.YolikerApp
{
    public class YolikerLogs
    {
        public DataGridView dtgrvdata;
        public int row;
        public string StatusCells = "clstatus";
        public void Write(string message, TypeLog type)
        {
            if(type == TypeLog.NORMAL)
            {
                dtgrvdata.Rows[row].DefaultCellStyle.BackColor = YolikerStatics.NormalColor;
            }    
            else
            {
                dtgrvdata.Rows[row].DefaultCellStyle.BackColor = YolikerStatics.WarningColor;
            }
            dtgrvdata.Rows[row].Cells[StatusCells].Value = message;
        }
        public enum TypeLog
        {
            NORMAL,
            WARNING
        }
    }
}
