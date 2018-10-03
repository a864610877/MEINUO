select t.* from PointRebates t
where t.State = 1  
and exists(select * from AccountLevelPointRebates g  
			 where t.pointRebateid = g.pointRebateid 
			 and g.accountLevel = @accountLevel 
			 and g.AccountTypeId = @AccountTypeId)