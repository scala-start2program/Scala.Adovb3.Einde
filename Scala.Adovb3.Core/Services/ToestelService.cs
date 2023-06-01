using Scala.Adovb3.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.Adovb3.Core.Services
{
    public class ToestelService
    {
        public List<Toestel> GetToestellen(string soortfilter = null)
        {
            List<Toestel> toestellen = new List<Toestel>();
            string sql;
            sql = "select * from toestellen ";
            if (soortfilter != null)
            {
                sql += $"where soort = '{soortfilter}' ";
            }
            sql += " order by merk, serie";
            DataTable dt = DBServices.ExecuteSelect(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string id = dr["id"].ToString();
                string merk = dr["merk"].ToString();
                string serie = dr["serie"].ToString();
                decimal prijs = decimal.Parse(dr["prijs"].ToString());
                int stock = int.Parse(dr["stock"].ToString());
                string soort = dr["soort"].ToString();
                toestellen.Add(new Toestel(id, merk, serie, prijs, stock, soort));
            }
            return toestellen;
        }
        public bool ToestelToevoegen(Toestel toestel)
        {
            string sql;
            sql = "insert into toestellen (id, merk, serie, prijs, stock, soort) values (";
            sql += $"'{toestel.Id}' , ";
            sql += $"'{Helper.HandleQuotes(toestel.Merk)}' , ";
            sql += $"'{Helper.HandleQuotes(toestel.Serie)}' , ";
            sql += $"'{toestel.Prijs.ToString()}' , ";
            sql += $"'{toestel.Stock.ToString()}' , ";
            sql += $"'{Helper.HandleQuotes(toestel.Soort)}') ";
            return DBServices.ExecuteCommand(sql);
        }
        public bool ToestelWijzigen(Toestel toestel)
        {
            string strPrijs = toestel.Prijs.ToString();
            strPrijs = strPrijs.Replace(",", ".");
            string sql;
            sql = "update toestellen set ";
            sql += $" merk = '{Helper.HandleQuotes(toestel.Merk)}' , ";
            sql += $" serie = '{Helper.HandleQuotes(toestel.Serie)}' , ";
            sql += $" prijs = {strPrijs} , ";
            sql += $" stock = {toestel.Stock.ToString()} , ";
            sql += $" soort = '{Helper.HandleQuotes(toestel.Soort)}' ";
            sql += $" where id = '{toestel.Id}' ";
            return DBServices.ExecuteCommand(sql);
        }
        public bool ToestelVerwijderen(Toestel toestel)
        {
            string sql;
            sql = "delete from toestellen ";
            sql += $" where id = '{toestel.Id}' ";
            return DBServices.ExecuteCommand(sql);
        }

        public decimal GetStockwaarde()
        {
            string sql;
            sql = "select prijs * stock as waarde from toestellen";
            DataTable dt = DBServices.ExecuteSelect(sql);
            decimal stockwaarde = 0M;
            foreach(DataRow dr in dt.Rows)
            {
                stockwaarde += decimal.Parse(dr["waarde"].ToString());
            }
            return stockwaarde;
        }
    }
}
