select * from printtickets t where 
(@logtype is null or @logtype = t.logtype)
and (@accountName is null or @accountName = t.accountName)
and (@SerialNo is null or @SerialNo = t.SerialNo)