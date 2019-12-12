CREATE PROCEDURE dbo.sp_GetLikesByPost
	@postId int,
	@total int OUT
AS
SELECT @total = COUNT(PostId) FROM Likes WHERE PostId = @postId
