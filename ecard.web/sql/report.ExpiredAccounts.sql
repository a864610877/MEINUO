select acc.Name accountName, (select u.displayName  from users u where u.userid = acc.ownerid) userDisplayName,
acc.expiredMonths, Convert(nvarchar(20), acc.expiredDate, 23) expiredDate, acc.Amount
from accounts acc left join accounttypes accType on acc.accounttypeid = acctype.accounttypeid
where acc.ExpiredDate < @start
 