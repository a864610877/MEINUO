using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlUserAddressService : IUserAddressService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_UserAddress";

         public SqlUserAddressService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(UserAddress item)
        {
            return _databaseInstance.Insert(item,TableName);
        }

        public int Update(Models.UserAddress item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(Models.UserAddress item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public UserAddress GetById(int userAddressId)
        {
            return _databaseInstance.GetById<UserAddress>(TableName, userAddressId);
        }


        public DataTables<UserAddress> GetByUserId(UserAddressRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@UserId",request.UserId),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getFz_UserAddressByUserId", param);
            return _databaseInstance.GetTables<UserAddress>(sp);
        }


        public DataTables<UserAddress> GetByAccountId(int accountId)
        {
            var model = new DataTables<UserAddress>();
            string sql = "select * from fz_UserAddress where userId=@accountId";
            model.ModelList = new QueryObject<UserAddress>(_databaseInstance, sql, new { accountId = accountId }).ToList();
            return model;
        }
    }
}
