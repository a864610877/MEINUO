select t.* from PointRebates t
where (@state is null or @state = t.State)
and (@AccountLevelPolicyId is null 
	or exists(select * from AccountLevelPointRebates g 
			inner join accountlevelpolicies p 
				on g.accountLevel = p.Level 
				and g.accountTypeId = p.accountTypeId 
				and p.state = 1 
				and p.AccountLevelPolicyId = @AccountLevelPolicyId
			 where t.pointRebateid = g.pointRebateid))