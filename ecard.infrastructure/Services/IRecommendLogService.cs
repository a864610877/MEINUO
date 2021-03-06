﻿using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public class MemberRecommendLogRequest : PageRequest
    {
        public int? salerId { get; set; }
        public string salerName { get; set; }
        public string userName { get; set; }
        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }
    }
    public interface IRecommendLogService
    {

        RecommendLog GetById(int id);
        int Insert(RecommendLog item);

        int Update(RecommendLog item);

        int Delete(RecommendLog item);

        DataTables<RecommendLogModel> Query(MemberRecommendLogRequest request);

        int RecommendCount(string UserName);
        DataTables<RecommendLog> MemberQuery(MemberRecommendLogRequest request);

        List<RecommendLogss> GetList(int userId);
       

    }
    public class RecommendLogss
    {
        public string DisplayName { get; set; }

        public int grade { get; set; }
        /// <summary>
        /// 0直推 1二级推荐
        /// </summary>
        public int tj { get; set; }
    }
}

