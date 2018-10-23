using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Bookings.Queries
{
    public class OccupancyQuery
    {
        private readonly string _constr;
        public OccupancyQuery(string constr) => _constr = constr;


        public async Task<(double sunny, double notSunny)> GetRoomOcuppancy(DateTime date, int idRoom) =>
            (await GetRoomOcuppancy(date, idRoom, isSunny: true), 0);

        private async Task<double> GetRoomOcuppancy(DateTime date, int idRoom, bool isSunny)
        {
            var day = date.Date;

            using (var con = new SqlConnection(_constr))
            {
                await con.OpenAsync();
                var sql = "PredictOccupation";
                using (var cmd = new SqlCommand(sql,con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = day;
                    cmd.Parameters.Add("@idRoom", System.Data.SqlDbType.Int).Value = idRoom;
                    cmd.Parameters.Add("@isSunnyDay", System.Data.SqlDbType.Bit).Value = isSunny;

                    var percent = Convert.ToDouble(await cmd.ExecuteScalarAsync());

                    return percent;
                }
            }

        }
    }
}
