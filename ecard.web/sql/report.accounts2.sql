select accType.DisplayName as accountTypeName,
	isnull ((select sum(amount) from ReportDealLogDay where accType.accounttypeId = accounttypeid and dealtype = 103 and (@start is null or submitdate >= @start) and (@end is null or submitdate < @end)), 0) openAmount,
	isnull ((select sum(amount) from ReportDealLogDay where accType.accounttypeId = accounttypeid and dealtype = 102 and (@start is null or submitdate >= @start) and (@end is null or submitdate < @end)), 0) rechargingAmount,
	isnull ((select sum(amount) from ReportDealLogDay where accType.accounttypeId = accounttypeid and dealtype in (1, 4,8) and (@start is null or submitdate >= @start) and (@end is null or submitdate < @end)), 0) dealAmount,
	isnull ((select sum(amount) from ReportDealLogDay where accType.accounttypeId = accounttypeid and dealtype = 104 and (@start is null or submitdate >= @start) and (@end is null or submitdate < @end)), 0) closeAmount
from accountTypes accType
group by accType.AccountTypeId, accType.DisplayName
