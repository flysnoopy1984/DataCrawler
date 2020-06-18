--delete from PersonInfo
--delete from BookInfo
--delete from DataSection
--delete from TagInfo

--delete from BookTag
--delete from BookSeries

--delete from [Plan_FromDouBanTagUrls]

select * from PersonInfo
select * from BookInfo order by id desc
select * from DataSection
select * from TagInfo
select * from BookTag
select * from BookSeries
select * from [dbo].[Plan_FromDouBanTagUrls] where ProcessPageIndex <1000

select count(1) from BookInfo

update Plan_FromDouBanTagUrls 
set ProcessPageIndex = 640 where id=290

select top 20 * from BookInfo
