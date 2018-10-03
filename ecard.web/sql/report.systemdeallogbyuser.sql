 

select u.Name userName, u.displayName,
		isnull(t.[售卡], 0.0)          as '售卡',
		isnull(t.[售卡手续费], 0.0)	as '售卡手续费',
		isnull(t.[充值], 0.0)			as '充值',
		isnull(t.[储值卡押金], 0.0)	as '储值卡押金',
		isnull(t.[清算支付], 0.0)		as '清算支付' ,
		isnull(t.[换卡], 0.0)		   as '换卡' ,
		isnull(t.[退卡], 0.0)          as '退卡' ,
		isnull(t.[退储值卡押金], 0.0)  as '退储值卡押金' 
from users u 
	left join (
		 select userid, 
			[1] as '售卡',
			[2] as '售卡手续费',
			[3] as '充值',
			[4] as '储值卡押金' ,
			[6] as '清算支付' ,
			[7] as '换卡' ,
			[11] as '退卡' ,
			[14] as '退储值卡押金' 
			from (select userid, sumAmount, dealtype from (
						select	0 as SystemDealLogByUserId, 
							userId, 
							@start as SubmitDate, 
							sum(amount) as SumAmount, 
							avg(amount) as AvgAmount, 
							count(*) as [Count], 
							DealType from systemdeallogs 
							where state = 1 
							and submittime >= @start 
							and submittime < @end 
							group by userid, dealtype) v
					where  submitdate >= @start  and  submitdate  < @end)  as t 
		 pivot (sum(sumAmount) for [dealtype] in ([1],[2],[3],[4],[6],[7],[11],[14])) as ourPivot
		) as t 
		on u.userid = t.userid

where discriminator = 'adminuser'