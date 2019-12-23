CREATE PROCEDURE dbo.sp_GetLikesByPost
	@postId int,
	@total int OUT
AS
SELECT @total = COUNT(PostId) FROM Likes WHERE PostId = @postId


--Para insertarlo por code-first añadiremos una migración en blanco y se insertará en el método Up y Down

--protected override void Up(MigrationBuilder migrationBuilder)
--{
--    var sp = "CREATE PROCEDURE dbo.sp_GetLikesByPost @postId int, @total int OUT AS BEGIN SELECT @total = COUNT(PostId) FROM Likes WHERE PostId = @postId END";
--    migrationBuilder.Sql(sp);
--}

--protected override void Down(MigrationBuilder migrationBuilder)
--{
--    migrationBuilder.Sql("DROP PROCEDURE dbo.sp_GetLikesByPost");
--}


