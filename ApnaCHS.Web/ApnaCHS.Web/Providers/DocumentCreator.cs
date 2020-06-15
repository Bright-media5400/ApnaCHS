using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Web.Providers
{
    public class DocumentCreator
    {
        public ReportDocument GetBillReportDocument(string spName, string tablenames, string filePath, long socId, long flatid, int month, int year)
        {
            DataSet ds = new DataSet();
            BillDataSets(ds, spName, tablenames.Split(','), socId, flatid, month, year);

            ReportDocument report = new ReportDocument();
            report.Load(filePath);
            report.SetDataSource(ds);

            return report;
        }

        public void BillDataSets(DataSet ds, string sp, string[] tablenames, long socId, long flatid, int month, int year)
        {
            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter daEmp = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand(sp, conn);

            cmd.Parameters.Add(new SqlParameter("@FlatId", flatid));
            cmd.Parameters.Add(new SqlParameter("@Month", month));
            cmd.Parameters.Add(new SqlParameter("@Year", year));
            cmd.Parameters.Add(new SqlParameter("@SocietyId", socId));

            cmd.CommandType = CommandType.StoredProcedure;
            daEmp.SelectCommand = cmd;

            for (int i = 0; i < tablenames.Length; i++)
            {
                daEmp.TableMappings.Add("Table" + (i == 0 ? "" : i.ToString()), tablenames[i]);
            }
            //Fill the DataSet
            daEmp.Fill(ds);
        }


        public ReportDocument GetCustomReportDocument(string spName, string tablenames, string filePath)
        {
            DataSet ds = new DataSet();
            FillCustomReportDataSets(ds, spName, tablenames.Split(','));

            ReportDocument report = new ReportDocument();
            report.Load(filePath);
            report.SetDataSource(ds);

            return report;
        }

        public void FillCustomReportDataSets(DataSet ds, string sp, string[] tablenames)
        {
            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter daEmp = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();


            cmd = new SqlCommand(sp, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            daEmp.SelectCommand = cmd;

            for (int i = 0; i < tablenames.Length; i++)
            {
                daEmp.TableMappings.Add("Table" + (i == 0 ? "" : i.ToString()), tablenames[i]);

            }
            //Fill the DataSet
            daEmp.Fill(ds);
        }

    }
}