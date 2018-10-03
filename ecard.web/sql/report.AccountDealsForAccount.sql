select accountTypeName, accountName, AccountShopName, Convert(nvarchar(20), ExpiredDate, 23) ExpiredDate, userDisplayName, mobile, 
		isnull(startAccountAmount, 0.0) startAccountAmount, 
		openAmount, 
		rechargingAmount, 
		dealAmount, 
		closeAmount, 
		endAccountAmount, 
		dealCount, 
		case dealCount when 0 then 0.0 else (dealAmount / dealCount) end dealAvgAmount 
		from (

			select accType.DisplayName accountTypeName, acc.Name accountName , acc.ExpiredDate, u.displayName userDisplayName, u.mobile,
				isnull((select s.displayname from shops s where s.shopid = acc.shopid), '系统总部') AccountShopName,
				IsNull((select top 1 lg.accountAmount  startAccountAmount
					from deallogs lg 
					where (@start is null or lg.submittime < @start) 
								and acc.AccountId = lg.AccountId and lg.state <> 3 
							order by submittime desc), 0.0) startAccountAmount,
			(select isnull(sum(-lg.amount), 0.0) from deallogs lg where lg.AccountId = acc.AccountId and lg.dealtype = 103  and lg.state = 1 and (@start is null or lg.submittime >= @start) and (@end is null or lg.submittime < @end)) openAmount,
			(select isnull(sum(-lg.amount), 0.0) from deallogs lg where lg.AccountId = acc.AccountId and lg.dealtype = 102  and lg.state = 1 and (@start is null or lg.submittime >= @start) and (@end is null or lg.submittime < @end)) rechargingAmount,
			(select isnull(sum(lg.amount), 0.0) from deallogs lg where lg.AccountId = acc.AccountId and lg.dealtype in (1,4,8) and lg.state = 1 and (@start is null or lg.submittime >= @start) and (@end is null or lg.submittime < @end)) dealAmount,
			(select isnull(sum(lg.amount), 0.0) from deallogs lg where lg.AccountId = acc.AccountId and lg.dealtype = 104 and lg.state = 1 and (@start is null or lg.submittime >= @start) and (@end is null or lg.submittime < @end)) closeAmount,
			(select isnull(count(*), 0.0) from deallogs lg where lg.AccountId = acc.AccountId and lg.dealtype in (1,4,8) and lg.state = 1 and (@start is null or lg.submittime >= @start) and (@end is null or lg.submittime < @end)) dealCount,
			IsNull((select top 1 (lg.accountAmount + lg.Amount) endAccountAmount
							from deallogs lg 
							where  (@end is null or lg.submittime >= @end)
									and acc.AccountId = lg.AccountId and lg.state <> 3 
									order by submittime), acc.Amount) endAccountAmount
			from accounts acc 
				left join accountTypes accType on acc.AccountTypeId = accType.AccountTypeId
				left join users u on acc.ownerid = u.userid
		
			where (@accountShopId is null or acc.shopid = @accountShopId)
		
		) tmp
	