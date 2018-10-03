select * from Commodities where 
(@NameStartWith is null or Name like  @NameStartWith + '%')
and (@DisplayNameWith is null or DisplayName like '%'+ @DisplayNameWith+'%') 
and (@NameWith is null or Name like '%'+@NameWith+'%') 
and (@state is null or state = @state)  
and (@Name is null or Name = @Name)  