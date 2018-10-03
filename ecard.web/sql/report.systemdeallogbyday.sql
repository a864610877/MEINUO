select tn.[Text] as dealtype, 
		isnull(sum(t.SumAmount), 0) + isnull((select sum(s.amount) from systemdeallogs s where convert(nvarchar(50), dateadd(day, 1, getdate()), 101) = convert(nvarchar(50), @end, 101) and s.submittime > dateadd(day, -1, @end) and  s.dealtype = tn.typeid ), 0) SumAmount, 
		isnull(Sum(t.[Count]), 0)  + isnull((select count(s.amount) from systemdeallogs s where convert(nvarchar(50), dateadd(day, 1, getdate()), 101) = convert(nvarchar(50), @end, 101) and s.submittime > dateadd(day, -1, @end) and  s.dealtype = tn.typeid ), 0) [Count] , tn.ordernum
		
		from [TypeNames] tn left Join [ReportSystemDealLogDay] t on tn.typeId = t.dealtype and (@start is null or submitdate >= @start) and  (@end is null or submitdate < @end) 
		
		where tn.category = 'systemdeallogtype'  group by tn.text, t.DealType, tn.typeId, tn.ordernum
union all 
		select '合计',
		(select isnull(sum(SumAmount), 0) + isnull((select sum(s.amount) from systemdeallogs s where convert(nvarchar(50), dateadd(day, 1, getdate()), 101) = convert(nvarchar(50), @end, 101) and s.submittime > dateadd(day, -1, @end)  ), 0) 
				
		from [ReportSystemDealLogDay] where (@start is null or submitdate >= @start) and  (@end is null or submitdate < @end) ),
		(select sum([Count]) + isnull((select count(s.amount) from systemdeallogs s where convert(nvarchar(50), dateadd(day, 1, getdate()), 101) = convert(nvarchar(50), @end, 101) and s.submittime > dateadd(day, -1, @end)  ), 0) 
				
		from [ReportSystemDealLogDay] where (@start is null or submitdate >= @start) and  (@end is null or submitdate < @end) ),
		 999
union all 
		select '系统余额',
		(select top 1 amount from SiteAccounts),
		null, 1000